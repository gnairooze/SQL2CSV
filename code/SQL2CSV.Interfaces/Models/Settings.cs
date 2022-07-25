using System;
using System.Collections.Generic;
using System.Text;

namespace SQL2CSV.Interfaces.Models
{
    public class Settings
    {
        public CSVSettings CSVSetting { get; set; }
        public DataSettings DataSetting { get; set; }
        public FileSettings FileSetting { get; set; }
        public EmailSettings EmailSetting { get; set; }

        public Settings()
        {
            this.CSVSetting = new CSVSettings();
            this.DataSetting = new DataSettings();
            this.EmailSetting = new EmailSettings();
            this.FileSetting = new FileSettings();
        }
    }
}
