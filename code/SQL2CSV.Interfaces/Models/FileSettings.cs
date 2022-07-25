using System;
using System.Collections.Generic;
using System.Text;

namespace SQL2CSV.Interfaces.Models
{
    public class FileSettings
    {
        #region file name parameters
        public const string DATE_PARAMETER = "{$Date$}";
        public const string BATCH_PARAMETER = "{$Batch$}";

        public string DateFormat { get; set; } = string.Empty;//"dd_MMM_yyyy_UTC_HH_mm_ss_fff"
        public string BatchFormat { get; set; } = string.Empty; //"D3"
        #endregion
        public string FileName { get; set; } = string.Empty;
        public string RelativeFilePath { get; set; } = string.Empty;
        public int BufferWrite { get; set; }
        public bool Compress { get; set; }
        public bool DeleteFile { get; set; }
    }
}
