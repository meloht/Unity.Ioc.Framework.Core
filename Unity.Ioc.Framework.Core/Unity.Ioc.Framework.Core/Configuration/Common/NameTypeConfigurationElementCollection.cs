﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

namespace Unity.Ioc.Framework.Core.Configuration.Common
{
    /// <summary>
    /// Represesnts a collection of <see cref="NameTypeConfigurationElement"/> objects.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="NameTypeConfigurationElement"/> object this collection contains.</typeparam>
    /// <typeparam name="TCustomElementData">The type used for Custom configuration elements in this collection.</typeparam>
    public class NameTypeConfigurationElementCollection<T, TCustomElementData> : PolymorphicConfigurationElementCollection<T>
        where T : NameTypeConfigurationElement, new()
        where TCustomElementData : T, new()
    {
        private const string typeAttribute = "type";

        /// <summary>
        /// Get the configuration object for each <see cref="NameTypeConfigurationElement"/> object in the collection.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> that is deserializing the element.</param>
        protected override Type RetrieveConfigurationElementType(XmlReader reader)
        {
            Type configurationElementType = null;
            if (reader.AttributeCount > 0)
            {
                // expect the first attribute to be the name
                for (bool go = reader.MoveToFirstAttribute(); go; go = reader.MoveToNextAttribute())
                {
                    if (typeAttribute.Equals(reader.Name))
                    {
                        Type providerType = Type.GetType(reader.Value, false);
                        if (providerType == null)
                        {
                            configurationElementType = typeof(TCustomElementData);
                            break;
                        }

                        Attribute attribute = Attribute.GetCustomAttribute(providerType, typeof(ConfigurationElementTypeAttribute));
                        if (attribute == null)
                        {
                            throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoConfigurationElementAttribute, providerType.Name));
                        }

                        configurationElementType = ((ConfigurationElementTypeAttribute)attribute).ConfigurationType;
                        break;
                    }
                }

                if (configurationElementType == null)
                {
                    throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionNoTypeAttribute, reader.Name));
                }

                // cover the traces ;)
                reader.MoveToElement();
            }
            return configurationElementType;
        }
    }
}
