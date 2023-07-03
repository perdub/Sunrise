namespace Sunrise;
using System.Web.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public class AspnetHoster{
    WebApplication app;

    public async Task StartApp(){
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<SunriseContext>();
        
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