using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MP.Core.Packs;
using OSharp.AspNetCore;
using System.ComponentModel;

namespace OSharp.DynamicApi;

/// <summary>
/// 动态API
/// </summary>
[Description("动态API")]
[DependsOnPacks(typeof(AspNetCorePack))]
public class DynamicApiPack : AspOsharpPack
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
        //新增动态weiApi
        //services.AddDynamicApi(options =>
        //{
        //    // 指定全局默认的 api 前缀
        //    //options.DefaultApiPrefix = "api";

        //    /**
        //     * 自定义 ActionName 处理函数;
        //     */
        //    options.GetRestFulActionName = (actionName) => actionName.Replace("Aaync", "");
        //    options.RemoveControllerPostfixes = new List<string>() { "Service" };
        //});


        services.AddDynamicApi((options) =>
        {
            // 指定全局默认的 api 前缀
            options.DefaultApiPrefix = "apis";

            /**
             * 清空API结尾，不删除API结尾;
             * 若不清空 CreatUserAsync 将变为 CreateUser
             */
            options.RemoveActionPostfixes.Clear();

            /**
             * 自定义 ActionName 处理函数;
             */
            options.GetRestFulActionName = (actionName) => actionName;

            /**
             * 指定程序集 配置 url 前缀为 apis
             * 如: http://localhost:8080/apis/User/CreateUser
             */
            options.AddAssemblyOptions(this.GetType().Assembly, apiPreFix: "apis");

            /**
             * 指定程序集 配置所有的api请求方式都为 POST
             */
            options.AddAssemblyOptions(this.GetType().Assembly, httpVerb: "POST");

            /**
             * 指定程序集 配置 url 前缀为 apis, 且所有请求方式都为POST
             * 如: http://localhost:8080/apis/User/CreateUser
             */
            options.AddAssemblyOptions(this.GetType().Assembly, apiPreFix: "apis", httpVerb: "POST");
        });

        return services;
    }




    /// <summary>
    /// 应用模块服务
    /// </summary>
    /// <param name="app">应用程序</param>
    public override void UsePack(WebApplication app)
    {


        IsEnabled = true;
    }

}
