using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core
{
    /// <summary>
    /// 用来标注接口和实现类的映射关系.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceImplementationAttribute : Attribute
    {
        private readonly ICollection<Type> _interfaceTypes;
        private string _name;
        private Lifetime _lifetime;

        /// <summary>
        /// 表示将所标注的类实现的所有接口映射到所标注的类
        /// </summary>
        public ServiceImplementationAttribute()
        {
            _interfaceTypes = new List<Type>();
        }

        /// <summary>
        /// 表示将参数中指定的类型映射到所标注的类
        /// </summary>
        /// <param name="interfaceTypes">指定所标注的类应当注册为这些接口的实现</param>
        public ServiceImplementationAttribute(params Type[] interfaceTypes)
        {
            _interfaceTypes = interfaceTypes;
        }

        /// <summary>
        /// 指定注册对象时使用的名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 指定容器创建对象时使用的生命期
        /// </summary>
        public Lifetime ResolveLifetime
        {
            get { return _lifetime; }
            set { _lifetime = value; }
        }

        /// <summary>
        /// 返回由本标注确定的所有需要映射的接口类
        /// </summary>
        public ICollection<Type> InterfaceTypes
        {
            get { return _interfaceTypes; }
        }

    }
}
