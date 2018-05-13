using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Unity.Ioc.Framework.Core.Configuration.Common
{
    /// <summary>
    /// Represents a <see cref="ConfigurationElement"/> that has a name and type.
    /// </summary>
    public class NameTypeConfigurationElement : NamedConfigurationElement, IObjectWithNameAndType
    {
        private readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Name of the property that holdNameTypeConfigurationElementeTypeConfigurationElement"/>.
        /// </summary>
        public const string typeProperty = "type";

        /// <summary>
        /// Intialzie an instance of the <see cref="NameTypeConfigurationElement"/> class.
        /// </summary>
        public NameTypeConfigurationElement()
        {
        }

        /// <summary>
        /// Initialize an instance of the <see cref="NameTypeConfigurationElement"/> class
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <param name="type">The <see cref="Type"/> that this element is the configuration for.</param>
        public NameTypeConfigurationElement(string name, Type type)
            : base(name)
        {
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> the element is the configuration for.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> the element is the configuration for.
        /// </value>
        public Type Type
        {
            get { return (Type)typeConverter.ConvertFrom(TypeName); }
            set { TypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the <see cref="Type"/> the element is the configuration for.
        /// </summary>
        /// <value>
        /// the fully qualified name of the <see cref="Type"/> the element is the configuration for.
        /// </value>
        [ConfigurationProperty(typeProperty, IsRequired = true)]
        public string TypeName
        {
            get { return (string)this[typeProperty]; }
            set { this[typeProperty] = value; }
        }

        internal new virtual ConfigurationPropertyCollection Properties
        {
            get { return base.Properties; }
        }
    }
}
