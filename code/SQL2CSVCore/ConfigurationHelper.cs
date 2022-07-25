using Microsoft.Extensions.Configuration;
using SQL2CSV.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SQL2CSVCore
{
    internal class ConfigurationHelper
    {
        private readonly IConfigurationRoot _Config;
        private readonly SQL2CSV.Interfaces.Models.Settings _Settings;
        public ConfigurationHelper()
        {
            _Config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddXmlFile("appsettings.xml")
                           .Build();

            _Settings = ReadSettingsFromConfig();
        }

        private Settings ReadSettingsFromConfig()
        {
            var settings = new SQL2CSV.Interfaces.Models.Settings();

            settings.CSVSetting = new SQL2CSV.Interfaces.Models.CSVSettings()
            {
                NewLineReplacement = _Config.GetSection("csvSettings")["newLineReplacement"],
                Quote = bool.Parse(_Config.GetSection("csvSettings")["quote"]),
                QuoteReplacement = _Config.GetSection("csvSettings")["quoteReplacement"],
                Separator = _Config.GetSection("csvSettings")["separator"],
                SeparatorReplacement = _Config.GetSection("csvSettings")["separatorReplacement"]
            };

            settings.DataSetting = new SQL2CSV.Interfaces.Models.DataSettings()
            {
                ConnectionString = _Config.GetSection("dataSettings")["connectionString"],
                MaxRecords = int.Parse(_Config.GetSection("dataSettings")["maxRecords"]),
                SQL = _Config.GetSection("dataSettings")["sql"],
                Timeout = int.Parse(_Config.GetSection("dataSettings")["timeout"]),
            };

            settings.EmailSetting = new SQL2CSV.Interfaces.Models.EmailSettings()
            {
                MailBody = _Config.GetSection("emailSettings")["mailBody"],
                MailSubjectPrefix = _Config.GetSection("emailSettings")["mailSubjectPrefix"],
                MailTo = _Config.GetSection("emailSettings")["mailTo"],
                SendMail = bool.Parse(_Config.GetSection("emailSettings")["sendMail"])
            };

            settings.FileSetting = new SQL2CSV.Interfaces.Models.FileSettings()
            {
                BatchFormat = _Config.GetSection("fileSettings")["batchFormat"],
                BufferWrite = int.Parse(_Config.GetSection("fileSettings")["bufferWrite"]),
                Compress = bool.Parse(_Config.GetSection("fileSettings")["compress"]),
                DateFormat = _Config.GetSection("fileSettings")["dateFormat"],
                DeleteFile = bool.Parse(_Config.GetSection("fileSettings")["deleteFile"]),
                FileName = _Config.GetSection("fileSettings")["fileName"],
                RelativeFilePath = _Config.GetSection("fileSettings")["relativeFilePath"],
            };

            return settings;
        }

        public SQL2CSV.Interfaces.Models.Settings Settings
        {
            get 
            { 
                return this._Settings; 
            }
        }

        public SQL2CSV.Interfaces.Models.CSVSettings CSVSetting
        {
            get
            {
                return _Settings.CSVSetting;
            }
        }

        public SQL2CSV.Interfaces.Models.DataSettings DataSetting
        {
            get
            {
                return _Settings.DataSetting;
            }
        }

        public SQL2CSV.Interfaces.Models.EmailSettings EmailSetting
        {
            get
            {
                return _Settings.EmailSetting;
            }
        }


    }
}
