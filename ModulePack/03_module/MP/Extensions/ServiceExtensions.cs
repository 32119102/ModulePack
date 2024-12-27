using Microsoft.Extensions.Configuration;
using MP.Core.Builders;
using MP.Data;
using MP.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions2
{
    #region IServiceProvider
    /// <summary>
    /// 获取所有模块信息
    /// </summary>
    public static OsharpPack[] GetAllPacks(this IServiceProvider provider)
    {
        OsharpPack[] packs = provider.GetServices<OsharpPack>().OrderBy(m => m.Level).ThenBy(m => m.Order).ThenBy(m => m.GetType().FullName).ToArray();
        return packs;
    }

    #endregion

    #region IServiceCollection

    /// <summary>
    /// 创建OSharp构建器，开始构建OSharp服务
    /// </summary>
    public static IOsharpBuilder AddOSharp(this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        //初始化所有程序集查找器
        services.GetOrAddSingletonInstance(() => new StartupLogger());

        IOsharpBuilder builder = services.GetOrAddSingletonInstance<IOsharpBuilder>(() => new OSharpBuilder(services));
        builder.AddCorePack();



        return builder;
    }

    /// <summary>
    /// 获取<see cref="IConfiguration"/>配置信息
    /// </summary>
    public static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        return services.GetSingletonInstanceOrNull<IConfiguration>();
    }


    /// <summary>
    /// 获取单例注册服务对象
    /// </summary>
    public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
    {
        ServiceDescriptor descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T) && d.Lifetime == ServiceLifetime.Singleton);

        if (descriptor?.ImplementationInstance != null)
        {
            return (T)descriptor.ImplementationInstance;
        }

        if (descriptor?.ImplementationFactory != null)
        {
            return (T)descriptor.ImplementationFactory.Invoke(null);
        }

        return default;
    }


    /// <summary>
    /// 如果指定服务不存在，创建实例并添加
    /// </summary>
    public static TServiceType GetOrAddSingletonInstance<TServiceType>(this IServiceCollection services, Func<TServiceType> factory) where TServiceType : class
    {
        TServiceType item = services.GetSingletonInstanceOrNull<TServiceType>();
        if (item == null)
        {
            item = factory();
            services.AddSingleton<TServiceType>(item);
            services.ServiceLogDebug(typeof(TServiceType), item.GetType(), "ServiceExtensions");
        }
        return item;
    }

    /// <summary>
    /// 添加启动消息日志
    /// </summary>
    public static IServiceCollection LogInformation(this IServiceCollection services, string message, string logName)
    {
        StartupLogger logger = services.GetOrAddSingletonInstance(() => new StartupLogger());
        logger.LogInformation(message, logName);
        return services;
    }

    /// <summary>
    /// 添加服务调试日志
    /// </summary>
    public static IServiceCollection ServiceLogDebug(this IServiceCollection services, ServiceDescriptor[] oldDescriptors, string logName)
    {
        var list = services.Except(oldDescriptors);
        foreach (ServiceDescriptor desc in list)
        {
            if (desc.ImplementationType != null)
            {
                services.ServiceLogDebug(desc.ServiceType, desc.ImplementationType, logName, desc.Lifetime);
                continue;
            }

            if (desc.ImplementationInstance != null)
            {
                services.ServiceLogDebug(desc.ServiceType, desc.ImplementationInstance.GetType(), logName, desc.Lifetime);
            }
        }

        return services;
    }

    /// <summary>
    /// 添加服务调试日志
    /// </summary>
    public static IServiceCollection ServiceLogDebug(this IServiceCollection services, Type serviceType, Type implementType, string logName, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        string lifetimeType = lifetime == ServiceLifetime.Singleton ? "单例" : lifetime == ServiceLifetime.Scoped ? "作用域" : "瞬时";
        return services.LogDebug($"添加服务，{lifetimeType}，{serviceType.FullName} -> {implementType.FullName}", logName);
    }

    /// <summary>
    /// 添加启动调试日志
    /// </summary>
    public static IServiceCollection LogDebug(this IServiceCollection services, string message, string logName)
    {
        StartupLogger logger = services.GetOrAddSingletonInstance(() => new StartupLogger());
        logger.LogDebug(message, logName);
        return services;
    }

    #endregion





}