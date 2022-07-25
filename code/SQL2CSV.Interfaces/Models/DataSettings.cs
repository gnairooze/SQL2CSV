using System;
using System.Collections.Generic;
using System.Text;

namespace SQL2CSV.Interfaces.Models
{
    public class DataSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SQL { get; set; } = string.Empty;
        public int Timeout { get; set; }
        public int MaxRecords { get; set; }
    }
}
