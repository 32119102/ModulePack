
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{



    public static class ServiceExtensions
    {
        #region IServiceCollection
        /// <summary>
        /// 判断指定服务类型是否存在
        /// </summary>
        public static bool AnyServiceType(this IServiceCollection services, Type serviceType)
        {
            return services.Any(m => m.ServiceType == serviceType);
        }
        #endregion

        #region IServiceProvider

        /// <summary>
        /// 获取指定名称的日志对象
        /// </summary>
        public static ILogger GetLogger(this IServiceProvider provider, string name)
        {
            ILoggerFactory factory = provider.GetRequiredService<ILoggerFactory>();
            return factory.CreateLogger(name);
        }
        #endregion

    }
}
