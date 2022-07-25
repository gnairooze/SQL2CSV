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
        private readonly List<SQL2CSV.Interfaces.Models.Settings> _Settings;
        public ConfigurationHelper()
        {
            _Config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddXmlFile("appsettings.xml")
                           .Build();

            _Settings = ReadSettingsFromConfig();
        }

        private List<Settings> ReadSettingsFromConfig()
        {
            List<Settings> settingsList = new();

            var settingsConfigs = _Config.GetSection("settings").GetChildren();

            foreach (var settingsConfig in settingsConfigs)
            {
                var settings = new SQL2CSV.Interfaces.Models.Settings();

                settings.CSVSetting = new SQL2CSV.Interfaces.Models.CSVSettings()
                {
                    NewLineReplacement = settingsConfig.GetSection("csvSettings")["newLineReplacement"],
                    Quote = bool.Parse(settingsConfig.GetSection("csvSettings")["quote"]),
                    QuoteReplacement = settingsConfig.GetSection("csvSettings")["quoteReplacement"],
                    Separator = settingsConfig.GetSection("csvSettings")["separator"],
                    SeparatorReplacement = settingsConfig.GetSection("csvSettings")["separatorReplacement"]
                };

                settings.DataSetting = new SQL2CSV.Interfaces.Models.DataSettings()
                {
                    ConnectionString = settingsConfig.GetSection("dataSettings")["connectionString"],
                    MaxRecords = int.Parse(settingsConfig.GetSection("dataSettings")["maxRecords"]),
                    SQL = settingsConfig.GetSection("dataSettings")["sql"],
                    Timeout = int.Parse(settingsConfig.GetSection("dataSettings")["timeout"]),
                };

                settings.EmailSetting = new SQL2CSV.Interfaces.Models.EmailSettings()
                {
                    MailBody = settingsConfig.GetSection("emailSettings")["mailBody"],
                    MailSubjectPrefix = settingsConfig.GetSection("emailSettings")["mailSubjectPrefix"],
                    MailTo = settingsConfig.GetSection("emailSettings")["mailTo"],
                    SendMail = bool.Parse(settingsConfig.GetSection("emailSettings")["sendMail"])
                };

                settings.FileSetting = new SQL2CSV.Interfaces.Models.FileSettings()
                {
                    BatchFormat = settingsConfig.GetSection("fileSettings")["batchFormat"],
                    BufferWrite = int.Parse(settingsConfig.GetSection("fileSettings")["bufferWrite"]),
                    Compress = bool.Parse(settingsConfig.GetSection("fileSettings")["compress"]),
                    DateFormat = settingsConfig.GetSection("fileSettings")["dateFormat"],
                    DeleteFile = bool.Parse(settingsConfig.GetSection("fileSettings")["deleteFile"]),
                    FileName = settingsConfig.GetSection("fileSettings")["fileName"],
                    RelativeFilePath = settingsConfig.GetSection("fileSettings")["relativeFilePath"],
                };

                settingsList.Add(settings);
            }

            return settingsList;
        }

        public List<Settings> Settings
        {
            get 
            { 
                return this._Settings; 
            }
        }
    }
}
