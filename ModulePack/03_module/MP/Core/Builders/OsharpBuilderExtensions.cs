namespace MP.Core.Builders;

/// <summary>
/// IOSharpBuilder扩展方法
/// </summary>
public static class OsharpBuilderExtensions
{
    /// <summary>
    /// 添加核心模块
    /// </summary>
    internal static IOsharpBuilder AddCorePack(this IOsharpBuilder builder)
    {
        builder.AddPack<OsharpCorePack>()
            
            ;
        //.AddPack<DependencyPack>()
        //.AddPack<EventBusPack>();

        return builder;
    }
}