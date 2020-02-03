/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

using System;
using System.ComponentModel;
using System.IO;

namespace SourceCrawler
{
    public class RootObject : INotifyPropertyChanged
    {
        public string RootId { get; set; }
        public string RootPath { get; set; }
        public bool IsDefault { get; set; }
        public DateTime LastUpdate { get; set; }

        public string FullRootRepoFile
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.PROGRAM_NAME, String.Format(Constants.REPO_FILE_FORMAT, RootId));
            }
        }

        public void Refresh(string rootId)
        {
            var SQL = "select * from roots where root_id={0}";
            var ds = RepositoryUtils.ExecuteQuery(String.Format(SQL, rootId.SafeStringToSQL()));

            if (ds.Tables[0].Rows.Count == 1)
            {
                var row = ds.Tables[0].Rows[0];

                RootId = rootId;
                RootPath = row["root_path"].ToString();
                IsDefault = Convert.ToInt32(row["is_default"]) == 1 ? true : false;
                LastUpdate = Convert.ToDateTime(row["last_update"]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
