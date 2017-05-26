//-------------------------------------------------------------------------------------------------
// <copyright file="TokenManager.cs" company="Microsoft">
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
    public class TokenManager : ITokenManager
    {
        /// <summary>
        /// Saves the token into a persistent store.
        /// </summary>
        /// <param name="token">Token to save.</param>
        public void Save(string token)
        {
            // Implement a way to store the token in a file or any other persistent store.
        }

        /// <summary>
        /// Reads the token from the persistent store.
        /// </summary>
        /// <returns>Token read from the persistent store or <see langword="null"/> if none exists.</returns>
        public string Read()
        {
            // Implement a way to retrieve the token from a file or any other persistent store.
            return null;
        }
    }
}
