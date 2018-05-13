using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Unity.Ioc.Framework.Core.Attributes
{
    /// <summary>
    /// 提供根据Assembly, Type, MemberInfo等对象获取标注的方法.
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        /// 根据memberInfo查询其是否具有指定类型T的标注, 有的话返回标注对象, 
        /// 且对返回的对象调用<see cref="SmartAttribute.ReadMemberInfo"/>使其有机会了解标注目标的信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static T TryLoad<T>(MemberInfo memberInfo)
            where T : Attribute
        {
            T attr = default(T);
            if (Attribute.IsDefined(memberInfo, typeof(T)))
            {
                attr = Attribute.GetCustomAttribute(memberInfo, typeof(T)) as T;
                if (attr != null && attr is SmartAttribute)
                {
                    (attr as SmartAttribute).ReadMemberInfo(memberInfo);
                }
            }
            return attr;
        }

        /// <summary>
        /// 根据parameterInfo查询其是否具有指定类型T的标注, 有的话返回标注对象,
        /// 且对返回的对象调用<see cref="SmartAttribute.ReadParameterInfo"/>使其有机会了解标注目标的信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public static T TryLoad<T>(ParameterInfo parameterInfo)
            where T : Attribute
        {
            T attr = default(T);
            if (Attribute.IsDefined(parameterInfo, typeof(T)))
            {
                attr = Attribute.GetCustomAttribute(parameterInfo, typeof(T)) as T;

                if (attr != null && attr is SmartAttribute)
                {
                    (attr as SmartAttribute).ReadParameterInfo(parameterInfo);
                }
            }
            return attr;
        }

        /// <summary>
        /// 在<paramref name="assembly"/>程序集中查找所有被标注了
        /// <typeparamref name="T"/>的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IList<Type> FindTypesWith<T>(Assembly assembly)
            where T : Attribute
        {
            IList<Type> result = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                T attr = TryLoad<T>(type);
                if (attr != null && !attr.Equals(default(T)))
                {
                    result.Add(type);
                }
            }
            return result;
        }

    }
}
