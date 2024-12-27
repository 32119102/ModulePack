using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MP.Core.Packs;
using OSharp.AspNetCore;
using System;

namespace OSharp.Authentication;




/// <summary>
/// 身份认证模块基类
/// </summary>
[Description("身份认证模块")]
[DependsOnPacks(typeof(AspNetCorePack))]
public class AuthenticationPack : AspOsharpPack
{
    /// <summary>
    /// 获取 模块级别
    /// </summary>
    public override PackLevel Level => PackLevel.Application;

    /// <summary>
    /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动，
    /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
    /// </summary>
    public override int Order => 1;

    /// <summary>
    /// 将模块服务添加到依赖注入服务容器中
    /// </summary>
    /// <param name="services">依赖注入服务容器</param>
    /// <returns></returns>
    public override IServiceCollection AddServices(IServiceCollection services)
    {

        string defaultSchema = defaultSchema = JwtBearerDefaults.AuthenticationScheme;

        AuthenticationBuilder builder = services.AddAuthentication(opts =>
        {
            opts.DefaultScheme = defaultSchema;
            opts.DefaultAuthenticateScheme = defaultSchema;
        });
        AddJwtBearer(services, builder);
        return services;
    }





    /// <summary>
    /// 应用模块服务
    /// </summary>
    /// <param name="app">应用程序</param>
    public override void UsePack(WebApplication app)
    {
        app.UseAuthentication();

        IsEnabled = true;
    }

    /// <summary>
    /// 添加JwtBearer支持
    /// </summary>
    protected virtual AuthenticationBuilder AddJwtBearer(IServiceCollection services, AuthenticationBuilder builder)
    {


        builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
            opts =>
            {
                //opts.TokenValidationParameters = new TokenValidationParameters()
                //{
                //    ValidIssuer = "osharp identity",
                //    ValidAudience = "osharp client",
                //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dwqwdqw22121d1fcewf")),
                //    LifetimeValidator = (nbf, exp, token, param) => exp > DateTime.UtcNow
                //};


                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "osharp identity",
                    ValidAudience = "osharp client",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dwqwdqw22121d1fcewf")),
                    ClockSkew = TimeSpan.Zero
                };


            });

        return builder;
    }


}

