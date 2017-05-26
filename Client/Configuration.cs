//-------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Graph Delta Query sample client.
// 
//     This source is subject to the Sample Client End User License Agreement
//     included in this project.
// </summary>
//
// <remarks />
//
// <disclaimer>
//     THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//     EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
//     WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </disclaimer>
//-------------------------------------------------------------------------------------------------

namespace DeltaQueryClient
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Defines methods to manage configuration for the sample application.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Assembly configuration
        /// </summary>
        private static readonly System.Configuration.Configuration config =
            ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Gets the value of the configuration element with the specified name.
        /// </summary>
        /// <param name="name">The name of the configuration element.</param>
        /// <returns>The value of the configuration element.</returns>
        public static string GetElementValue(string name)
        {
            KeyValueConfigurationElement element = config.AppSettings.Settings[name];

            if (element == null)
            {
                string errorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    "Missing app setting '{0}' in '{1}'.",
                    name,
                    config.FilePath);
                throw new ArgumentException(errorMessage, "name");
            }

            return element.Value;
        }
    }
}
