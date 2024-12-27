using Microsoft.AspNetCore.Builder;
using MP.Core.Packs;


namespace OSharp.AspNetCore;

/// <summary>
///  基于AspNetCore环境的Pack模块基类
/// </summary>
public abstract class AspOsharpPack : OsharpPack
{
    /// <summary>
    /// 应用AspNetCore的服务业务
    /// </summary>
    /// <param name="app">Web应用程序</param>
    public virtual void UsePack(WebApplication app)
    {
        IServiceProvider provider = app.Services;
        base.UsePack(provider);
    }
}
