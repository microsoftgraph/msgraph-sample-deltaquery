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
            AppConfiguration appConfiguration = AppConfiguration.ReadFromJsonFile("appsettings.json");
            IChangeManager changeManager = new ChangeManager();

            var task = Task.Run(() => changeManager.DeltaQueryStartAsync(appConfiguration));

            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
