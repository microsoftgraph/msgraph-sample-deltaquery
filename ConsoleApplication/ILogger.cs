/// <copyright file="ILogger.cs" company="Microsoft">
/// Copyright (c) Microsoft Corporation. All rights reserved.
/// </copyright>
/// <Summary> 
/// Project: Differential Query Client Sample Application
/// Copyright (c) Microsoft Corporation.
/// 
/// This source is subject to the Sample Client End User License Agreement included in this project
/// Sample Client EULA.rtf
/// 
/// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
/// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
/// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
/// </summary>

namespace DifferentialQueryClient
{
    /// <summary>
    /// Defines methods to implement logging for differential query.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log a given string allowing formatting.
        /// </summary>
        /// <param name="format">Format string </param>
        /// <param name="objects"></param>
        void LogMessage(string format, params object[] objects);
    }
}
