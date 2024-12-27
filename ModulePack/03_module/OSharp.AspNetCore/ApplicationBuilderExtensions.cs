using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MP.Core.Packs;
using MP.Logging;
using MP.Reflection;
using OSharp.AspNetCore;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Builder;




/// <summary>
/// <see cref="IApplicationBuilder"/>辅助扩展方法
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// OSharp框架初始化，适用于AspNetCore环境
    /// </summary>
    public static WebApplication UseOSharp(this WebApplication app)
    {
        IServiceProvider provider = app.Services;
        ILogger logger = provider.GetLogger("ApplicationBuilderExtensions");
        try
        {
            logger.LogInformation(0, "OSharp框架初始化开始");

            // 输出注入服务的日志
            StartupLogger startupLogger = provider.GetService<StartupLogger>();
            startupLogger?.Output(provider);

            Stopwatch watch = Stopwatch.StartNew();
            OsharpPack[] packs = provider.GetAllPacks();
            logger.LogInformation($"共有 {packs.Length} 个Pack模块需要初始化");
            foreach (OsharpPack pack in packs)
            {
                Type packType = pack.GetType();
                string packName = packType.GetDescription();
                logger.LogInformation($"正在初始化模块 “{packName} ({packType.Name})”");
                if (pack is AspOsharpPack aspPack)
                {
                    aspPack.UsePack(app);
                }
                else
                {
                    pack.UsePack(provider);
                }
                logger.LogInformation($"模块 “{packName} ({packType.Name})” 初始化完成\n");
            }

            watch.Stop();
            logger.LogInformation(0, $"OSharp框架初始化完成，耗时：{watch.Elapsed}\r\n");

            return app;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

}
