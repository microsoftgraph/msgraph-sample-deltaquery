//-------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
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

namespace DeltaQueryApplication
{
    /// <summary>
    /// This class contains the constant fields used in this sample.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Authorization header.
        /// </summary>
        public const string HeaderNameAuthorization = "Authorization";

        /// <summary>
        /// deltaLink query parameter.
        /// </summary>
        public const string DeltaLinkQueryParameter = "deltatoken";
        
        /// <summary>
        /// nextLink query parameter.
        /// </summary>
        public const string NextLinkQueryParameter = "skiptoken";
        
        /// <summary>
        /// Feed annotation that represents a URI to be called immediately for more changes.
        /// </summary>        
        public const string DeltaLinkFeedAnnotation = "@odata.deltaLink";

        /// <summary>
        /// Feed annotation that represents a URI to be called later after the polling interval has passed.
        /// </summary>
        public const string NextLinkFeedAnnotation = "@odata.nextLink";

        /// <summary>
        /// Format for getting STS endpoint.
        /// </summary>
        public const string AuthEndpoint = "https://login.windows.net/{0}";
    }
}
