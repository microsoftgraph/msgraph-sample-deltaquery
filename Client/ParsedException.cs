//-------------------------------------------------------------------------------------------------
// <copyright file="ParsedException.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Delta Query sample client.
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

namespace DeltaQueryClient
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Exception type which represents the <see cref="DataServiceException"/> thrown by the ADO.NET Data Service.
    /// </summary>
    [Serializable]
    public class ParsedException : XmlErrorResponse
    {
        /// <summary>
        /// XML serializer
        /// </summary>
        private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlErrorResponse));

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedException"/> class.
        /// </summary>
        public ParsedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedException"/> class.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">Error message.</param>
        public ParsedException(string code, string message)
        {
            this.Code = code;
            this.Message = new ErrorMessage() { Value = message };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedException"/> class.
        /// </summary>
        /// <param name="error">Error object.</param>
        public ParsedException(XmlErrorResponse error)
        {
            this.Code = error.Code;
            this.InnerError = error.InnerError;
            this.Message = error.Message;
            this.Values = error.Values;
        }

        /// <summary>
        /// Parses the specified exception.
        /// </summary>
        /// <param name="ex">Exception to parse.</param>
        /// <returns>Parsed exception.</returns>
        /// <exception cref="ArgumentNullException">Exception is null.</exception>
        /// <exception cref="ArgumentException">Exception is not of type InvalidOperationException.</exception>
        public static ParsedException Parse(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            if (!(ex is InvalidOperationException))
            {
                throw new ArgumentException("Exception is not of type InvalidOperationException", "ex");
            }

            ParsedException parsedException = new ParsedException();

            string messageToBeParsed = ex.Message;
            if (ex.InnerException != null)
            {
                messageToBeParsed = ex.InnerException.Message;
            }

            try
            {
                StringReader stringReader = new StringReader(messageToBeParsed);
                XmlErrorResponse errorObject =
                    ParsedException.xmlSerializer.Deserialize(stringReader) as XmlErrorResponse;
                parsedException = new ParsedException(errorObject);
            }
            catch (InvalidOperationException)
            {
            }

            if (String.IsNullOrEmpty(parsedException.Code))
            {
                parsedException.Code = messageToBeParsed;
            }

            return parsedException;
        }
    }
}
