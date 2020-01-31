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
