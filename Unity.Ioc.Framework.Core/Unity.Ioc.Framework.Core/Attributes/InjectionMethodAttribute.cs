using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core.Attributes
{
    /// <summary>
    /// 用来要求容器创建对象后调用所标注的方法进行初始化.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InjectionMethodAttribute : Attribute
    {
    }
}
