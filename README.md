# SourceCrawler Repository

SourceCrawler is designed to search a large monolithic C# code base using an in-memory database. It identifies the project (.csproj), solution (.sln) and assembly (.dll) file where the source exists. SourceCrawler will read an entire file system's tree structure, starting from whatever location the user specifies, and organize it in memory to facilitate extremely fast searching over thousands of .cs files.

## Getting Started
1. Clone the repository
2. Compile the code in VS (created with VS 2017) or MSBuild
3. Or you could run the latest install in the Release tab

## Program execution and use
1. Click "Root Management"
2. Click the "+" button on the lower left to add at least one source root, then click Ok. It will then “crawl” that source tree

## Basic Use
1. To search for a specific .cs file name, use the “Source file match” text box, and select the appropriate operator ("Contains", "Equals", "StartsWith", "EndsWith")
2. To grep the entire code base, enter that into the “Code search” text box. If both text boxes have values, it will “AND” them together.
3. You can also search based on the DLL or EXE name in the "DLL/EXE search" text box
4. Double-click on the .sln file to open it in Visual Studio. (You may need to configure the path to devenv.exe in the Options dialog.)
5. Search is case-insensitive.
6. You can add other roots and just select that row on the Manage Roots screen to make it the default root cached.
7. You can use the "-crawldefault" command line parameter to re-crawl the default root without invoking the UI; this could be used at the end of a get latest source pipeline.

## Known limitations
1. No more than 5000 results will be returned to the grid
2. Source code is not editable here--this is a searching tool
3. "Code search" doesn't [yet] have option to search on "Full word", nor is it a RegEx

## Questions, bugs, comments
[tkashin@salesforce.com](mailto:tkashin@salesforce.com)
