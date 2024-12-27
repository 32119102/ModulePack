using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Reflection;


[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public class OrderAttribute : Attribute
{
    /// <summary>
    /// 排序值
    /// </summary>
    public int Value { get; set; } = 0;

    /// <summary>
    /// 排序设置
    /// </summary>
    /// <param name="value">排序值</param>
    public OrderAttribute(int value)
    {
        Value = value;
    }
}
