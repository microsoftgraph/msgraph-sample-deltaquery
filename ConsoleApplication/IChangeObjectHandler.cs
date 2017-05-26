//-------------------------------------------------------------------------------------------------
// <copyright file="IChangeObjectHandler.cs" company="Microsoft">
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
