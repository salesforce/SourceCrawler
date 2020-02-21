using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceCrawler
{
    public class HistoryItem
    {
        public int Position { get; set; }
        public string HistoryValue { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
