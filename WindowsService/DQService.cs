//-------------------------------------------------------------------------------------------------
// <copyright file="DQService.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Delta Query sample Windows service.
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

namespace DeltaQueryWindowsService
{
    using System.ServiceProcess;
    using DeltaQueryApplication;

    /// <summary>
    /// Delta Query sample Windows service.
    /// </summary>
    public partial class DQService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DQService"/> class.
        /// </summary>
        public DQService()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            IChangeManager changeManager = new ChangeManager();
            changeManager.DeltaQuery();
        }

        /// <summary>
        /// Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
        }
    }
}
