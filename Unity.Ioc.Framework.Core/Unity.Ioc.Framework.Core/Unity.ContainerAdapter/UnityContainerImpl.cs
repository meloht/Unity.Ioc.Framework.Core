using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using Unity.Injection;
using Unity.Ioc.Framework.Core.Attributes;
using Unity.Ioc.Framework.Core.Configuration;
using Unity.Lifetime;

namespace Unity.Ioc.Framework.Core.Unity.ContainerAdapter
{
    /// <summary>
    /// 以代理方式使用UnityContainer实例实现IContainer接口.
    /// </summary>
    public class UnityContainerImpl : ContainerImplBase
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// 初始化实例, 根据配置加载类型映射
        /// </summary>
        public UnityContainerImpl(IUnityContainer container)
        {
            _container = container;
            SetupContainer();
        }

        #region ServiceLocatorImplBase

        private readonly List<string> _configuredModules = new List<string>();

        /// <summary>
        /// 根据系统模块(程序集)内的特定标注信息, 配置映射关系
        /// </summary>
        /// <param name="moduleName">程序集名称</param>
        public override void ConfigureWithModule(string moduleName)
        {
            try
            {
                if (_configuredModules.Contains(moduleName))
                    return;

                Assembly assembly = Assembly.Load(moduleName);
                if (assembly != null)
                    ConfigureByAttributesInAssembly(assembly);

                _configuredModules.Add(moduleName);
            }
            catch
            {
                Console.WriteLine(
                    "WARNING: Unable to load assembly " + moduleName + ".");
                Console.Error.WriteLine(
                    "WARNING: Unable to load assembly " + moduleName + ".");
            }
        }

        /// <summary>
        /// 子类覆写此方法时, 应当实现将 <paramref name="toType"/> 类型注册为
        /// <paramref name="fromType"/> 类型的实现类.
        /// </summary>
        /// <param name="fromType">接口类型</param>
        /// <param name="toType">实现类型</param>
        /// <param name="key">注册命名对象的名称, 可以为 null 以表示注册为默认实现类</param>
        /// <param name="lifetime">指定根据本条映射关系创建的对象的生命期</param>
        protected override void DoRegisterType(Type fromType, Type toType,
            string key, Lifetime lifetime)
        {
            LifetimeManager lm;
            switch (lifetime)
            {
                case Lifetime.New:
                    _container.RegisterType(fromType, toType, key);
                    break;
                case Lifetime.PerThread:
                    lm = new PerThreadLifetimeManager();
                    _container.RegisterType(fromType, toType, key, lm);
                    break;
                case Lifetime.Singleton:
                    lm = new ContainerControlledLifetimeManager();
                    _container.RegisterType(fromType, toType, key, lm);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("lifetime");
            }
        }

        /// <summary>
        /// 子类覆写此方法时, 应当实现将 <paramref name="instance"/> 注册为
        /// 服务接口类型 <paramref name="serviceType"/>的实现实例.
        /// </summary>
        /// <param name="serviceType">服务接口类型</param>
        /// <param name="key">注册命名对象的名称, 可以为 null 以表示注册为默认实现实例</param>
        /// <param name="instance">实现实例</param>
        protected override void DoRegisterInstance(Type serviceType,
            string key, object instance)
        {
            _container.RegisterInstance(serviceType, key, instance);
        }

