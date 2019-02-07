//-------------------------------------------------------------------------------------------------
// <copyright file="ChangeManager.cs" company="Microsoft">
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
    using System.IO;
    using System.Threading;
    using DeltaQueryClient;
    using System.Collections;

    /// <summary>
    /// Defines methods to call the Delta Query service and process the results.
    /// </summary>
    public class ChangeManager : IChangeManager
    {
        /// <summary>
        /// Cookie manager.
        /// </summary>
        private static readonly TokenManager _tokenManager = new TokenManager();

        /// <summary>
        /// Object handler.
        /// </summary>
        private static readonly IChangeObjectHandler _changeObjectHandler = new ChangeObjectHandler();

        /// <summary>
        /// Output file for Delta Query result.
        /// </summary>
        private static StreamWriter _outputFile;

        /// <summary>
        /// Calls the Delta Query service and processes the result.
        /// </summary>
        public void DeltaQuery(AppConfiguration appConfiguration)
        {
            Logger.DefaultLogger.LogDebug(
                "Delta Query initialized with appPrincipalId {0}.",
                appConfiguration.AppPrincipalId);

            int pullIntervalSec = appConfiguration.PullIntervalSec;
            int retryAfterFailureIntervalSec = pullIntervalSec;
            string outputFilePath = Path.Combine(
                Environment.CurrentDirectory,
                "DeltaQueryResult" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");

            _outputFile = new StreamWriter(outputFilePath);

            Logger.DefaultLogger.Log("Delta Query sample application initialized");
            Logger.DefaultLogger.Log("Detected changes will be written to {0}.", outputFilePath);

            string stateToken = _tokenManager.Read();
            int retries = 0;

            Client client = new Client(appConfiguration.Scopes, appConfiguration.AppPrincipalId, Logger.DefaultLogger);

            while (true)
            {
                DeltaQueryResult result;

                try
                {
                    result = client.DeltaQuery(stateToken, appConfiguration.Scopes);
                }
                catch (Exception e)
                {
                    Logger.DefaultLogger.LogDebug(
                        "Delta Query request failed. Error: {0}, retrying after {1} sec",
                        e.Message,
                        retryAfterFailureIntervalSec);

                    Thread.Sleep(retryAfterFailureIntervalSec * 1000);

                    if (++retries < 5) // this should be adjusted specific to the application scenario
                    {
                        continue;
                    }
                    throw new Exception("Delta Query request failed. Error: {0}");
                }

                foreach (Dictionary<string, object> change in result.Changes)
                {
                    try
                    {
                        this.HandleChange(change);
                    }
                    catch (ArgumentException e)
                    {
                        Logger.DefaultLogger.Log("Invalid change: {0}", e.Message);
                    }
                }

                stateToken = result.StateToken;
                _tokenManager.Save(stateToken);
                retries = 0;
                if (!result.More)
                {
                    Logger.DefaultLogger.Log(
                        "Processed change(s) successfully. Will check back in {0} sec.",
                        pullIntervalSec);

                    Thread.Sleep(pullIntervalSec * 1000);
                }
            }
        }

        /// <summary>
        /// Processes the specified object represented by the change.
        /// </summary>
        /// <param name="change">Change representing an object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change"/> is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Invalid change.</exception>
        public void HandleChange(Dictionary<string, object> change)
        {
            if (change == null)
            {
                throw new ArgumentNullException("change");
            }

            if (!change.ContainsKey("id"))
            {
                throw new ArgumentException("Invalid object", "change");
            }
            string changeId = change["id"].ToString();
            bool isDeleted = change.ContainsKey("@removed");
            if (isDeleted)
            {
                IDictionary idict = (IDictionary)change["@removed"];
                bool isSoftDeleted = idict["reason"].Equals("changed");
                _changeObjectHandler.Delete(change);
            }
            else if (_changeObjectHandler.Exists(change))
            {
                _changeObjectHandler.Update(change);
            }
            else
            {
                _changeObjectHandler.Create(change);
            }
        
         }
    }
}
