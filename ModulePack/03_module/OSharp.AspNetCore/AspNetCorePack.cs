﻿using Microsoft.Extensions.DependencyInjection;
using MP.Core.Packs;
using System.ComponentModel;
using System.Security.Principal;

namespace OSharp.AspNetCore;

/// <summary>
/// AspNetCore模块
/// </summary>
[Description("AspNetCore模块")]
public class AspNetCorePack : OsharpPack 
{
    /// <summary>
    /// 获取 模块级别
    /// </summary>
    public override PackLevel Level => PackLevel.Core;

    /// <summary>
    /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动
    /// </summary>
    public override int Order => 2;


    /// <summary>
    /// 将模块服务添加到依赖注入服务容器中
    /// </summary>
    /// <param name="services">依赖注入服务容器</param>
    /// <returns></returns>
    public override IServiceCollection AddServices(IServiceCollection services)
    {
        //上下文
        services.AddHttpContextAccessor();

      

        return services;
    }

}