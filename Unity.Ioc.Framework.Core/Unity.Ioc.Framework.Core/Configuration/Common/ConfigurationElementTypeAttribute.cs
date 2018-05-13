﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Ioc.Framework.Core.Configuration.Common
{
    /// <summary>
    /// Indicates the configuration object type that is used for the attributed object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ConfigurationElementTypeAttribute : Attribute
    {
        private Type configurationType;

        /// <summary>
        /// Initialize a new instance of the <see cref="EnterpriseLibrary.Common.Configuration.ConfigurationElementTypeAttribute"/> class.
        /// </summary>
        public ConfigurationElementTypeAttribute()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationElementTypeAttribute"/> class with the configuration object type.
        /// </summary>
        /// <param name="configurationType">The <see cref="Type"/> of the configuration object.</param>
        public ConfigurationElementTypeAttribute(Type configurationType)
        {
            this.configurationType = configurationType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the configuration object.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the configuration object.
        /// </value>
        public Type ConfigurationType
        {
            get { return configurationType; }
        }
    }
}
