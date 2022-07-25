using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SQL2CSV.Interfaces.Models
{
    public class CSVSettings
    {
        public string Separator { get; set; } = string.Empty;
        public string SeparatorReplacement { get; set; } = string.Empty;
        public bool Quote { get; set; }
        public string QuoteReplacement { get; set; } = string.Empty;
        public string NewLineReplacement { get; set; } = string.Empty;
    }
}
