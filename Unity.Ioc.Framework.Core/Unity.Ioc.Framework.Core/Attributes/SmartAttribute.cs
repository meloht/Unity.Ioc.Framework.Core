using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Unity.Ioc.Framework.Core.Attributes
{
    /// <summary>
    /// 扩充标注类, 继承此类的标注可以了解所标注的对象的信息.
    /// </summary>
    public abstract class SmartAttribute : Attribute
    {
        /// <summary>
        /// 将标注目标的信息传给SmartAttribute对象实例
        /// </summary>
        /// <param name="memberInfo"></param>
        protected internal virtual void ReadMemberInfo(MemberInfo memberInfo)
        {
        }

        /// <summary>
        /// 将标注目标的信息传给SmartAttribute对象实例
        /// </summary>
        /// <param name="parameterInfo"></param>
        protected internal virtual void ReadParameterInfo(ParameterInfo parameterInfo)
        {
        }
    }
}
