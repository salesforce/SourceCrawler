/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SourceCrawler
{
    public class WhereClauseManager
    {
        public static string GetWhereClause(Collection<Column> cols)
        {
            var final = String.Join(" and ", cols.Select(cl => cl.GetClause()));
            return final;
        }
    }
}
