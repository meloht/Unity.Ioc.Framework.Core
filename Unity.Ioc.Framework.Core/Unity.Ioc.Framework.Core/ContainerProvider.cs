using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core
{
    /// <summary>
    /// 此委托类型定义的方法, 被 <see cref="Container"/>
    /// 静态类用来获取当前容器对象.
    /// </summary>
    /// <returns><see cref="IContainer"/>的一个实例.</returns>
    public delegate IContainer ContainerProvider();
}
