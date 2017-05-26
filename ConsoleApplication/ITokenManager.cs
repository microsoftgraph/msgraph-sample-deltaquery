//-------------------------------------------------------------------------------------------------
// <copyright file="ITokenManager.cs" company="Microsoft">
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
    /// <summary>
    /// Defines methods to manage the sync token obtained from Delta Query.
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// Saves the token into a persistent store.
        /// </summary>
        /// <param name="token">Token to save.</param>
        void Save(string token);

        /// <summary>
        /// Reads the token from the persistent store.
        /// </summary>
        /// <returns>Token read from the persistent store or <see langword="null"/> if none exists.</returns>
        string Read();
    }
}
