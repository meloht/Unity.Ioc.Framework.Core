using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core.Attributes
{
    /// <summary>
    /// 用来明确指定容器创建对象时使用的构造函数, 而不是按照默认规则
    /// (使用参数表最长的一个)来确定.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class InjectionConstructorAttribute : Attribute
    {
    }
}
