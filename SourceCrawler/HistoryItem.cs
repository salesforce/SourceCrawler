using System;
using System.Linq;

namespace SourceCrawler
{
    public class HistoryItem
    {
        public int Position { get; set; }
        public string FileValue { get; set; }
        public string GrepyValue { get; set; }
        public string DLLValue { get; set; }
        public DateTime Timestamp { get; set; }

        public int RestultCount { get; set; }
        public override string ToString()
        {
            return String.Join(", ", new[] { FileValue, GrepyValue, DLLValue }.Where(s => !String.IsNullOrEmpty(s))) + " (" + RestultCount.ToString() + ")";
        }
    }
}
