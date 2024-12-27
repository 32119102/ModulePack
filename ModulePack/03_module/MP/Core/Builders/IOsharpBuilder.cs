namespace MP.Core.Builders;

/// <summary>
/// 定义OSharp构建器
/// </summary>
public interface IOsharpBuilder
{
    /// <summary>
    /// 获取 服务集合
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// 获取 加载的模块集合
    /// </summary>
    IEnumerable<OsharpPack> Packs { get; }

    /// <summary>
    /// 添加指定模块
    /// </summary>
    /// <typeparam name="TPack">要添加的模块类型</typeparam>
    IOsharpBuilder AddPack<TPack>() where TPack : OsharpPack;

    /// <summary>
    /// 添加加载的所有Pack，并可排除指定的Pack类型
    /// </summary>
    /// <param name="exceptPackTypes">要排除的Pack类型</param>
    /// <returns></returns>
    IOsharpBuilder AddPacks(params Type[] exceptPackTypes);
}