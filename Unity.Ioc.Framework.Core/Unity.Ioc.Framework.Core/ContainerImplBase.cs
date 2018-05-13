using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Unity.Ioc.Framework.Core.Configuration.Common;

namespace Unity.Ioc.Framework.Core
{
    /// <summary>
    /// 实现了 <see cref="IContainer"/> 接口的内容, 减少子类需要实现的方法数量.
    /// </summary>
    public abstract class ContainerImplBase : IContainer
    {
        #region 实现 IContainer 接口

        /// <summary>
        /// 根据系统模块(程序集)内的特定标注信息, 配置映射关系
        /// </summary>
        /// <param name="moduleName">程序集名称</param>
        public abstract void ConfigureWithModule(string moduleName);

        #region 注册类型

        /// <summary>
        /// 注册从 <typeparamref name="TFrom"/> 到 <typeparamref name="TTo"/> 的映射关系
        /// </summary>
        /// <typeparam name="TFrom">指定待注册的服务接口类型</typeparam>
        /// <typeparam name="TTo">指定待注册的实现类类型</typeparam>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        public virtual IContainer RegisterType<TFrom, TTo>()
        {
            DoRegisterType(typeof(TFrom), typeof(TTo), null, Lifetime.New);
            return this;
        }

        /// <summary>
        /// 注册从 <typeparamref name="TFrom"/> 到 <typeparamref name="TTo"/> 的映射关系
        /// </summary>
        /// <typeparam name="TFrom">指定待注册的服务接口类型</typeparam>
        /// <typeparam name="TTo">指定待注册的实现类类型</typeparam>
        /// <param name="lifetime">指定对象生存期</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        public virtual IContainer RegisterType<TFrom, TTo>(Lifetime lifetime)
        {
            DoRegisterType(typeof(TFrom), typeof(TTo), null, lifetime);
            return this;
        }

        /// <summary>
        /// 注册从 <typeparamref name="TFrom"/> 到 <typeparamref name="TTo"/>,
        /// 命名为 <paramref name="name"/> 的映射关系
        /// </summary>
        /// <typeparam name="TFrom">指定待注册的服务接口类型</typeparam>
        /// <typeparam name="TTo">指定待注册的实现类类型</typeparam>
        /// <param name="name"></param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        public virtual IContainer RegisterType<TFrom, TTo>(string name)
        {
            DoRegisterType(typeof(TFrom), typeof(TTo), name, Lifetime.New);
            return this;
        }

        /// <summary>
        /// 注册从 <typeparamref name="TFrom"/> 到 <typeparamref name="TTo"/>,
        /// 命名为 <paramref name="name"/> 的映射关系
        /// </summary>
        /// <typeparam name="TFrom">指定待注册的服务接口类型</typeparam>
        /// <typeparam name="TTo">指定待注册的实现类类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <param name="lifetime">指定对象生存期</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        public virtual IContainer RegisterType<TFrom, TTo>(string name, Lifetime lifetime)
        {
            DoRegisterType(typeof(TFrom), typeof(TTo), name, lifetime);
            return this;
        }

        #endregion

        #region 注册实例

        /// <summary>
        /// 注册 <typeparamref name="TInterface"/> 类型的对象实例
        /// </summary>
        /// <typeparam name="TInterface">指定待注册的服务接口类型</typeparam>
        /// <param name="instance">指定待注册的实现对象引用</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        public IContainer RegisterInstance<TInterface>(TInterface instance)
        {
            DoRegisterInstance(typeof(TInterface), null, instance);
            return this;
        }

        /// <summary>
        /// 注册 <typeparamref name="TInterface"/> 类型的对象实例, 
        /// 命名为<paramref name="name"/>
        /// </summary>
        /// <typeparam name="TInterface">指定待注册的服务接口类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <param name="instance">指定待注册的实现对象引用</param>
        /// <returns>返回当前IContainer自身引用, 以便继续调用其他方法</returns>
        public IContainer RegisterInstance<TInterface>(string name, TInterface instance)
        {
            DoRegisterInstance(typeof(TInterface), name, instance);
            return this;
        }

        #endregion

        #region 获取指定类型的对象实例

        /// <summary>
        /// 获取为指定的类型配置的默认实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <returns>实现服务接口的对象实例</returns>
        public virtual TService GetInstance<TService>()
        {
            return (TService)GetInstance(typeof(TService), null);
        }

        /// <summary>
        /// 按照名称获取为指定的类型配置的实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <param name="name">映射名称</param>
        /// <returns>实现服务接口的对象实例</returns>
        public virtual TService GetInstance<TService>(string name)
        {
            return (TService)GetInstance(typeof(TService), name);
        }

        /// <summary>
        /// 获取指定的 <paramref name="serviceType"/> 类型的命名实例.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <param name="key">Name the object was registered with.</param>
        /// <exception cref="ActivationException">if there is an error resolving
        /// the service instance.</exception>
        /// <returns>The requested service instance.</returns>
        protected virtual object GetInstance(Type serviceType, string key)
        {
            try
            {
                return DoGetInstance(serviceType, key);
            }
            catch (Exception ex)
            {
                throw new ActivationException(
                    FormatActivationExceptionMessage(ex, serviceType, key),
                    ex);
            }
        }

