using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SourceCrawler
{
    public class RepositoryFile
    {
        private string _SQLCSGrep = @"select sf.source_file_id, s.solution_file [Solution], p.project_file [Project], sf.source_file [Source], d.dll_name [DLLName]
                from source_files sf 
                inner join solutions s on s.solution_id=sf.solution_id
                inner join projects p on p.project_id=sf.project_id
                inner join dlls d on p.dll_id = d.dll_id
                where {0} order by s.solution_file limit 5000";

        //private string _AndClause = " and sf.source like '%{0}%' COLLATE [NOCASE]";

        private readonly RootObject _root;
        public RootObject Root
        {
            get
            {
                return _root;
            }
        }

        public string FileConnectString
        {
            get
            {
                return String.Format(Constants.CONNECTION_STRING_FORMAT, _root.FullRootRepoFile);
            }
        }

        private SQLiteConnection _cacheConnection;
        public SQLiteConnection CacheConnection
        {
            get
            {
                if (_cacheConnection == null)
                {
                    _cacheConnection = new SQLiteConnection(Constants.CACHE_CONNSTRING);
                    _cacheConnection.Open();
                }

                return _cacheConnection;
            }
        }

        private int _solutionCount = -1;
        public int SolutionCount
        {
            get
            {
                return _solutionCount;
            }
        }

        #region Constructors
        public RepositoryFile(string RootId, IProgress<ProgressResult> prg)
        {
            _root = new RootObject();
            _root.Refresh(RootId);
            CheckRepositoryFile();
            File2Cache(prg);
        }
        #endregion

        #region Methods
       
        private void CheckRepositoryFile()
        {
            if (!Directory.Exists(RepositoryUtils.RepositoryFolder)) //shouldn't happen at this point
            {
                Directory.CreateDirectory(RepositoryUtils.RepositoryFolder);
            }

            if (!File.Exists(_root.FullRootRepoFile))
            {
                SQLiteConnection.CreateFile(_root.FullRootRepoFile);
                CreateFileSchema();
            }
        }

        private void File2Cache(IProgress<ProgressResult> prg)
        {
            var cmd = new SQLiteCommand(Constants.CREATE_REPO_DB, CacheConnection);
            cmd.ExecuteNonQuery();

            var Attach = String.Format("ATTACH DATABASE {0} as FileBased", _root.FullRootRepoFile.SafeStringToSQL());
            cmd.CommandText = Attach;
            cmd.ExecuteNonQuery();

            var InsertInto = "insert into {0} select * from FileBased.{0}";
            
            cmd.CommandText = String.Format(InsertInto, "source_files");
            cmd.ExecuteNonQuery();
            prg.Report(new ProgressResult { ProgressValue = 20, WorkingOn = "Caching source files..."});
            
            cmd.CommandText = String.Format(InsertInto, "dlls");
            cmd.ExecuteNonQuery();
            prg.Report(new ProgressResult { ProgressValue = 40, WorkingOn = "DLLs..." });

            cmd.CommandText = String.Format(InsertInto, "projects");
            cmd.ExecuteNonQuery();
            prg.Report(new ProgressResult { ProgressValue = 60, WorkingOn = "Projects..." });

            cmd.CommandText = String.Format(InsertInto, "solutions");
            cmd.ExecuteNonQuery();
            prg.Report(new ProgressResult { ProgressValue = 80, WorkingOn = "Solutions..." });

            cmd.CommandText = "DETACH DATABASE FileBased";
            cmd.ExecuteNonQuery();
            prg.Report(new ProgressResult { ProgressValue = 100, WorkingOn = String.Empty });
        }

        public int GetSourceCount()
        {
            var ds = ExecuteQueryCache("select count(*) from source_files");
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        public void ClearCache()
        {
            if (_cacheConnection != null && _cacheConnection.State != ConnectionState.Closed)
            {
                _cacheConnection.Close();
                _cacheConnection = null;
            }
        }

        public void CrawlSource(IProgress<ProgressResult> prog )
        {
            try
            {
                ClearCache();

                //clear out all old data from file
                ExecuteNonQueryFile("delete from source_files");
                ExecuteNonQueryFile("delete from dlls");
                ExecuteNonQueryFile("delete from projects");
                ExecuteNonQueryFile("delete from solutions");

                ExecuteNonQueryFile("vacuum");

                //Solutions
                var sols = new Collection<string>();
                GetFilesOfType("*.sln", _root.RootPath, ref sols);
                _solutionCount = sols.Count;
                InsertAllSolutionsToCache(sols, prog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        
        public bool AnySolutionsInRepo()
        {
            var ds = ExecuteQueryFile("select count(*) from solution_files");
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
        }

        private void ExecuteNonQueryCache(string nonQuery)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(nonQuery, CacheConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void ExecuteNonQueryFile(string nonQuery)
        {
            using (SQLiteConnection conn = new SQLiteConnection(FileConnectString))
            using (SQLiteCommand cmd = new SQLiteCommand(nonQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private DataSet ExecuteQueryCache(string query)
        {
            var ds = new DataSet();
            using (var cmd = new SQLiteCommand(query, CacheConnection))
            {
                var da = new SQLiteDataAdapter(cmd);
                da.Fill(ds);
            }

            return ds;
        }

        private DataSet ExecuteQueryFile(string query)
        {
            var ds = new DataSet();
            using (var conn = new SQLiteConnection(FileConnectString))
            using (var cmd = new SQLiteCommand(query, conn))
            {
                conn.Open();
                var da = new SQLiteDataAdapter(cmd);
                da.Fill(ds);
            }

            return ds;
        }

        private void CreateCacheSchema()
        {
            ExecuteNonQueryCache(Constants.CREATE_REPO_DB);
        }

        private void CreateFileSchema()
        {
            ExecuteNonQueryFile(Constants.CREATE_REPO_DB);
        }

        private static void GetFilesOfType(string MatchPattern, string currentDir, ref Collection<string> AllFiles)
        {
            string[] filesInThisRoot = null;
            try
            {
                filesInThisRoot = Directory.GetFiles(currentDir, MatchPattern);
            }
            catch (PathTooLongException ptle)
            {
                //eat this
            }
            //catch (DirectoryNotFoundException noDirEx)
            //{

            //}

            if (filesInThisRoot != null)
            {
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
        }

        private void InsertAllSolutionsToCache(Collection<string> solutions, IProgress<ProgressResult> prog)
        {
            var cmd = new SQLiteCommand(CacheConnection);
            cmd.CommandText = Constants.CREATE_REPO_DB;
            cmd.ExecuteNonQuery();

            var SQLsol = "insert into solutions (solution_id,solution_path,solution_file) values ('{0}','{1}','{2}')";
            var SQLProj = "insert into projects (project_id,solution_id,dll_id,project_path,project_file) values ('{0}','{1}','{2}','{3}','{4}')";
            var SQLSource = "insert into source_files (source_file_id,project_id,solution_id,source_path,source_file,source) values ('{0}','{1}','{2}','{3}','{4}','{5}')";
            var SQLDll = "insert into dlls (dll_id,dll_name) values ('{0}','{1}')";
            
            var solCounter = 0;
            using (SQLiteTransaction trans = CacheConnection.BeginTransaction())
            {
                foreach (var s in solutions)
                {
                    solCounter++;
                    var current = ((float)solCounter / (float)solutions.Count) * 100;

                    prog.Report(new ProgressResult { ProgressValue = (int)current, WorkingOn = s});

                    var newSolId = Guid.NewGuid().ToString();

                    cmd.CommandText = String.Format(SQLsol, newSolId, Path.GetDirectoryName(s).FixForSQL(), Path.GetFileName(s).FixForSQL());
                    cmd.ExecuteNonQuery();

                    //insert projects in this solution
                    var projs = GetAllProjectsInSolution(s);
                    foreach (var p in projs)
                    {
                        //insert into dll table first
                        var newDLLId = Guid.NewGuid().ToString();
                        cmd.CommandText = String.Format(SQLDll, newDLLId, p.DLLName);
                        cmd.ExecuteNonQuery();

                        var newProjId = Guid.NewGuid().ToString();
                        cmd.CommandText = String.Format(SQLProj, newProjId, newSolId, newDLLId, Path.GetDirectoryName(p.ProjectName).FixForSQL(), Path.GetFileName(p.ProjectName).FixForSQL());
                        cmd.ExecuteNonQuery();

                        var sourceFiles = GetAllSourceFiles(p.ProjectName);
                        foreach (var sf in sourceFiles)
                        {
                            var newFileId = Guid.NewGuid().ToString();
                            var lines = String.Empty;
                            try
                            {
                                if (File.Exists(Uri.UnescapeDataString(sf.Replace("?", String.Empty))))
                                {
                                    lines = File.ReadAllText(Uri.UnescapeDataString(sf));
                                    cmd.CommandText = String.Format(SQLSource, newFileId, newProjId, newSolId, Path.GetDirectoryName(sf).FixForSQL(), Path.GetFileName(sf).FixForSQL(), lines.FixForSQL());
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                //ignore
                            }
                        }
                    }
                }
                trans.Commit();
            }

            
            //put into file-based database
            Cache2File();
        }

        private void Cache2File()
        {
            var cmd = new SQLiteCommand(CacheConnection);
            cmd.CommandText = String.Format("ATTACH DATABASE '{0}' as FileBased", _root.FullRootRepoFile.FixForSQL());
            cmd.ExecuteNonQuery();

            var InsertInto = "insert into FileBased.{0} select * from {0}";
            cmd.CommandText = String.Format(InsertInto, "source_files");
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(InsertInto, "dlls");
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(InsertInto, "projects");
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(InsertInto, "solutions");
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DETACH DATABASE FileBased";
            cmd.ExecuteNonQuery();
        }

        private Collection<ProjectResults> GetAllProjectsInSolution(string solutionFile)
        {
            const string proj = ".csproj";

            var projects = new Collection<ProjectResults>();
            var directoryOnly = Path.GetDirectoryName(solutionFile);

            var SolFileLines = File.ReadLines(solutionFile);
            foreach (var line in SolFileLines)
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
                    var wholeProj = line.Substring(firstQuote + 2, secondQuote - firstQuote - 1);
                    
                    var FullyQualifiedProjectName = String.Concat(directoryOnly, Path.DirectorySeparatorChar, wholeProj);
                    var DLLName = GetDLLName(FullyQualifiedProjectName);

                    if (!FullyQualifiedProjectName.Contains("..")) //don't add referenced projects too
                    {
                        projects.Add(new ProjectResults
                        {
                            ProjectName = FullyQualifiedProjectName,
                            DLLName = DLLName
                        });
                    }
                }
            }

            return projects;
        }

        private string GetDLLName(string FullyQualifiedProjectName)
        {
                var xFile = new XmlDocument();
                var AssemblyName = String.Empty;
            try
            {

                if (File.Exists(FullyQualifiedProjectName))
                {
                    //var fStream = File.OpenRead(FullyQualifiedProjectName);
                    var fStream = new StreamReader(FullyQualifiedProjectName, Encoding.UTF8);
                    var xmlReader = new XmlTextReader(fStream) {Namespaces = false};

                    //xFile.Load(fStream); // copyright signs screw this up ©
                    xFile.Load(xmlReader);

                    var outputTypeNode = xFile.SelectSingleNode("/Project/PropertyGroup/OutputType");
                   // var outputTypeNodes = xFile.SelectNodes("/Project/PropertyGroup");
                    //outputTypeNode = outputTypeNodes[0];
                    if (outputTypeNode != null)
                    {
                        var OutputType = outputTypeNode.InnerText;
                        if (OutputType.ToLower() == "library")
                        {
                            AssemblyName = String.Concat(xFile.SelectSingleNode("/Project/PropertyGroup/AssemblyName").InnerText, ".dll");
                        }
                        else if (OutputType.ToLower() == "winexe")
                        {
                            if (xFile.SelectSingleNode("/Project/PropertyGroup/AssemblyName") != null)
                            {
                                AssemblyName = String.Concat(xFile.SelectSingleNode("/Project/PropertyGroup/AssemblyName").InnerText, ".exe");
                            }
                            else
                            {
                                //Service without an AssemblyName node, so just use the project name?
                                AssemblyName = String.Concat(Path.GetFileNameWithoutExtension(FullyQualifiedProjectName), ".exe");
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                //eat this
            }

            return AssemblyName;
        }

        private Collection<string> GetAllSourceFiles(string ProjectFile)
        {
            var directoryOnly = Path.GetDirectoryName(ProjectFile);
            var xDoc = new XmlDocument();
            var csFilesRet = new Collection<string>();

            if (File.Exists(ProjectFile))
            {
                var fStream = new StreamReader(ProjectFile, Encoding.UTF8);
                //var xmlReader = new XmlTextReader(fStream) { Namespaces = false };
                //xDoc.Load(ProjectFile);
                xDoc.Load(fStream);
                var manager = new XmlNamespaceManager(xDoc.NameTable);
                var ns = xDoc.DocumentElement.GetNamespaceOfPrefix("");
                manager.AddNamespace("ns", ns);

                var csFiles = xDoc.GetElementsByTagName("Compile", ns);
                foreach (XmlNode fNode in csFiles)
                {
                    if (fNode.Attributes["Include"] != null)
                    {
                        var fileName = String.Concat(directoryOnly, Path.DirectorySeparatorChar, fNode.Attributes["Include"].Value.ToString());

                        if (!csFilesRet.Contains(fileName) && !fileName.Contains(".."))
                        {
                            csFilesRet.Add(fileName);
                        }
                    }

                    if (fNode.ChildNodes.Count > 0)
                    {
                        if (fNode.ChildNodes[0].Name == "DependentUpon")
                        {
                            var depFile = String.Concat(directoryOnly, Path.DirectorySeparatorChar, fNode.ChildNodes[0].InnerText);
                            if (!csFilesRet.Contains(depFile))
                            {
                                csFilesRet.Add(depFile);
                            }
                        }
                    }
                }

                if (!csFilesRet.Any())
                {
                    //No files found in the .csproj; iterate .cs files recursively starting in the current directory
                    var nonProjCSFiles = new Collection<string>();
                    GetFilesOfType("*.cs", Path.GetDirectoryName(ProjectFile), ref nonProjCSFiles);
                    foreach (var csFile in nonProjCSFiles.Where(c => !c.Contains("..")))
                    {
                        csFilesRet.Add(csFile);
                    }

                }

            }
            return csFilesRet;
        }
        
        public DataSet DoSearch(string SourceFile, string CodeGrep, string DLL, Enums.Opers[] oper)
        {
            if (String.IsNullOrWhiteSpace(SourceFile) && String.IsNullOrWhiteSpace(CodeGrep) && String.IsNullOrWhiteSpace(DLL))
            {
                //nothing entered
                return new DataSet();
            }

            var cols = new Collection<Column>();

            if (!String.IsNullOrWhiteSpace(SourceFile))
            {
                cols.Add(new Column
                {
                    ColumnName = "sf.source_file",
                    Operator = oper[0],
                    ColumnValue = SourceFile
                });
            };

            if (!String.IsNullOrWhiteSpace(CodeGrep))
            {
                cols.Add(new Column
                {
                    ColumnName = "sf.source",
                    Operator = Enums.Opers.operContains,
                    ColumnValue = EscapeSpecialCharacters(CodeGrep)
                });
            };

            if (!String.IsNullOrWhiteSpace(DLL))
            {
                cols.Add(new Column
                {
                    ColumnName = "d.dll_name",
                    Operator =oper[1],
                    ColumnValue = DLL
                });
            };

            var finalWhere = WhereClauseManager.GetWhereClause(cols);
            var finalSQL = String.Format(_SQLCSGrep, finalWhere);

#if DEBUG
            System.Diagnostics.Trace.WriteLine(finalSQL);
            System.Diagnostics.Trace.WriteLine(String.Empty);
#endif
            return ExecuteQueryCache(finalSQL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SourceId"></param>
        /// <returns></returns>
        public SourceData GetSourceData(string SourceId)
        {
            var SQL = String.Format(@"select sol.solution_path, sol.solution_file, sf.source, sf.source_path, sf.source_file, pr.project_path, pr.project_file
                from source_files sf
                inner join solutions sol on sf.solution_id=sol.solution_id
                inner join projects pr on sf.project_id=pr.project_id
                where sf.source_file_id='{0}'", SourceId);
            
            var ds = ExecuteQueryCache(SQL);

            return new SourceData
            {
                FullSourceText = ds.Tables[0].Rows[0][2].ToString(),
                PathToSolution = ds.Tables[0].Rows[0][0].ToString(),
                PathToSource = ds.Tables[0].Rows[0][3].ToString(),
                SolutionFileName = ds.Tables[0].Rows[0][1].ToString(),
                SourceFileName = ds.Tables[0].Rows[0][4].ToString(),
                PathToProject = ds.Tables[0].Rows[0][5].ToString(),
                ProjectFileName = ds.Tables[0].Rows[0][6].ToString()
            };
        }

        private string EscapeSpecialCharacters(string stringIn)
        {
            var final = new StringBuilder();
            foreach (var s in stringIn.ToCharArray())
            {
                if (Constants.CHARS_TO_ESCAPE.Contains(s.ToString()))
                {
                    final.Append(Constants.ESCAPE_CHAR + s);
                }
                else
                {
                    final.Append(s);
                }
            }
            return final.ToString();
        }

        #endregion
    }
}
