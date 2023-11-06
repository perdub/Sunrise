namespace Sunrise.Types;

using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Sunrise.Types.Enums;

public class File{
    public PostType PostType {get;init;} = Enums.PostType.Unknown;

    public string previewPath {get;init;}
    public string samplePath {get;init;}
    public bool isSampleExsist {get;init;} = false;
    public string fullPath {get;init;}

    public string Sha256{get;init;}

    public Post LinkedPost{get;private set;}
    public Guid PostId{get;private set;} = Guid.Empty;

    [Key]
    public Guid FileId {get;set;} = Guid.Empty;
    private File(){
        if(pathPrefix == null){
            pathPrefix = appConfiguration.GetValue<string>("RequestPath");
        }
    }

    public File(Post p) : this(){
        LinkedPost = p;
    }

    public string GetBaseLink(){
        if(isSampleExsist)
            return samplePath;
        return fullPath;
    }

    public string GetItemPath(ContentVariant variant){
        switch(variant){
            case ContentVariant.Preview:
                return pathPrefix + previewPath;
            case ContentVariant.Sample:
                if(isSampleExsist){
                    return pathPrefix + samplePath;
                }
                goto case ContentVariant.Original;
            case ContentVariant.Original:
                return pathPrefix + fullPath;
            default:
                throw new Exception("Unknown variant.");
        }
    }
    private static IConfiguration appConfiguration;
    public static void SetConfiguration(IConfiguration configuration){
        appConfiguration = configuration;
    }
    private static string? pathPrefix = null;
}