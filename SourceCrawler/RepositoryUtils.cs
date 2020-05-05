/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Xml;

namespace SourceCrawler
{
    class RepositoryUtils
    {
        #region Table DDL

        private const string CREATE_CONFIG_DB = @"CREATE TABLE [options] (
                [option_key] VARCHAR(200) NOT NULL, 
                [option_value] NVARCHAR2(2048),
                [last_update] DATETIME NOT NULL,
                CONSTRAINT [] PRIMARY KEY ([option_key]) ON CONFLICT FAIL);
                CREATE INDEX [IX_options_option_key] ON [options] ([option_key]);

                CREATE TABLE [roots] (
                    [root_id] char(36) NOT NULL,
                    [root_path] NVARCHAR2(4000) NOT NULL,
                    [is_default] int(1) NOT NULL,
                    [last_update] DATETIME NOT NULL);
                    CREATE INDEX [IX_root_root_id] ON [roots] ([root_id]);
            ";
        #endregion

        #region Static Variables
        internal static string ConfigRepositoryFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.PROGRAM_NAME, Constants.CONFIG_FILE);
        internal static string RepositoryFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.PROGRAM_NAME);
        internal static string ConfigConnectionString = String.Format(Constants.CONNECTION_STRING_FORMAT, ConfigRepositoryFullPath);

        private static string[] _optionNames = { Constants.CONFIRM_RECRAWL, Constants.VS_LOCATION };
        private static string[] _optionInitialValues = { @"1", Constants.DEFAULT_VS_LOCATION };

        #endregion

        private static void CheckRepositoryFile()
        {
            if (!Directory.Exists(RepositoryFolder))
            {
                Directory.CreateDirectory(RepositoryFolder);
            }

            if (!File.Exists(ConfigRepositoryFullPath))
            {
                SQLiteConnection.CreateFile(ConfigRepositoryFullPath);
                CreateConfigSchema();
                InsertDefaultOptionRows();
            }

            //Check integrity of file
            if (String.IsNullOrWhiteSpace(GetOptionValue(Constants.VS_LOCATION, false)))
            {
                File.Delete(ConfigRepositoryFullPath);
                SQLiteConnection.CreateFile(ConfigRepositoryFullPath);
                CreateConfigSchema();
                InsertDefaultOptionRows();
            }
        }

        private static void InsertDefaultOptionRows()
        {
            for (int i = 0; i <= _optionNames.Length - 1; i++)
            {
                ExecuteNonQuery(String.Format("insert into options (option_key,option_value,last_update) values ({0},{1},'{2}')",
                    _optionNames[i].SafeStringToSQL(), _optionInitialValues[i].SafeStringToSQL(), DateTime.Now.ToString(Constants.DATETIME_FORMAT)));
            }
        }

        public static bool RepoHasData()
        {
            
            var ds = ExecuteQuery("select count(*) from solutions");
            return ds.Tables[0].Rows[0][0].SafeToInt32() > 0;
        }

        internal static string GetDefaultRootId()
        {
            try
            {
                CheckRepositoryFile();
                var ds = RepositoryUtils.ExecuteQuery("select root_id from roots where is_default=1"); // should be only 1 record

                if (ds.Tables[0].Rows.Count == 1)
                {
                    return ds.Tables[0].Rows[0]["root_id"].ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static string GetOptionValue(string keyName, bool CheckRepo = true)
        {
            try
            {
                if (CheckRepo)
                {
                    CheckRepositoryFile();
                }

                var dsOption = ExecuteQuery(string.Format("select option_value from options where option_key={0}", keyName.SafeStringToSQL()));
                return dsOption.Tables[0].Rows[0]["option_value"].ToString();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        internal static void UpdateOptionValue(string optionKey, string optionValue)
        {
            try
            {
                CheckRepositoryFile();
                ExecuteNonQuery(String.Format("update options set option_value={0} where option_key={1}", optionValue.SafeStringToSQL(), optionKey.SafeStringToSQL()));
            }
            catch (Exception ex)
            {
                return;
            }
        }

        internal static void ExecuteNonQuery(string nonQuery)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigConnectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(nonQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        internal static DataSet ExecuteQuery(string query)
        {
            var ds = new DataSet();
            using (var conn = new SQLiteConnection(ConfigConnectionString))
            using (var cmd = new SQLiteCommand(query, conn))
            {
                conn.Open();
                var da = new SQLiteDataAdapter(cmd);
                da.Fill(ds);
            }

            return ds;
        }

        private static void CreateConfigSchema()
        {
            ExecuteNonQuery(CREATE_CONFIG_DB);
        }

        public static void Upsert(RootObject root)
        {
            try
            {
                if (RecordExists("roots", "root_id", root.RootId))
                {
                    var SQL = "update roots set root_path={0},is_default={1},last_update='{2}' where root_id={3}";
                    ExecuteNonQuery(String.Format(SQL, root.RootPath.SafeStringToSQL(),
                        root.IsDefault ? "1" : "0",
                        DateTime.Now.ToString(Constants.DATETIME_FORMAT),
                        root.RootId.SafeStringToSQL()));
                }
                else
                {
                    var SQL = "insert into roots (root_id,root_path,is_default,last_update) VALUES ({0},{1},{2},'{3}')";
                    ExecuteNonQuery(String.Format(SQL,
                        root.RootId.SafeStringToSQL(),
                        root.RootPath.SafeStringToSQL(),
                        root.IsDefault ? "1" : "0",
                        DateTime.Now.ToString(Constants.DATETIME_FORMAT)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteRoot (string rootId)
        {
            try
            {
                var SQL = "delete from roots where root_id={0}";
                ExecuteNonQuery(string.Format(SQL, rootId.SafeStringToSQL()));
                var repoFile = Path.Combine(RepositoryFolder, String.Format(Constants.REPO_FILE_FORMAT, rootId));
                if (File.Exists(repoFile))
                {
                    File.Delete(repoFile);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Collection<RootObject> GetAllRoots()
        {
            var ds = ExecuteQuery("select * from roots order by root_path");
            var ret = new Collection<RootObject>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ret.Add(new RootObject
                {
                    RootId = dr["root_id"].ToString(),
                    RootPath = dr["root_path"].ToString(),
                    IsDefault = Convert.ToInt32(dr["is_default"]) == 1 ? true : false,
                    LastUpdate = Convert.ToDateTime(dr["last_update"].ToString()),
                });
            }
            return ret;
        }

        public static bool RecordExists(string TableName, string KeyColumn, string Id)
        {
            try
            {
                var sql = "select * from {0} where {1}={2}";
                var ds = ExecuteQuery(string.Format(sql, TableName, KeyColumn, Id.SafeStringToSQL()));
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void GetFilesOfType(string MatchPattern, string currentDir, ref Collection<string> AllFiles)
        {
            var filesInThisRoot = Directory.GetFiles(currentDir, MatchPattern);
            foreach (var f in filesInThisRoot)
            {
                AllFiles.Add(f);
            }

            var dirs = Directory.GetDirectories(currentDir);
            foreach (var d in dirs)
            {
                GetFilesOfType(MatchPattern, d, ref AllFiles);
            }
        }

        private static void InsertAllSolutions(Collection<string> solutions)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConfigConnectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                var SQLsol = "insert into solutions (solution_id,solution_file) values ({0},{1})";
                var SQLProj = "insert into projects (project_id,solution_id,project_file) values ({0},{1},{2})";
                var SQLSource = "insert into source_files (source_file_id,project_id,solution_id,source_file) values ({0},{1},{2},{3})";
                conn.Open();
                foreach (var s in solutions)
                {
                    var newSolId = Guid.NewGuid().ToString();

                    cmd.CommandText = String.Format(SQLsol, newSolId.SafeStringToSQL(), s.SafeStringToSQL());
                    cmd.ExecuteNonQuery();

                    //insert projects in this solution
                    var projs = GetAllProjectsInSolution(s);
                    foreach (var p in projs)
                    {
                        var newProjId = Guid.NewGuid().ToString();
                        cmd.CommandText = String.Format(SQLProj, newProjId.SafeStringToSQL(), newSolId.SafeStringToSQL(), p.SafeStringToSQL());
                        cmd.ExecuteNonQuery();

                        var sourceFiles = GetAllSourceFiles(p);
                        foreach (var sf in sourceFiles)
                        {
                            var newFileId = Guid.NewGuid().ToString();
                            cmd.CommandText = String.Format(SQLSource, newFileId.SafeStringToSQL(), newProjId.SafeStringToSQL(), newSolId.SafeStringToSQL(), sf.SafeStringToSQL());
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
            }
        }

        private static Collection<string> GetAllProjectsInSolution(string solutionFile)
        {
            var fStream = File.OpenRead(solutionFile);
            const Int32 BufferSize = 128;
            const string proj = ".csproj";
            var line = String.Empty;
            var projects = new Collection<string>();
            var directoryOnly = Path.GetDirectoryName(solutionFile);

            using (var sReader = new StreamReader(fStream, Encoding.UTF8, true, BufferSize))
            {
                while ((line = sReader.ReadLine()) != null)
                {
                    if (line.Contains(proj) && line.Contains("\""))
                    {
                        var secondQuote = line.IndexOf(proj) + proj.Length - 1;
                        var firstQuote = line.IndexOf(proj) - 1;
                        var quoteChar = line.Substring(firstQuote, 1);
                        while (quoteChar != "\"")
                        {
                            quoteChar = line.Substring(firstQuote, 1);
                            firstQuote--;
                        }
                        var wholeProj = line.Substring(firstQuote + 2 , secondQuote - firstQuote - 1);
                        projects.Add(String.Concat(directoryOnly, Path.DirectorySeparatorChar, wholeProj));
                    }
                }
            }
            return projects;

        }

        private static Collection<string> GetAllSourceFiles(string ProjectFile)
        {
            var directoryOnly = Path.GetDirectoryName(ProjectFile);
            var xDoc = new XmlDocument();
            var csFilesRet = new Collection<string>();
            
            if (File.Exists(ProjectFile))
            {
                xDoc.Load(ProjectFile);
                var manager = new XmlNamespaceManager(xDoc.NameTable);
                var ns = xDoc.DocumentElement.GetNamespaceOfPrefix("");
                manager.AddNamespace("ns", ns);

                var csFiles = xDoc.GetElementsByTagName("Compile", ns);
                foreach (XmlNode fNode in csFiles)
                {
                    var fileName = fNode.Attributes["Include"].Value.ToString();
                    csFilesRet.Add(String.Concat(directoryOnly, Path.DirectorySeparatorChar, fileName));
                }
            }
            return csFilesRet;
        }
       

    }
}
