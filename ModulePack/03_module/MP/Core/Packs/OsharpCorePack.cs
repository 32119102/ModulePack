﻿using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MP.Logging;

namespace MP.Core.Packs;


/// <summary>
/// OSharp核心模块
/// </summary>
[Description("OSharp核心模块")]
public class OsharpCorePack : OsharpPack
{
    /// <summary>
    /// 获取 模块级别
    /// </summary>
    public override PackLevel Level => PackLevel.Core;

    /// <summary>
    /// 将模块服务添加到依赖注入服务容器中
    /// </summary>
    /// <param name="services">依赖注入服务容器</param>
    /// <returns></returns>
    public override IServiceCollection AddServices(IServiceCollection services)
    {
        //services.TryAddSingleton<IConfigureOptions<OsharpOptions>, OsharpOptionsSetup>();
        //services.TryAddSingleton<ICancellationTokenProvider, NoneCancellationTokenProvider>();
        //services.TryAddSingleton<IEmailSender, DefaultEmailSender>();
        //services.TryAddSingleton<StartupLogger>();

        //services.TryAddSingleton<ICacheService, CacheService>();
        //services.TryAddScoped<IFilterService, FilterService>();
        //services.TryAddScoped<IKeyValueStore, KeyValueStore>();

        //services.TryAddTransient<IClientHttpCrypto, ClientHttpCrypto>();
        //services.AddTransient<ClientHttpCryptoHandler>();

        //services.AddDistributedMemoryCache();

        return services;
    }
}