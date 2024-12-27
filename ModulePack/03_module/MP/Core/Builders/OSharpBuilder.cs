
namespace MP.Core.Builders;

/// <summary>
/// 构建器
/// </summary>
public class OSharpBuilder : IOsharpBuilder
{
    private readonly List<OsharpPack> _source;
    private List<OsharpPack> _packs;

    /// <summary>
    /// 获取 服务集合
    /// </summary>
    public IServiceCollection Services { get; }


    /// <summary>
    /// 初始化一个<see cref="OsharpBuilder"/>类型的新实例
    /// </summary>
    public OSharpBuilder(IServiceCollection services)
    {
        Services = services;
        _source = GetAllPacks(services);
        _packs = new List<OsharpPack>();



        if (!services.AnyServiceType(typeof(ILoggerFactory)))
        {
            services.AddLogging(opts =>
            {
#if DEBUG
                opts.SetMinimumLevel(LogLevel.Debug);
#else
                opts.SetMinimumLevel(LogLevel.Information);
#endif
            });
        }


    }


    private static List<OsharpPack> GetAllPacks(IServiceCollection services)
    {
        //需要排除已经被继承的Pack实类
        Type[] types = AssemblyManager.FindTypesByBase<OsharpPack>();
        Type[] packTypes = types.Select(m => m.BaseType).Where(m => m != null && !m.IsAbstract).ToArray();
        packTypes = types.Except(packTypes).ToArray();

        return packTypes.Select(m => (OsharpPack)Activator.CreateInstance(m))
            .OrderBy(m => m.Level).ThenBy(m => m.Order).ThenBy(m => m.GetType().FullName).ToList();
    }




    private OSharpBuilder AddPack(Type type)
    {
        if (!type.IsBaseOn(typeof(OsharpPack)))
        {
            throw new OsharpException($"要加载的Pack类型“{type}”不派生于基类 OsharpPack");
        }

        if (_packs.Any(m => m.GetType() == type))
        {
            return this;
        }

        OsharpPack[] tmpPacks = new OsharpPack[_packs.Count];
        _packs.CopyTo(tmpPacks);
        OsharpPack pack = _source.FirstOrDefault(m => m.GetType() == type);
        if (pack == null)
        {
            throw new OsharpException($"类型为“{type.FullName}”的模块实例无法找到");
        }
        _packs.AddIfNotExist(pack);

        // 添加依赖模块
        Type[] dependTypes = pack.GetDependPackTypes();
        foreach (Type dependType in dependTypes)
        {
            OsharpPack dependPack = _source.Find(m => m.GetType() == dependType);
            if (dependPack == null)
            {
                throw new OsharpException($"加载模块{pack.GetType().FullName}时无法找到依赖模块{dependType.FullName}");
            }
            _packs.AddIfNotExist(dependPack);
        }

        // 按先层级后顺序的规则进行排序
        _packs = _packs.OrderBy(m => m.Level).ThenBy(m => m.Order).ToList();

        string logName = typeof(OSharpBuilder).FullName;
        tmpPacks = _packs.Except(tmpPacks).ToArray();
        foreach (OsharpPack tmpPack in tmpPacks)
        {
            Type packType = tmpPack.GetType();
            string packName = packType.GetDescription();
            Services.LogInformation($"添加模块 “{packName} ({packType.Name})” 的服务", logName);
            ServiceDescriptor[] tmp = Services.ToArray();
            AddPack(Services, tmpPack);
            Services.ServiceLogDebug(tmp, packType.FullName);
            Services.LogInformation($"模块 “{packName} ({packType.Name})” 的服务添加完毕，添加了 {Services.Count - tmp.Length} 个服务\n", logName);
        }

        return this;
    }


    private static IServiceCollection AddPack(IServiceCollection services, OsharpPack pack)
    {
        Type type = pack.GetType();
        Type serviceType = typeof(OsharpPack);

        if (type.BaseType?.IsAbstract == false)
        {
            //移除多重继承的模块
            ServiceDescriptor[] descriptors = services.Where(m =>
                m.Lifetime == ServiceLifetime.Singleton && m.ServiceType == serviceType
                && m.ImplementationInstance?.GetType() == type.BaseType).ToArray();
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }
        }

        if (!services.Any(m => m.Lifetime == ServiceLifetime.Singleton && m.ServiceType == serviceType && m.ImplementationInstance?.GetType() == type))
        {
            services.AddSingleton(typeof(OsharpPack), pack);
            pack.AddServices(services);
        }

        return services;
    }


    /// <summary>
    /// 添加指定模块
    /// </summary>
    /// <typeparam name="TPack">要添加的模块类型</typeparam>
    public IOsharpBuilder AddPack<TPack>() where TPack : OsharpPack
    {
        Type type = typeof(TPack);
        return AddPack(type);
    }


    public IOsharpBuilder AddPacks(params Type[] exceptPackTypes)
    {
        throw new NotImplementedException();
    }




    /// <summary>
    /// 获取 加载的模块集合
    /// </summary>
    public IEnumerable<OsharpPack> Packs => _packs;






}
