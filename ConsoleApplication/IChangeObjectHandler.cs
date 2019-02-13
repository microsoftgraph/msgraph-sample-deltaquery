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
    using System.Collections.Generic;

    /// <summary>
    /// Defines methods to process the MS-Graph objects returned by Delta Query.
    /// </summary>
    public interface IChangeObjectHandler
    {
        /// <summary>
        /// Creates the specified object in the local store.
        /// </summary>
        /// <param name="change">Change representing a new object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        void Create(Dictionary<string, object> change);

        /// <summary>
        /// Updates the specified object in the local store.
        /// </summary>
        /// <param name="change">Change representing an update to an existing object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        void Update(Dictionary<string, object> change);

        /// <summary>
        /// Determines whether the local store contains the specified object.
        /// </summary>
        /// <param name="change">Change representing the object to be searched</param>
        /// <returns>
        /// <see langword="true"/> if the local store contains the specified object;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        bool Exists(Dictionary<string, object> change);

        /// <summary>
        /// Determines whether the local store contains an object with the specified object ID.
        /// </summary>
        /// <param name="id">The ID of the object.</param>
        /// <returns>
        /// <see langword="true"/> if the local store contains an object with the specified object ID;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        bool Exists(string id);

        /// <summary>
        /// Deletes the specified object from the local store.
        /// </summary>
        /// <param name="change">Change representing a deleted object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        void Delete(Dictionary<string, object> change);
    }
}
