using System;

namespace SourceCrawler
{
    public class Column
    {
        public string ColumnName { get; set; }
        public Enums.Opers Operator { get; set; }
        public string ColumnValue { get; set; }
        public bool RequiresQuotes {get;set;}
        
        public Column()
        {
            RequiresQuotes = true; //default to this
        }

        public string GetClause()
        {
            var quote = RequiresQuotes ? "'" : String.Empty;
            var leftPercent = Operator == Enums.Opers.operContains || Operator == Enums.Opers.opersEndsWith ? "%" : String.Empty;
            var rightPercent = Operator == Enums.Opers.operContains || Operator == Enums.Opers.operStartWith ? "%" : String.Empty;
            var escapeUnderscores = (Operator == Enums.Opers.operContains || Operator == Enums.Opers.opersEndsWith || Operator == Enums.Opers.operStartWith) && ColumnValue.Contains("_") ? "ESCAPE '\\' " : String.Empty;

            return String.Format("{0} {1} {2}{3}{4}{5}{6} COLLATE [NOCASE] {7}", ColumnName, GetOperatorString, quote, leftPercent, ColumnValue, rightPercent, quote, escapeUnderscores);
        }

        private string GetOperatorString
        {
            get
            {
                switch (Operator)
                {
                    case Enums.Opers.operContains:
                    case Enums.Opers.operStartWith:
                    case Enums.Opers.opersEndsWith:
                        return "like";
                        break;
                    default :
                        return "=";
                        break;
                }
            }
        }
    }
}