        /// <summary>
        /// 获取容器中为 <typeparamref name="TService"/> 类型配置的所有命名的实例
        /// </summary>
        /// <typeparam name="TService">指定待解析的服务接口类型</typeparam>
        /// <exception cref="ActivationException">解析类型时出错.</exception>
        /// <returns>实现服务接口的对象实例</returns>
        public virtual IEnumerable<TService> GetAllInstances<TService>()
        {
            foreach (object item in GetAllInstances(typeof(TService)))
            {
                yield return (TService)item;
            }
        }

        /// <summary>
        /// 获取指定的 <paramref name="serviceType"/> 类型目前已注册的所有命名实例.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <exception cref="ActivationException">if there is are errors resolving
        /// the service instance.</exception>
        /// <returns>A sequence of instances of the requested <paramref name="serviceType"/>.</returns>
        public virtual IEnumerable<object> GetAllInstances(Type serviceType)
        {
            try
            {
                return DoGetAllInstances(serviceType);
            }
            catch (Exception ex)
            {
                throw new ActivationException(
                    FormatActivateAllExceptionMessage(ex, serviceType),
                    ex);
            }
        }

        #endregion

        #region 对已存在的对象进行依赖注入

        /// <summary>
        /// 对其他方式创建的 <paramref name="existing"/> 对象进行依赖注入
        /// </summary>
        /// <typeparam name="T">待注入依赖的对象类型</typeparam>
        /// <param name="existing">待注入依赖的对象实例引用</param>
        /// <returns>注入依赖之后的对象</returns>
        public virtual T BuildUp<T>(T existing)
        {
            return (T)DoBuildUp(typeof(T), existing);
        }

        #endregion

        #endregion

        #region 供子类覆写的抽象方法

        #region 注册类型

        /// <summary>
        /// 子类覆写此方法时, 应当实现将 <paramref name="toType"/> 类型注册为
        /// 服务接口类型 <paramref name="fromType"/> 的实现类.
        /// </summary>
        /// <param name="fromType">接口类型</param>
        /// <param name="toType">实现类型</param>
        /// <param name="key">注册命名对象的名称, 可以为 null 以表示注册为默认实现类</param>
        /// <param name="lifetime">指定根据本条映射关系创建的对象的生命期</param>
        protected abstract void DoRegisterType(Type fromType, Type toType,
            string key, Lifetime lifetime);

        #endregion

        #region 注册实例

        /// <summary>
        /// 子类覆写此方法时, 应当实现将 <paramref name="instance"/> 注册为
        /// 服务接口类型 <paramref name="serviceType"/>的实现实例.
        /// </summary>
        /// <param name="serviceType">服务接口类型</param>
        /// <param name="key">注册命名对象的名称, 可以为 null 以表示注册为默认实现实例</param>
        /// <param name="instance">实现实例</param>
        protected abstract void DoRegisterInstance(Type serviceType,
            string key, object instance);

        #endregion

        #region 获取指定类型的对象实例

        /// <summary>
        /// 子类覆写此方法时, 应当实现解析对指定的服务接口 <paramref name="serviceType"/>
        /// 的映射, 获取一个实例.
        /// </summary>
        /// <param name="serviceType">待解析的服务接口</param>
        /// <param name="key">指定希望获取的映射关系名称, 可以为 null 以获取默认映射的实例</param>
        /// <returns>通过解析获取到的对象实例</returns>
        protected abstract object DoGetInstance(Type serviceType, string key);

        /// <summary>
        /// 子类覆写此方法时, 应当实现解析对指定的服务接口 <paramref name="serviceType"/>
        /// 的所有命名映射, 获取一组实例.
        /// </summary>
        /// <param name="serviceType">待解析的服务接口</param>
        /// <returns>通过解析获取到的对象实例</returns>
        protected abstract IEnumerable<object> DoGetAllInstances(Type serviceType);

        #endregion

        #region 对已存在的对象进行依赖注入

        /// <summary>
        /// 子类覆写此方法时, 应当对已存在的对象 <paramref name="existing"/> 进行依赖注入.
        /// </summary>
        /// <param name="t">待注入依赖的对象类型</param>
        /// <param name="existing">待注入依赖的对象实例引用</param>
        /// <returns>已完成依赖注入的对象</returns>
        protected abstract object DoBuildUp(Type t, object existing);

        #endregion

        /// <summary>
        /// Format the exception message for use in an <see cref="ActivationException"/>
        /// that occurs while resolving a single service.
        /// </summary>
        /// <param name="actualException">The actual exception thrown by the implementation.</param>
        /// <param name="serviceType">Type of service requested.</param>
        /// <param name="key">Name requested.</param>
        /// <returns>The formatted exception message string.</returns>
        protected virtual string FormatActivationExceptionMessage(Exception actualException, Type serviceType, string key)
        {
            return string.Format(CultureInfo.CurrentUICulture, Resources.ActivationExceptionMessage, serviceType.Name, key);
        }

        /// <summary>
        /// Format the exception message for use in an <see cref="ActivationException"/>
        /// that occurs while resolving multiple service instances.
        /// </summary>
        /// <param name="actualException">The actual exception thrown by the implementation.</param>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>The formatted exception message string.</returns>
        protected virtual string FormatActivateAllExceptionMessage(Exception actualException, Type serviceType)
        {
            return string.Format(CultureInfo.CurrentUICulture, Resources.ActivateAllExceptionMessage, serviceType.Name);
        }

        #endregion
    }
}
