namespace Sunrise;

using System.Net.Http.Headers;

public class AspnetHoster{
    WebApplication app;

    public async Task StartApp(string[] args){
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel((x, y)=>{
            y.ListenAnyIP(3268);
            y.Limits.MaxRequestBodySize = (512*1024*1024);
        });

        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<SunriseContext>((x)=>{}, ServiceLifetime.Transient, ServiceLifetime.Scoped);
        builder.Services.AddSingleton<Random>(new Random());
        
        builder.Services.AddControllers();

        builder.Services.AddCors((x)=>{
            x.AddPolicy("corslocalapi",(y)=>{
                y.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
            });
        });

        

        
        app = builder.Build();

        app.UseMiddleware<Middleware.LoginMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseStaticFiles(
            new StaticFileOptions{
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    Path.Combine(
                        builder.Environment.ContentRootPath,
                        "storage"
                    )
                ),
                RequestPath = "/storage",
                OnPrepareResponse = (x)=>{
                    x.Context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue{
                        Public = true,
                        MaxAge = TimeSpan.FromDays(30.5)
                    };
                }
            }
        );

        app.UseRouting();
        app.UseCors("corslocalapi");
        app.UseEndpoints((x)=>{
            x.MapControllers();
        });

        app.UseAuthorization();

        app.MapRazorPages();

        

        app.Run();
    }
}