using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core
{
    /// <summary>
    /// 容器初始化过程中提供的扩展点.
    /// 各模块可通过 ServiceImplementation 标注注册一个此接口的实现类(需要注册成命名实例).
    /// 那么, 在容器初始化期间, 将会调用一次该实现类的InitContainer方法.
    /// 此方法的实现过程当中可通过传入的container引用, 动态向当前容器注册实现类或实例等映射关系.
    /// </summary>
    public interface IContainerInitializer
    {
        /// <summary>
        /// 在此方法中利用 <paramref name="container"/> 提供的方法进行映射注册。
        /// </summary>
        /// <param name="container">实现类可通过由此参数传入的IContainer对象引用调用其方法</param>
        void InitContainer(IContainer container);
    }
}
