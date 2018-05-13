using System;
using System.Collections.Generic;
using System.Text;
using Unity.Ioc.Framework.Core.Unity.ContainerAdapter;

namespace Unity.Ioc.Framework.Core
{
    /// <summary>
    /// 实现底层容器的加载替换. 提供当前容器对象实例引用.
    /// </summary>
    /// <remarks>静态接口部分已由ServiceLocator替代</remarks>
    public static class Container
    {
        #region 通过委托初始化, 运行时可替换获取过程

        /// <summary>
        /// This class provides the ambient container for this application. If your
        /// framework defines such an ambient container, use ServiceLocator.Current
        /// to get it.
        /// </summary>
        private static ContainerProvider _currentProvider;

        /// <summary>
        /// 公用的IContainer实例
        /// </summary>
        private static IContainer _currentInstance;

        /// <summary>
        /// 初始化默认实例
        /// </summary>
        static Container()
        {
            SetLocatorProvider(delegate
            {
                if (_currentInstance == null)
                    _currentInstance = new UnityContainerImpl(new UnityContainer());
                return _currentInstance;
            });
        }

        /// <summary>
        /// 设置用来获取当前容器的方法委托实例
        /// </summary>
        /// <param name="newProvider">此委托被调用时, 应当返回当前容器对象的引用</param>
        public static void SetLocatorProvider(ContainerProvider newProvider)
        {
            _currentProvider = newProvider;
        }

        #endregion

        /// <summary>
        /// 当前容器对象.
        /// </summary>
        public static IContainer Current
        {
            get { return _currentProvider(); }
        }
    }
}