        /// <summary>
        /// 子类覆写此方法时, 应当实现解析对指定的服务接口 <paramref name="serviceType"/>
        /// 的映射, 获取一个实例.
        /// </summary>
        /// <param name="serviceType">待解析的服务接口</param>
        /// <param name="key">指定希望获取的映射关系名称, 可以为 null 以获取默认映射的实例</param>
        /// <returns>通过解析获取到的对象实例</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return _container.Resolve(serviceType, key);
        }

        /// <summary>
        /// 子类覆写此方法时, 应当实现解析对指定的服务接口 <paramref name="serviceType"/>
        /// 的所有命名映射, 获取一组实例.
        /// </summary>
        /// <param name="serviceType">待解析的服务接口</param>
        /// <returns>通过解析获取到的对象实例</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }

        /// <summary>
        /// 子类覆写此方法时, 应当对已存在的对象 <paramref name="existing"/> 进行依赖注入.
        /// </summary>
        /// <param name="t">待注入依赖的对象类型</param>
        /// <param name="existing">待注入依赖的对象实例引用</param>
        /// <returns>已完成依赖注入的对象</returns>
        protected override object DoBuildUp(Type t, object existing)
        {
            return _container.BuildUp(t, existing);
        }

        #endregion

        #region Private routines

        /// <summary>
        /// 加载配置和标注, 初始化容器中的类型映射
        /// </summary>
        protected void SetupContainer()
        {
            // 加载配置文件中unity节指定的配置
            UnityConfigurationSection section = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;
            if (section != null)
            {
                //section.Containers.Default.Configure(_container);//Use the UnityConfigurationSection.Configure(container, name) method instead
                section.Configure(_container, section.Containers.Default.Name);
            }

            // 从入口程序集当中查找接口映射关系标注, 向容器注册
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            ConfigureByAttributesInAssembly(entryAssembly);

            // 加载配置文件中指定的模块程序集, 在其中查找接口映射关系标注, 向容器注册
            PackingSection packingSection = PackingSection.Content;
            if (packingSection != null)
            {
                IEnumerable<ModuleDefinition> modules =
                    PackingSection.Content.ModuleAssemblies;
                foreach (ModuleDefinition module in modules)
                {
                    string assemblyString = module.Name;
                    ConfigureWithModule(assemblyString);
                }
            }

            // 除了静态标注之外, 为模块提供另一个自定义容器配置的扩展机会.
            // 如果已加载的模块注册了 IContainerInitializer 接口的命名实现,
            // 它们将在此被调用.
            // 模块实现者可从接口的 InitContainer 方法参数中获取当前容器的引用,
            // 从而可以调用 IContainer API 对容器进行配置.
            IEnumerable<IContainerInitializer> initializers =
                _container.ResolveAll<IContainerInitializer>();
            foreach (IContainerInitializer initializer in initializers)
            {
                initializer.InitContainer(this);
            }
        }

        /// <summary>
        /// 加载 assembly 程序集, 向容器注册其中通过各种标注体现的容器配置信息
        /// </summary>
        /// <param name="assembly"></param>
        protected void ConfigureByAttributesInAssembly(Assembly assembly)
        {
            if (assembly == null) return;

            try
            {
                foreach (Type type in assembly.GetTypes())
                {

                    if (type.IsClass)
                    {
                        LoadServiceImplementation(type);
                        LoadInjectionCtors(type);
                        LoadInjectionMethods(type);
                        LoadDependencyAttributes(type);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(assembly.FullName + " Unable to load assembly");
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 检查 <paramref name="type"/> 上是否有 <see cref="ServiceImplementationAttribute"/>
        /// 标注, 有则向容器注册为相关接口的实现类
        /// </summary>
        /// <param name="type"></param>
        private void LoadServiceImplementation(Type type)
        {
            if (!type.IsDefined(typeof(ServiceImplementationAttribute), false)) return;

            ServiceImplementationAttribute attr =
                (ServiceImplementationAttribute)Attribute.GetCustomAttribute(
                    type, typeof(ServiceImplementationAttribute));

            ICollection<Type> interfaceTypes;
            if (attr.InterfaceTypes != null && attr.InterfaceTypes.Count > 0)
            {
                // 如果标注中限定了要注册的接口, 则只注册指定范围内的接口
                interfaceTypes = attr.InterfaceTypes;
            }
            else
            {
                // 没有限定, 则注册 type 类实现的所有接口
                interfaceTypes = type.GetInterfaces();
            }

            if (attr.Name == string.Empty)
                attr.Name = null;

            foreach (Type interfaceType in interfaceTypes)
            {
                if (interfaceType.IsGenericType
                    && !interfaceType.IsGenericTypeDefinition)
                {
                    Type genDefType = interfaceType.GetGenericTypeDefinition();
                    DoRegisterType(genDefType, type, attr.Name, attr.ResolveLifetime);
                }
                else
                {
                    DoRegisterType(interfaceType, type,
                                   attr.Name, attr.ResolveLifetime);
                }
            }
        }

        /// <summary>
        /// 检查 <paramref name="type"/> 的构造函数当中是否存在具有
        /// <see cref="InjectionConstructorAttribute"/> 标注的,
        /// 如果有, 则向容器注册指定的构造函数, 要求在创建时进行依赖注入.
        /// 如果没有, 
        /// </summary>
        /// <param name="type"></param>
        private void LoadInjectionCtors(Type type)
        {
            ConstructorInfo[] ctors = type.GetConstructors();
            ConstructorInfo found = null;
            int paramCount = -1;
            ConstructorInfo ctorWithMostParams = null;
            foreach (ConstructorInfo constructor in ctors)
            {
                int length = constructor.GetParameters().Length;
                if (length > paramCount)
                {
                    paramCount = length;
                    ctorWithMostParams = constructor;
                }
                if (constructor.IsDefined(typeof(InjectionConstructorAttribute), false))
                {
                    // 每个类找到一个带[InjectionConstructor]标注的构造函数就结束
                    found = constructor;
                    break;
                }
            }
            // 如果未找到标注[InjectionConstructor]的ctor, 则使用参数表最长的一个
            if (found == null && ctorWithMostParams != null)
            {
                found = ctorWithMostParams;
            }
            if (found != null)
            {
                List<object> paramList = BuildMethodParametersInfoList(found);
                _container.RegisterType(type,
                    new InjectionConstructor(paramList.ToArray()));
                return;
            }
        }

        /// <summary>
        /// 检查 <paramref name="type"/> 类中是否存在具有
        /// <see cref="InjectionMethodAttribute"/> 标注的方法,
        /// 如果有, 则向容器注册此方法作为初始化方法, 要求创建后调用且进行依赖注入.
        /// </summary>
        /// <param name="type"></param>
        private void LoadInjectionMethods(Type type)
        {
            foreach (MethodInfo method in type.GetMethods(
                BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.IsDefined(typeof(InjectionMethodAttribute), true))
                {
                    List<object> paramList = BuildMethodParametersInfoList(method);
                    if (paramList.Count > 0)
                        _container.RegisterType(type,
                            new InjectionMethod(method.Name, paramList.ToArray()));
                }
            }
        }

        /// <summary>
        /// InjectionConstructor 和 InjectionMethod 公用的参数列表分析过程
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private List<object> BuildMethodParametersInfoList(MethodBase method)
        {
            List<object> paramList = new List<object>();
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                if (parameter.IsDefined(typeof(DependencyAttribute), false))
                {
                    DependencyAttribute dep = (DependencyAttribute)
                        Attribute.GetCustomAttribute(
                        parameter, typeof(DependencyAttribute));
                    ResolvedParameter p =
                        new ResolvedParameter(parameter.ParameterType, dep.ResolveName);
                    paramList.Add(p);
                }
                else
                {
                    paramList.Add(parameter.ParameterType);
                }
            }
            return paramList;
        }

        /// <summary>
        /// 检查 <paramref name="type"/> 类公共可写属性上的
        /// <see cref="DependencyAttribute"/> 标注.
        /// 如果找到, 则向容器注册此属性要求依赖注入.
        /// </summary>
        /// <param name="type"></param>
        private void LoadDependencyAttributes(Type type)
        {
            foreach (PropertyInfo prop in type.GetProperties(
                BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance))
            {
                if (prop.GetIndexParameters().Length == 0 &&
                    prop.CanWrite &&
                    prop.IsDefined(typeof(DependencyAttribute), false))
                {
                    DependencyAttribute dep =
                        (DependencyAttribute)Attribute.GetCustomAttribute(
                        prop, typeof(DependencyAttribute), false);

                    _container.RegisterType(type,
                        new InjectionProperty(prop.Name,
                            new ResolvedParameter(prop.PropertyType, dep.ResolveName)));
                }
            }
        }

        #endregion
    }
}
