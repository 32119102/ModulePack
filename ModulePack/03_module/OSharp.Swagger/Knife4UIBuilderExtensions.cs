﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Swagger;



public static class Knife4UIBuilderExtensions
{
    public static IApplicationBuilder UseKnife4UI(this IApplicationBuilder app, Knife4UIOptions options)
    {
        return app.UseMiddleware<Knife4jUIMiddleware>(options);
    }
    public static IApplicationBuilder UseKnife4UI(
         this IApplicationBuilder app,
         Action<Knife4UIOptions> setupAction = null)
    {
        var options = new Knife4UIOptions();
        using (var scope = app.ApplicationServices.CreateScope())
        {
            options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<Knife4UIOptions>>().Value;
            setupAction?.Invoke(options);
        }

        if (options.ConfigObject.Urls == null)
        {
            var hostingEnv = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            options.ConfigObject.Urls = new[] { new UrlDescriptor { Name = $"{hostingEnv.ApplicationName} v1", Url = "v1/swagger.json" } };
        }
        return app.UseKnife4UI(options);
    }
}