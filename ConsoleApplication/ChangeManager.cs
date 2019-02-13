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
    using System.IO;
    using System.Threading.Tasks;

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
        public async Task DeltaQueryStartAsync(AppConfiguration appConfiguration)
        {
            Logger.DefaultLogger.LogDebug(
                "Delta Query initialized with ClientId {0}.",
                appConfiguration.ClientId);

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

            DeltaQuery deltaQuery = new DeltaQuery(appConfiguration,Logger.DefaultLogger);

            while (true)
            {
                DeltaQueryResult result;

                try
                {
                    result = await deltaQuery.DeltaQueryRunAsync(stateToken);
                }
                catch (Exception e)
                {
                    Logger.DefaultLogger.LogDebug(
                        "Delta Query request failed. Error: {0}, retrying after {1} sec",
                        e.Message,
                        retryAfterFailureIntervalSec);

                    await Task.Delay(retryAfterFailureIntervalSec * 1000);

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

                    await Task.Delay(pullIntervalSec * 1000);
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
