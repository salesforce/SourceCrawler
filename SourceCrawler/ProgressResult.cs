/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

namespace SourceCrawler
{
    public class ProgressResult
    {
        public int ProgressValue { get; set; }
        public string WorkingOn { get; set; }
        public bool? CloseForm { get; set; }
    }
}
