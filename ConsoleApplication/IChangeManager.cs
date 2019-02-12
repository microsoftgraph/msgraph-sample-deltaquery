//-------------------------------------------------------------------------------------------------
// <copyright file="IChangeManager.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines methods to call the Delta Query service and process the results.
    /// </summary>
    public interface IChangeManager
    {
        /// <summary>
        /// Calls the Delta Query Graph service and processes the result.
        /// </summary>
        Task DeltaQueryStartAsync(AppConfiguration appConfiguration);

        /// <summary>
        /// Processes the specified object represented by the change.
        /// </summary>
        /// <param name="change">Change representing a MS-Graph object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        void HandleChange(Dictionary<string, object> change);
    }
}
