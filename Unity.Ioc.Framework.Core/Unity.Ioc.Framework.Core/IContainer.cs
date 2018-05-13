using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core
{
    /// <summary>
    /// 定义依赖注入容器的对外接口.
    /// </summary>
    public interface IContainer
    {
        #region 

        /// <summary>
        /// 根据系统模块(程序集)内的特定标注信息, 配置映射关系
        /// </summary>
        /// <param name="moduleName">程序集名称</param>
        void ConfigureWithModule(string moduleName);

        #endregion

        #region 注册类型

        /// <summary>
        /// 注册从 <typeparamref name="TFrom"/> 到 <typeparamref name="TTo"/> 的映射关系
        /// </summary>
        /// <typeparam name="TFrom">指定待注册的服务接口类型</typeparam>
        /// <typeparam name="TTo">指定待注册的实现类类型</typeparam>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        IContainer RegisterType<TFrom, TTo>();

        /// <summary>
        /// 注册从 <typeparamref name="TFrom"/> 到 <typeparamref name="TTo"/> 的映射关系
        /// </summary>
        /// <typeparam name="TFrom">指定待注册的服务接口类型</typeparam>
        /// <typeparam name="TTo">指定待注册的实现类类型</typeparam>
        /// <param name="lifetime">指定对象生存期</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        IContainer RegisterType<TFrom, TTo>(Lifetime lifetime);

        /// <summary>
        /// 注册从 <typeparamref name="TFrom"/> 到 <typeparamref name="TTo"/>,
        /// 命名为 <paramref name="name"/> 的映射关系
        /// </summary>
        /// <typeparam name="TFrom">指定待注册的服务接口类型</typeparam>
        /// <typeparam name="TTo">指定待注册的实现类类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        IContainer RegisterType<TFrom, TTo>(string name);

        /// <summary>
        /// 注册从 <typeparamref name="TFrom"/> 到 <typeparamref name="TTo"/>,
        /// 命名为 <paramref name="name"/> 的映射关系
        /// </summary>
        /// <typeparam name="TFrom">指定待注册的服务接口类型</typeparam>
        /// <typeparam name="TTo">指定待注册的实现类类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <param name="lifetime">指定对象生存期</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        IContainer RegisterType<TFrom, TTo>(string name, Lifetime lifetime);

        #endregion

        #region 注册实例

        /// <summary>
        /// 注册 <typeparamref name="TInterface"/> 类型的对象实例
        /// </summary>
        /// <typeparam name="TInterface">指定待注册的服务接口类型</typeparam>
        /// <param name="instance">指定待注册的实现对象引用</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        IContainer RegisterInstance<TInterface>(TInterface instance);

        /// <summary>
        /// 注册 <typeparamref name="TInterface"/> 类型的对象实例, 
        /// 命名为<paramref name="name"/>
        /// </summary>
        /// <typeparam name="TInterface">指定待注册的服务接口类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <param name="instance">指定待注册的实现对象引用</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        IContainer RegisterInstance<TInterface>(string name, TInterface instance);

        #endregion

        #region 获取指定类型的对象实例

        /// <summary>
        /// 获取为指定的类型配置的默认实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <returns>实现服务接口的对象实例</returns>
        TService GetInstance<TService>();

        /// <summary>
        /// 按照名称获取为指定的类型配置的实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <returns>实现服务接口的对象实例</returns>
        TService GetInstance<TService>(string name);

        /// <summary>
        /// 获取容器中为 <typeparamref name="TService"/> 类型配置的所有命名的实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <exception cref="ActivationException">解析类型时出错.</exception>
        /// <returns>实现服务接口的对象实例</returns>
        IEnumerable<TService> GetAllInstances<TService>();

        #endregion

        #region 对已存在对象进行配置

        /// <summary>
        /// 对其他方式创建的 <paramref name="existing"/> 对象进行依赖注入
        /// </summary>
        /// <typeparam name="T">待注入依赖的对象类型</typeparam>
        /// <param name="existing">待注入依赖的对象实例引用</param>
        /// <returns>已完成依赖注入的对象</returns>
        T BuildUp<T>(T existing);

        #endregion

    }
}
