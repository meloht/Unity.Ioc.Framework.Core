using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core.Attributes
{
    /// <summary>
    /// 用来标注需要容器解析的外部依赖.
    /// </summary>
    /// <remarks>适用于可写属性和方法参数</remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class DependencyAttribute : Attribute
    {
        private readonly string _name;

        /// <summary>
        /// 表示容器应当获取无命名的默认对象
        /// </summary>
        public DependencyAttribute()
        {
        }

        /// <summary>
        /// 表示容器应当按 <paramref name="name"/> 获取命名对象
        /// </summary>
        /// <param name="name"></param>
        public DependencyAttribute(string name)
        {
            _name = name;
        }

        /// <summary>
        /// 指定需要通过容器获取的命名对象的名称
        /// </summary>
        public string ResolveName
        {
            get { return _name; }
        }

    }
}
