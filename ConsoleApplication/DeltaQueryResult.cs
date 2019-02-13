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
