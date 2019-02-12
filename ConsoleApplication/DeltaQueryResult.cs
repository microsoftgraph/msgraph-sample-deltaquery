//-------------------------------------------------------------------------------------------------
// <copyright file="DeltaQueryResult.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Response from Delta Query service.
    /// </summary>
    public class DeltaQueryResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaQueryResult"/> class.
        /// </summary>
        /// <param name="response">Response from Delta Query service.</param>
        /// <exception cref="ArgumentNullException"><paramref name="response"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid response.</exception>
        public DeltaQueryResult(Dictionary<string, object> response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            if (response.ContainsKey(Constants.DeltaLinkFeedAnnotation))
            {
                this.StateToken = response[Constants.DeltaLinkFeedAnnotation].ToString();
                this.More = false;
            }
            else
            {
                // missing nextLink/deltaLink
                throw new ArgumentException("missing nextLink/deltaLink", "response");
            }

            if (response.ContainsKey("value"))
            {
                this.Changes = Array.ConvertAll((object[])response["value"], o => (Dictionary<string, object>)o);
            }
            else
            {
                // missing changes
                throw new ArgumentException("missing changes", "response");
            }
        }

        /// <summary>
        /// Gets the collection of changes.
        /// </summary>
        public ICollection<Dictionary<string, object>> Changes { get; private set; }

        /// <summary>
        /// Gets the next synchronization token.
        /// </summary>
        public string StateToken { get; private set; }

        /// <summary>
        /// Gets a value indicating whether more changes are available for synchronization.
        /// </summary>
        public bool More { get; private set; }
    }
}
