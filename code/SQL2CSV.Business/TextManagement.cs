using System;
using System.Collections.Generic;
using System.Text;

namespace SQL2CSV.Business
{
    internal class TextManagement
    {
        public static  string Sanitize(string input, Interfaces.Models.CSVSettings setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            #region remove new line
            string v = input.Replace(Environment.NewLine, setting.NewLineReplacement);
            v = v.Replace("\n", setting.NewLineReplacement);
            v = v.Replace("\r", setting.NewLineReplacement);
            #endregion

            //remove separator
            v = v.Replace(setting.Separator, setting.SeparatorReplacement);

            #region remove double quotes if double quotes configured as quoted text.
            if (setting.Quote)
            {
                v = v.Replace("\"", setting.QuoteReplacement); 
            }
            #endregion

            return v;
        }
    }
}
