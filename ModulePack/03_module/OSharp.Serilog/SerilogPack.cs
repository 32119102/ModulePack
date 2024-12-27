
using MP.Core.Packs;

using System.ComponentModel;


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Templates;



namespace OSharp.Serilog;


/// <summary>
/// Log4Net模块
/// </summary>
[Description("Log4Net日志模块")]
public class SerilogPack : OsharpPack
{
    /// <summary>
    /// 获取 模块级别，级别越小越先启动
    /// </summary>
    public override PackLevel Level => PackLevel.Application;

    /// <summary>
    /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
    /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
    /// </summary>
    public override int Order => 0;

    /// <summary>
    /// 将模块服务添加到依赖注入服务容器中
    /// </summary>
    /// <param name="services">依赖注入服务容器</param>
    /// <returns></returns>
    public override IServiceCollection AddServices(IServiceCollection services)
    {
        Console.WriteLine("SerilogPack->AddServices->我执行了");
        IConfiguration configuration = services.GetConfiguration();

        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console(new ExpressionTemplate(
        "{ {@t, @mt, @r, @l ,@p ,@i} }\n"))
        .CreateLogger();


        services.AddSerilog(Log.Logger);

        return services;
    }
}