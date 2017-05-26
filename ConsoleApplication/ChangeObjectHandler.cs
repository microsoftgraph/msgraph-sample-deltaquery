//-------------------------------------------------------------------------------------------------
// <copyright file="ChangeObjectHandler.cs" company="Microsoft">
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
    using System.Collections.Generic;

    /// <summary>
    /// Defines methods to process the MS-Graph objects returned by Delta Query.
    /// </summary>
    public class ChangeObjectHandler : IChangeObjectHandler
    {
        /// <summary>
        /// Local object store.
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, object>> _objectStore =
            new Dictionary<string, Dictionary<string, object>>();

        /// <summary>
        /// Creates the specified object in the local store.
        /// </summary>
        /// <param name="change">Change representing a new object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        public void Create(Dictionary<string, object> change)
        {
            if (change == null)
            {
                throw new ArgumentNullException("change");
            }

            if (change.ContainsKey("id"))
            {
                string id = (string)change["id"];
                _objectStore.Add(id, change);
                Logger.DefaultLogger.Log(
                    "Object {0} added to local store",
                    id);
            }
            else
            {
                throw new ArgumentException("Invalid object", "change");
            }
        }

        /// <summary>
        /// Updates the specified object in the local store.
        /// </summary>
        /// <param name="change">Change representing an update to an existing object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        public void Update(Dictionary<string, object> change)
        {
            if (change == null)
            {
                throw new ArgumentNullException("change");
            }

            if (change.ContainsKey("id"))
            {
                string id = (string)change["id"];
                _objectStore[id] = change;
                Logger.DefaultLogger.Log(
                    "Object {0} updated in local store",
                    id);
            }
            else
            {
                throw new ArgumentException("Invalid object", "change");
            }
        }

        /// <summary>
        /// Determines whether the local store contains the specified MS-Graph object.
        /// </summary>
        /// <param name="change">Change representing a MS-Graph object.</param>
        /// <returns>
        /// <see langword="true"/> if the local store contains the specified MS-Graph object;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        public bool Exists(Dictionary<string, object> change)
        {
            if (change == null)
            {
                throw new ArgumentNullException("change");
            }

            if (change.ContainsKey("id"))
            {
                string id = (string)change["id"];
                return _objectStore.ContainsKey(id);
            }

            throw new ArgumentException("Invalid object", "change");
        }

        /// <summary>
        /// Determines whether the local store contains an object with the specified object ID.
        /// </summary>
        /// <param name="id">The object ID.</param>
        /// <returns>
        /// <see langword="true"/> if the local store contains an object with the specified object ID;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Exists(string id)
        {
            return _objectStore.ContainsKey(id);
        }

        /// <summary>
        /// Deletes the specified object from the local store.
        /// </summary>
        /// <param name="change">Change representing a deleted object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        public void Delete(Dictionary<string, object> change)
        {
            if (change == null)
            {
                throw new ArgumentNullException("change");
            }

            if (change.ContainsKey("id"))
            {
                string id = (string)change["id"];
                _objectStore.Remove(id);
                Logger.DefaultLogger.Log(
                    "Object {0} removed from local store",
                    id);
            }
            else
            {
                throw new ArgumentException("Invalid object", "change");
            }
        }
    }
}
