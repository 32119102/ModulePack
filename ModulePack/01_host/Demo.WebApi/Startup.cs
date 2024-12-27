

using OSharp.Authentication;
using OSharp.Serilog;
using OSharp.Swagger;
using Panda.DynamicWebApi;
using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Demo.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //AddControllersAsServices 需要对service进行注入才行

            //services.AddControllers();
            services.AddOSharp()
                 //.AddPack<Log4NetPack>()
                 //.AddPack<SerilogPack>()
                 .AddPack<AuthenticationPack>()
                  .AddPack<DynamicWebApiPack>()
                 .AddPack<SwaggerPack>()

                 ;

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddEndpointsApiExplorer();

        }

        public void Configure(WebApplication app)
        {
            IWebHostEnvironment env = app.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHttpsRedirection();
            }
            //app.UseSwagger();
            //app.UseSwaggerUI();
            app //.UseMiddleware<JsonExceptionHandlerMiddleware>()
                .UseDefaultFiles()
                .UseStaticFiles();
            app.MapControllers();
            app.UseOSharp();
        }
    }
}