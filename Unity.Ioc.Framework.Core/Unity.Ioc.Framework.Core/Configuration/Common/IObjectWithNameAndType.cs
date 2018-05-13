using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core.Configuration.Common
{
    /// <summary>
    /// Represents the abstraction of an object with a name and a type.
    /// </summary>
    public interface IObjectWithNameAndType : IObjectWithName
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        Type Type { get; }
    }
}
