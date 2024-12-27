namespace MP.Reflection;

/// <summary>
/// 程序集管理器
/// </summary>
public static class AssemblyManager
{
    private static readonly string[] Filters = { "dotnet-", "Microsoft.", "mscorlib", "netstandard", "System", "Windows" };
    private static Assembly[] _allAssemblies;
    private static Type[] _allTypes;

    static AssemblyManager()
    {
        AssemblyFilterFunc = name =>
        {
            return name.Name != null && !Filters.Any(m => name.Name.StartsWith(m));
        };
    }

    /// <summary>
    /// 设置 程序集过滤器
    /// </summary>
    public static Func<AssemblyName, bool> AssemblyFilterFunc { private get; set; }


    /// <summary>
    /// 获取 所有类型
    /// </summary>
    public static Type[] AllTypes
    {
        get
        {
            if (_allTypes == null)
            {
                Init();
            }

            return _allTypes;
        }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        if (AssemblyFilterFunc == null)
        {
            throw new OsharpException("AssemblyManager.AssemblyFilterFunc 不能为空");
        }

        _allAssemblies = DependencyContext.Default.GetDefaultAssemblyNames()
            .Where(AssemblyFilterFunc).Select(Assembly.Load).ToArray();
        _allTypes = _allAssemblies.SelectMany(m => m.GetTypes()).ToArray();
    }




    /// <summary>
    /// 查找指定基类的实现类型
    /// </summary>
    public static Type[] FindTypesByBase<TBaseType>()
    {
        Type baseType = typeof(TBaseType);
        return FindTypesByBase(baseType);
    }

    /// <summary>
    /// 查找指定基类的实现类型
    /// </summary>
    public static Type[] FindTypesByBase(Type baseType)
    {
        return AllTypes.Where(type => type.IsDeriveClassFrom(baseType)).Distinct().ToArray();
    }
}
