//-------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Delta Query sample application.
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

namespace DeltaQueryApplication
{
    using System.Configuration;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Delta Query sample console application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point 
        /// </summary>
        private static void Main()
        {
            AppConfiguration appConfiguration = AppConfiguration.GetConfiguration();
            IChangeManager changeManager = new ChangeManager();

            var task = Task.Run(() => changeManager.DeltaQueryAsync(appConfiguration));

            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            changeManager.DeltaQueryAsync(appConfiguration);
        }
    }


    /// <summary>
    /// Class to hold the app configurations read from the file
    /// </summary>
    public class AppConfiguration
    {
        public static AppConfiguration GetConfiguration()
        {
            var appConfig = new AppConfiguration()
            {
                AppPrincipalId = ConfigurationManager.AppSettings["AppPrincipalId"],
                PullIntervalSec = int.Parse(ConfigurationManager.AppSettings["PullIntervalSec"]),
                Scopes = ConfigurationManager.AppSettings["Scopes"].Split(','),
                AppVersion = ConfigurationManager.AppSettings["AppVersion"],
            };

            return appConfig;
        }
        
        public string AppPrincipalId { get; set; }
        public string AppVersion { get; set; }
        public int PullIntervalSec { get; set; }
        public string [] Scopes { get; set; }
    }
}
