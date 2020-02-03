/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
 */

namespace SourceCrawler
{
    public class Constants
    {
        #region Table DDL
        public const string CREATE_REPO_DB = @"CREATE TABLE [projects](
            [project_id] char(36) PRIMARY KEY NOT NULL,
            [solution_id] char(36) NOT NULL, 
            [dll_id] char(36) NOT NULL, 
            [project_path] nvarchar2(4000), 
            [project_file] nvarchar2(500));

            CREATE TABLE [dlls](
                [dll_id] char(36) PRIMARY KEY NOT NULL,
                [dll_name] nvarchar2(200));

            CREATE TABLE [solutions](
                [solution_id] char(36) PRIMARY KEY NOT NULL, 
                [solution_path] nvarchar2(4000), 
                [solution_file] nvarchar2(500));

            CREATE TABLE [source_files](
                [source_file_id] char(36) PRIMARY KEY NOT NULL, 
                [project_id] char(36) NOT NULL, 
                [solution_id] char(36) NOT NULL, 
                [source_path] nvarchar2(4000),
                [source_file] nvarchar2(500), 
                [source] text);

            CREATE INDEX [IX_project_file]
            ON [projects](
                [project_file] ASC);
            
            CREATE INDEX [IX_dll_name]
            ON [dlls](
                [dll_name] ASC);

            CREATE INDEX [IX_solution_file]
            ON [solutions](
                [solution_file] ASC);

            CREATE INDEX [IX_source_file]
            ON [source_files](
                [source_file] COLLATE [NOCASE] ASC);
            ";

//        public const string CREATE_DB_NO_IDX = @"CREATE TABLE [projects](
//            [project_id] char(36) NOT NULL, 
//            [solution_id] char(36) NOT NULL, 
//            [project_path] nvarchar2(4000), 
//            [project_file] nvarchar2(500), 
//            PRIMARY KEY([project_id] ASC, [solution_id] ASC));
//
//            CREATE TABLE [solutions](
//                [solution_id] char(36) PRIMARY KEY NOT NULL, 
//                [solution_path] nvarchar2(4000), 
//                [solution_file] nvarchar2(500));
//
//            CREATE TABLE [source_files](
//                [source_file_id] char(36) NOT NULL, 
//                [project_id] char(36) NOT NULL, 
//                [solution_id] char(36) NOT NULL, 
//                [source_path] nvarchar2(4000),
//                [source_file] nvarchar2(500), 
//                [source] text,
//                PRIMARY KEY([source_file_id] ASC, [project_id] ASC, [solution_id] ASC));
//
//            CREATE TABLE [options] (
//                [option_key] VARCHAR(200) NOT NULL, 
//                [option_value] NVARCHAR2(2048),
//                [last_update] DATETIME NOT NULL,
//                CONSTRAINT [] PRIMARY KEY ([option_key]) ON CONFLICT FAIL);
//                CREATE INDEX [IX_options_option_key] ON [options] ([option_key]);
//            ";

//        public const string CREATE_DB_IDX_OLNY = @"CREATE INDEX [IX_project_file]
//            ON [projects](
//                [project_file] ASC);
//
//            CREATE INDEX [IX_solution_file]
//            ON [solutions](
//                [solution_file] ASC);
//
//            CREATE INDEX [IX_source_file]
//            ON [source_files](
//                [source_file] ASC);
//            ";

        #endregion

        public const string PROGRAM_NAME = "SourceCrawlerCommunity";
        public const string CONNECTION_STRING_FORMAT = @"Data Source={0};Version=3;";
        public const string CONFIG_FILE = "SourceCrawlerCommunityRepo.db";
        public const string CACHE_CONNSTRING = "Data Source=:memory:;Version=3;New=True; Journal Mode=Off";
        public const string REPO_FILE_FORMAT = "{0}.db";
        
        //Options
        public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string CONFIRM_RECRAWL = "ConfirmRecrawl";

        public const string DEFAULT_VS_LOCATION = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\devenv.exe";
        public const string VS_LOCATION = "VSLocation";
        
        public const string CHARS_TO_ESCAPE = @"_\%";
        public const string ESCAPE_CHAR = @"\";
    }
}
