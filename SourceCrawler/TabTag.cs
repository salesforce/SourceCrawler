namespace SourceCrawler
{
    internal class TabTag
    {
        internal string SourceFileName { get; set; }
        internal string CodeGrep { get; set; }
        internal string DLLFileName { get; set; }
        internal int RowSelected { get; set; }
        internal bool FromTabSwitch { get; set; }

        internal TabTag()
        {
            FromTabSwitch = false;
        }
    }
}
