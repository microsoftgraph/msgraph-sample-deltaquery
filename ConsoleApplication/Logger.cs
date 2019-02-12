//-------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Microsoft">
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
    using System;
    using System.IO;

    /// <summary>
    /// Defines methods to provide logging for the sample application.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Default logger.
        /// </summary>
        public static Logger DefaultLogger = new Logger();

        /// <summary>
        /// Name of the file used to log information used to debug Delta query issues.
        /// </summary>
        private const string DebugFileName = "DeltaQuery_Debug.txt";

        /// <summary>
        /// Path of the debug file.
        /// </summary>
        private readonly string debugFilePath = Path.Combine(Environment.CurrentDirectory, DebugFileName);

        /// <summary>
        /// Prevents a default instance of the <see cref="Logger"/> class from being created.
        /// </summary>
        private Logger()
        {
            FileInfo fileInfo = new FileInfo(this.debugFilePath);
            if (fileInfo.CreationTime < DateTime.Now.Subtract(TimeSpan.FromDays(2)))
            {
                fileInfo.Delete();
            }
        }

        /// <summary>
        /// Log a formatted string to a pre-defined output file.
        /// </summary>
        /// <param name="format">Composite format string.</param>
        /// <param name="args">Object array that contains zero or more objects to format.</param>
        public void LogDebug(string format, params object[] args)
        {
            StreamWriter debugFile = new StreamWriter(this.debugFilePath, true);
            debugFile.Write(DateTime.UtcNow.ToString() + " ");
            debugFile.WriteLine(format, args);
            debugFile.Close();
        }

        /// <summary>
        /// Log a formatted string.
        /// </summary>
        /// <param name="format">Composite format string.</param>
        /// <param name="args">Object array that contains zero or more objects to format.</param>
        public void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
