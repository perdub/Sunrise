using Sunrise.Database;
using Sunrise.Types;
using Sunrise.Types.Enums;
using Sunrise.Convert;
using Sunrise.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sunrise.Storage;

public class Storage
{
    private SunriseContext _context;
    private IConfiguration _config;
    private TagBuilder _tagBuilder;
    private string globalPathPrefix;

    private ILogger<VideoConverter> _convertLogger;

    public Storage(SunriseContext context, IConfiguration config, TagBuilder tagBuilder, ILogger<VideoConverter> videoLogger)
    {
        _context = context;
        _config = config;
        _tagBuilder = tagBuilder;
        globalPathPrefix = _config.GetValue<string>("StoragePath");
        Directory.CreateDirectory(globalPathPrefix);
        _convertLogger = videoLogger;
    }

    private string jsonUploadResult(string message = null){
        if(message is null){
            return "{\"result\": true}";
        }
        return "{\"result\": false, \"error\": \""+message+"\"}";
    }

    public async Task<(int httpCode, string message)> SavePost(byte[] post,
                               string sessionKey,
                               string[] tags){
        
        try
        {

        if(post is null || post.Length == 0){
            return (400, jsonUploadResult("No file."));
        }

        var account = _context.Sessions
            .Include(a=>a.Account)
            .Where(a=>a.SessionId == sessionKey)
            .FirstOrDefault().Account;

        if(account is null){
            return (401, jsonUploadResult("Unauthorized/Bad session key."));
        }

        Format format = post.GetFileFormat();

        var iHash = SharpHash.Base.HashFactory.Crypto.CreateSHA2_256();
        var hashResult = iHash.ComputeBytes(post);
        string fileHash = hashResult.ToString();

        _context.Files.Where(a=>a.Sha256 == fileHash).FirstOrDefault();
        if(_context.Files.Where(a=>a.Sha256 == fileHash).FirstOrDefault() != null){
            return (409, jsonUploadResult("File with same hash already exists."));
        }

        AbstractConverter converter;
        switch(format.Type){
            case PostType.Image:
                converter = new ImageConverter(globalPathPrefix, post, format.fileExtension);
                break;

            case PostType.Video:
                converter = new VideoConverter(globalPathPrefix, post, format.fileExtension, (l)=>{
                    _convertLogger.LogTrace(l);
                });
                break;

            case PostType.Gif:
                converter = new AnimationsConverter(globalPathPrefix, post, format.fileExtension, (l)=>{
                    _convertLogger.LogTrace(l);
                });
                break;

            default:
                return (415, jsonUploadResult("Unsupported file type."));
        }
        var paths = converter.Convert();
        paths = paths.DeletePrefix(globalPathPrefix);

        Post newPost = new Post(account);

        _context.Posts.Add(newPost);

        var Tags = await _tagBuilder.GetTags(tags);
        _context.Tags.UpdateRange(Tags);
        foreach(var tag in Tags){
            newPost.Tags.Add(tag);
            tag.Posts.Add(newPost);
            tag.PostCount++;
        }

        Types.File newFile = new Types.File(newPost){
            isSampleExsist = paths.sampleExsist,
            samplePath = paths.samplePath,
            previewPath = paths.previewPath,
            fullPath = paths.originalPath,
            Sha256 = fileHash,
            PostType = format.Type
        };

        _context.Files.Add(newFile);
        
        await _context.SaveChangesAsync();

        return (200, jsonUploadResult());
        }
        catch(Exception ex){
            #if DEBUG
            return (500, jsonUploadResult(ex.Message));
            #endif
            return (500, jsonUploadResult("Internal server error."));
        }
    }
}
