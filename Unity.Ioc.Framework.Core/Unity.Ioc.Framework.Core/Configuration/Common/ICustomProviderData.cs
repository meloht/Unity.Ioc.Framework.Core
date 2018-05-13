using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Unity.Ioc.Framework.Core.Configuration.Common
{
    /// <summary>
    /// Represents the configuration settings for a custom provider.
    /// </summary>
    public interface ICustomProviderData
    {
        /// <summary>
        /// Gets the name for the represented provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the attributes for the represented provider.
        /// </summary>
        NameValueCollection Attributes { get; }
    }
}
