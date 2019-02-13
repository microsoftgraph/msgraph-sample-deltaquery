/*
 The MIT License (MIT)

Copyright (c) 2019 Microsoft Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

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