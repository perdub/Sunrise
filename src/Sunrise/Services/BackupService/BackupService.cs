using System.Text;
using System.Linq;
using System.Diagnostics;
using Sunrise.Database;
using Microsoft.EntityFrameworkCore;

namespace Sunrise.Services;
#pragma warning disable CS8652
public class BackupService(SunriseContext context, ILogger<BackupService> logger, IConfiguration configuration) : IHostedService
#pragma warning restore 
{
    private string globalBackupPath = string.Empty;
    private string thisBackupPath = string.Empty;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        //Backup(cancellationToken);
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {


        return Task.CompletedTask;
    }

    private async Task Backup(CancellationToken token)
    {
        globalBackupPath = configuration.GetValue<string>("BackupDir");
        thisBackupPath = Path.Combine(globalBackupPath, $"{DateTime.UtcNow.ToString("yyyyMMddhhmmss")}");
        Directory.CreateDirectory(thisBackupPath);
        List<Task> tasks = new();
        tasks.Add(Task.Run(CopyConfig));
        tasks.Add(Task.Run(DumpDb));
        tasks.Add(Task.Run(DumpContent));

        await Task.WhenAll(tasks);
    }

    private Task CopyConfig()
    {
        File.Copy(
            Path.GetFullPath("SunriseConfig.json"),
            Path.Combine(thisBackupPath, "SunriseConfig.json")
        );
        File.Copy(
            Path.GetFullPath("SunriseConfig.Example.json"),
            Path.Combine(thisBackupPath, "SunriseConfig.Example.json")
        );
        logger.LogDebug("Config copied");
        return Task.CompletedTask;
    }
    private async Task DumpDb()
    {
        StringBuilder sb = new StringBuilder();

        //parce db connection string
        string connectionString = configuration.GetValue<string>("ConnectionString");
        var p = connectionString.Split(';');
        var dictionary = new Dictionary<string, string>();
        foreach (var s in p)
        {
            var tt = s.Split('=');
            dictionary.Add(tt[0], tt[1]);
        }

        //combine command
        sb.Append("-U ");
        sb.Append(dictionary["Username"]);
        sb.Append(" -h ");
        sb.Append(dictionary["Host"]);
        sb.Append(" -p ");
        sb.Append(dictionary["Port"]);
        sb.Append(" -w -F p -d ");
        sb.Append(dictionary["Database"]);

        logger.LogTrace(sb.ToString());

        //create process
        Process pr = new Process();
        pr.StartInfo = new ProcessStartInfo
        {
            Arguments = sb.ToString(),
            UseShellExecute = false,
            CreateNoWindow = true,
            FileName = "pg_dump",
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };
        //set postgres password
        pr.StartInfo.EnvironmentVariables.Add("PGPASSWORD", dictionary["Password"]);

        pr.Start();
        var so = pr.StandardOutput;

        so.ReadLine();

        await Task.Delay(100);

        pr.StandardInput.WriteLine(dictionary["Password"]);

        var fs = File.Create(Path.Combine(thisBackupPath, "db.sql"));
        StreamWriter sw = new StreamWriter(fs);
        while (!so.EndOfStream)
        {
            var l = so.ReadLine();
            sw.WriteLine(l);
        }

        sw.Flush();
        fs.Flush();
        sw.Dispose();
        fs.Dispose();

        await pr.WaitForExitAsync();
        logger.LogDebug("Database dump ended.");

    }

    //we are going dump only original files because we can convert it again
    private async Task DumpContent()
    {
        var files = context.Files
            .AsNoTracking()
            .Select(f => f.fullPath)
            .AsAsyncEnumerable();

        string t = Path.Combine(thisBackupPath, "files");
        Directory.CreateDirectory(t);
        string storage = configuration.GetValue<string>("StoragePath");
        List<Task> copyTasks = new (context.Files.Count());
        await foreach (var f in files)
        {
            try
            {
                var ff = f[1..];
                string source = Path.Combine(storage, ff);
                string dest = Path.Combine(t, ff);
                var d = Path.GetDirectoryName(dest);
                Directory.CreateDirectory(d);
                copyTasks.Add(CopyFileAsync(source, dest));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        await Task.WhenAll(copyTasks);
        logger.LogDebug("End dump content.");
    }
    public async Task CopyFileAsync(string sourceFile, string destinationFile)
    {
        int buffersize = 1024 * 64;
        using (var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, buffersize, FileOptions.Asynchronous | FileOptions.SequentialScan))
        using (var destinationStream = new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, buffersize, FileOptions.Asynchronous | FileOptions.SequentialScan))
            await sourceStream.CopyToAsync(destinationStream);
    }
}