using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MP.Core.Packs;
using MP.Reflection;
using OSharp.AspNetCore;
using OSharp.Swagger.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Swagger;


/// <summary>
/// 身份认证模块基类
/// </summary>
[Description("身份认证模块")]
[DependsOnPacks(typeof(AspNetCorePack))]
public abstract class SwaggerPackBase : AspOsharpPack
{
    /// <summary>
    /// 获取 模块级别，级别越小越先启动
    /// </summary>
    public override PackLevel Level => PackLevel.Application;

    /// <summary>
    /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
    /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
    /// </summary>
    public override int Order => 2;


    public override IServiceCollection AddServices(IServiceCollection services)
    {

        services.AddSwaggerGen(c =>
        {



            c.SwaggerDoc("Sys", new OpenApiInfo
            {
                Title = $"基础管理(Sys)",
                Version = "1.0",
                Description = "基础管理"
            });

            string[] xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            if (xmlFiles.Length > 0)
            {
                foreach (var xmlFile in xmlFiles)
                {
                    c.IncludeXmlComments(xmlFile, true);
                }
            }

            //c.CustomOperationIds(apiDesc =>
            //{
            //    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
            //    return controllerAction.ActionName;
            //});

            c.DocInclusionPredicate((docName, description) => true);
            ////动态webapi需要
            //c.DocInclusionPredicate((docName, description) =>
            //{
            //    var nonGroup = false;
            //    var groupNames = new List<string>();
            //    var dynamicApiAttribute = description.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is DynamicApiAttribute);
            //    if (dynamicApiAttribute != null)
            //    {
            //        var dynamicApi = dynamicApiAttribute as DynamicApiAttribute;
            //        if (dynamicApi.Area?.Length > 0)
            //        {
            //            groupNames.Add(dynamicApi.Area);
            //        }
            //    }
            //    return docName == description.GroupName || groupNames.Any(a => a == docName) || nonGroup;

            //});


            //var securityScheme = new OpenApiSecurityScheme
            //{
            //    Description = "JWT认证请求头格式: \"Authorization: Bearer {token}\"",
            //    Name = "Authorization",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.ApiKey,
            //    Scheme = "Bearer"
            //};

            ////添加设置Token的按钮
            //c.AddSecurityDefinition("Bearer", securityScheme);


            //地址和描述,文档暴露给外面的
            //var server = new OpenApiServer()
            //{
            //    Url = "www.xxxx.com",
            //    Description = "无敌文档"
            //};
            //server.Extensions.Add("extensions", new OpenApiObject
            //{
            //    ["copyright"] = new OpenApiString("201944")
            //});
            //c.AddServer(server);
            //c.CustomOperationIds(apiDesc =>
            //{
            //    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
            //    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
            //});

            //添加Jwt验证设置,如果说无授权则不加锁
            c.OperationFilter<AddAuthHeaderOperationFilter>();
            //隐藏  httpget 请求的参数,主要是分页参数
            c.OperationFilter<SwaggerJsonIgnoreFilter>();

            //枚举
            c.SchemaFilter<EnumSchemaFilter>();


            //排序
            c.DocumentFilter<OrderTagsDocumentFilter>();


            ////控制器排序
            //c.OrderActionsBy(apiDesc =>
            //{
            //    var order = 0;
            //    var objOrderAttribute = apiDesc.CustomAttributes().FirstOrDefault(x => x is OrderAttribute);
            //    if (objOrderAttribute != null)
            //    {
            //        var orderAttribute = objOrderAttribute as OrderAttribute;
            //        order = orderAttribute.Value;
            //    }
            //    return order.ToString();
            //});

        });




        return services;
    }



    /// <summary>
    /// 应用AspNetCore的服务业务
    /// </summary>
    /// <param name="app">Asp应用程序构建器</param>
    public override void UsePack(WebApplication app)
    {
        app.UseSwagger();
        //app.UseSwaggerUI(c =>
        //{
        //    c.SwaggerEndpoint($"/swagger/Sys/swagger.json", $"基础管理(Sys)");

        //    //启用过滤
        //    c.EnableFilter();
        //    //c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        //    //model删除
        //    c.DefaultModelsExpandDepth(-1);
        //    c.RoutePrefix = string.Empty;
        //});


        app.UseKnife4UI(c =>
        {
            c.SwaggerEndpoint($"/swagger/Sys/swagger.json", $"基础管理(Sys)");
            c.ConfigObject.DefaultModelsExpandDepth = -1;
            c.ConfigObject.DefaultModelExpandDepth = -1;
            ////启用过滤
            ////c.EnableFilter();
            //c.DocExpansion(DocExpansion.None);
            ////model删除
            //c.DefaultModelsExpandDepth(-1);
            c.RoutePrefix = string.Empty;

            //Knife4UIOptions 一些数据需要动态传
        });

        IsEnabled = true;
    }
}

