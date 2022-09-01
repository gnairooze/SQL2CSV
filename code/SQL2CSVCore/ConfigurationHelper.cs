using Microsoft.Extensions.Configuration;
using SQL2CSV.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SQL2CSVCore
{
    internal class ConfigurationHelper
    {
        private readonly XmlDocument _Config;
        private readonly List<SQL2CSV.Interfaces.Models.Settings> _Settings;

        public List<Settings> Settings
        {
            get
            {
                return this._Settings;
            }
        }

        public ConfigurationHelper()
        {
            _Config = new XmlDocument();
            _Config.Load("appsettings.xml");

            _Settings = ReadSettingsFromConfig();
        }

        private List<Settings> ReadSettingsFromConfig()
        {
            List<Settings> settingsList = new();

            var settingsConfigs = _Config.SelectNodes("//settings");

            foreach (XmlNode settingsConfig in settingsConfigs)
            {
                var settings = new SQL2CSV.Interfaces.Models.Settings();

                settings.CSVSetting = new SQL2CSV.Interfaces.Models.CSVSettings()
                {
                    NewLineReplacement = settingsConfig.SelectSingleNode("csvSettings")["newLineReplacement"].InnerText,
                    Quote = bool.Parse(settingsConfig.SelectSingleNode("csvSettings")["quote"].InnerText),
                    QuoteReplacement = settingsConfig.SelectSingleNode("csvSettings")["quoteReplacement"].InnerText,
                    Separator = settingsConfig.SelectSingleNode("csvSettings")["separator"].InnerText,
                    SeparatorReplacement = settingsConfig.SelectSingleNode("csvSettings")["separatorReplacement"].InnerText
                };

                settings.DataSetting = new SQL2CSV.Interfaces.Models.DataSettings()
                {
                    ConnectionString = settingsConfig.SelectSingleNode("dataSettings")["connectionString"].InnerText,
                    MaxRecords = int.Parse(settingsConfig.SelectSingleNode("dataSettings")["maxRecords"].InnerText),
                    SQL = settingsConfig.SelectSingleNode("dataSettings")["sql"].InnerText,
                    Timeout = int.Parse(settingsConfig.SelectSingleNode("dataSettings")["timeout"].InnerText),
                };

                settings.EmailSetting = new SQL2CSV.Interfaces.Models.EmailSettings()
                {
                    MailBody = settingsConfig.SelectSingleNode("emailSettings")["mailBody"].InnerText,
                    MailSubjectPrefix = settingsConfig.SelectSingleNode("emailSettings")["mailSubjectPrefix"].InnerText,
                    MailTo = settingsConfig.SelectSingleNode("emailSettings")["mailTo"].InnerText,
                    SendMail = bool.Parse(settingsConfig.SelectSingleNode("emailSettings")["sendMail"].InnerText)
                };

                settings.FileSetting = new SQL2CSV.Interfaces.Models.FileSettings()
                {
                    BatchFormat = settingsConfig.SelectSingleNode("fileSettings")["batchFormat"].InnerText,
                    BufferWrite = int.Parse(settingsConfig.SelectSingleNode("fileSettings")["bufferWrite"].InnerText),
                    Compress = bool.Parse(settingsConfig.SelectSingleNode("fileSettings")["compress"].InnerText),
                    DateFormat = settingsConfig.SelectSingleNode("fileSettings")["dateFormat"].InnerText,
                    DeleteFile = bool.Parse(settingsConfig.SelectSingleNode("fileSettings")["deleteFile"].InnerText),
                    FileName = settingsConfig.SelectSingleNode("fileSettings")["fileName"].InnerText,
                    RelativeFilePath = settingsConfig.SelectSingleNode("fileSettings")["relativeFilePath"].InnerText,
                };

                settingsList.Add(settings);
            }

            return settingsList;
        }
    }
}
