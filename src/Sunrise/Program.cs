using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace Sunrise;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel((kestrel) =>
        {
            kestrel.ListenAnyIP(3269);
            kestrel.Limits.MaxRequestBodySize = (512 * 1024 * 1024);
            kestrel.ConfigureEndpointDefaults((endpoints) =>
            {
                endpoints.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2AndHttp3;
            });
        });

        #region Config
        if (File.Exists("SunriseConfig.json"))
        {
            builder.Configuration.AddJsonFile("SunriseConfig.json");
        }
        else if (File.Exists("SunriseConfig.Example.json"))
        {
            builder.Configuration.AddJsonFile("SunriseConfig.Example.json");
        }
        #endregion

        builder.Services.AddRazorPages();

        builder.Services.AddScoped<Sunrise.Builders.SessionBuilder>();
        builder.Services.AddScoped<Sunrise.Builders.TagBuilder>();
        builder.Services.AddScoped<Sunrise.Storage.Storage>();

        builder.Services.AddResponseCompression(options =>
        {
            //really, website with anime images wont be attack with CRIME/BREACH (i`m hope)
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
        });
#region Set Compression Levels
        //maybe i should change CompressionLevel
        builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });

        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        {
           options.Level = CompressionLevel.SmallestSize;
        }   );
    #endregion

        //добавление контроллеров из Sunrise.Mvc
        builder.Services.AddMvc()
            .AddApplicationPart(Assembly.Load(new AssemblyName("Sunrise.Mvc")));

        builder.Services.AddDbContext<Sunrise.Database.SunriseContext>(ServiceLifetime.Transient);

        //регистрация политик ограничений запросов
        builder.Services.AddRateLimiter((options) =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            //ограничение на вызов эндпоинта для создания аккаунта (1 запрос в минуту)
            options.AddFixedWindowLimiter("AccountCreate", (fixedOptions) =>
            {
                fixedOptions.Window = TimeSpan.FromMinutes(1);
                fixedOptions.PermitLimit = 1;
            });
            options.AddFixedWindowLimiter("AccountLogin", (fixedOptions) =>
            {
                fixedOptions.Window = TimeSpan.FromSeconds(30);
                fixedOptions.PermitLimit = 1;
            });
        });



        var app = builder.Build();

        #region Ensure Create
        var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<Sunrise.Database.SunriseContext>();
        db.Database.EnsureCreated();
        db.Dispose();
        scope.Dispose();
        #endregion

        app.UseResponseCompression();

        app.UseMiddleware<Middleware.SessionMiddleware>();

        app.UseRouting();

        app.UseRateLimiter();

        app.UseStaticFiles();

        Sunrise.Types.File.SetConfiguration(app.Configuration);

        string storagePath = app.Configuration.GetValue<string>("StoragePath") ?? "/storage";
        Directory.CreateDirectory(storagePath);

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                storagePath
            ),
            RequestPath = app.Configuration.GetValue<string>("RequestPath"),
            OnPrepareResponse = (a) =>
            {
                a.Context.Response.Headers.CacheControl = $"public, max-age={60*60*24*30*6}";
            },
            ServeUnknownFileTypes = true
        });

        app.UseEndpoints((a) =>
        {
            a.MapControllers();
            a.MapRazorPages();
        });

        app.Run();
    }
}
