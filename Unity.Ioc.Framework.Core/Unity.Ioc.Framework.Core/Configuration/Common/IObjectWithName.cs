using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core.Configuration.Common
{
    /// <summary>
    /// Represents the abstraction of an object with a name.
    /// </summary>
    public interface IObjectWithName
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }
    }
}
