//-------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Microsoft">
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
    /// Defines interface to provide logging for the sample application.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log a formatted string to a pre-defined output file.
        /// </summary>
        /// <param name="format">Composite format string.</param>
        /// <param name="args">Object array that contains zero or more objects to format.</param>
        void LogDebug(string format, params object[] args);

        /// <summary>
        /// Log a formatted string.
        /// </summary>
        /// <param name="format">Composite format string.</param>
        /// <param name="args">Object array that contains zero or more objects to format.</param>
        void Log(string format, params object[] args);
    }
}