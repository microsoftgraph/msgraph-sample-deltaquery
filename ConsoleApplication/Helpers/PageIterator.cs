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
    using System.Threading.Tasks;
    using System;
    using Microsoft.Graph;

    /// <summary>
    /// Iterator for walking entities and pages of entities.
    /// </summary>
    /// <remarks>This class will be incorporated as a generic capability in the SDK as soon as
    /// we have a standard interface for collections.  Currently CollectionPage classes do not share
    /// any base class or interface so it is difficult to write generic class to support the paging behavior.
    /// </remarks>
    public class PageIterator
    {
        private IMailFolderDeltaCollectionPage mailfolders;
        private readonly Action<MailFolder> callback;
        public string DeltaLink { get; private set; }

        public PageIterator(IMailFolderDeltaCollectionPage mailFolders, Action<MailFolder> callback)
        {
            this.mailfolders = mailFolders;
            this.callback = callback;
        }
        public async Task Iterate()
        {
            var more = true;
            while (more)
            {
                foreach (var item in this.mailfolders)
                {
                    callback(item);
                }
                if (mailfolders.NextPageRequest != null)
                {
                    this.mailfolders = await mailfolders.NextPageRequest.GetAsync();
                } else
                {
                    more = false;
                }
                if (this.mailfolders.AdditionalData.ContainsKey(Constants.DeltaLinkFeedAnnotation))
                {
                    DeltaLink = this.mailfolders.AdditionalData[Constants.DeltaLinkFeedAnnotation] as string;
                }
            }
        }
    }
}
