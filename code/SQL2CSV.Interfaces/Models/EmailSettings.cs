using System;
using System.Collections.Generic;
using System.Text;

namespace SQL2CSV.Interfaces.Models
{
    public class EmailSettings
    {
        public string MailTo { get; set; } = string.Empty;
        public string MailSubjectPrefix { get; set; } = string.Empty;
        public string MailBody { get; set; } = string.Empty;
        public bool SendMail { get; set; }
    }
}
