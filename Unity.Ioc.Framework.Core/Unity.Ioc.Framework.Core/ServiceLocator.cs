using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core
{
    /// <summary>
    /// 以static方法的形式, 提供ServiceLocator模式的对外接口.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// 获取容器中为 <typeparamref name="TService"/> 类型配置的默认实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <returns>实现服务接口的对象实例</returns>
        public static TService GetInstance<TService>()
        {
            return Container.Current.GetInstance<TService>();
        }

        /// <summary>
        /// 按照名称获取容器中为 <typeparamref name="TService"/> 类型配置的实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <returns>实现服务接口的对象实例</returns>
        public static TService GetInstance<TService>(string name)
        {
            return Container.Current.GetInstance<TService>(name);
        }

        /// <summary>
        /// 获取容器中为 <typeparamref name="TService"/> 类型配置的所有命名的实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <exception cref="ActivationException">解析类型时出错.</exception>
        /// <returns>实现服务接口的对象实例</returns>
        public static IEnumerable<TService> GetAllInstances<TService>()
        {
            return Container.Current.GetAllInstances<TService>();
        }

        /// <summary>
        /// 获取容器中为 <typeparamref name="TService"/> 类型配置的默认实例.
        /// 如果找不到则返回null.
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <returns>实现服务接口的对象实例</returns>
        public static TService TryGetInstance<TService>()
        {
            try
            {
                return Container.Current.GetInstance<TService>();
            }
            catch (ActivationException)
            {
            }
            return default(TService);
        }

        /// <summary>
        /// 按照名称获取容器中为 <typeparamref name="TService"/> 类型配置的实例.
        /// 如果找不到则返回null.
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <returns>实现服务接口的对象实例</returns>
        public static TService TryGetInstance<TService>(string name)
        {
            try
            {
                return Container.Current.GetInstance<TService>(name);
            }
            catch (ActivationException)
            {
            }
            return default(TService);
        }

    }
}
