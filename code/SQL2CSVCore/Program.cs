using System;
using System.IO;

namespace SQL2CSVCore
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (File.Exists("Log.txt"))
                {
                    File.Delete("Log.txt");
                }

                ConfigurationHelper helper = new ConfigurationHelper();
                SQL2CSV.Interfaces.IEmailManager emailManager = new SQL2CSV.Business.EmailManager(helper.EmailSetting);
                SQL2CSV.Interfaces.IFileManager fileManager = new SQL2CSV.Business.FileManager(helper.Settings);

                SQL2CSV.Business.Manager manager = new SQL2CSV.Business.Manager(helper.Settings, fileManager, emailManager);

                manager.ReadData();
            }
            catch (Exception ex)
            {
                File.AppendAllText("Log.txt", ex.ToString());
            }
        }
    }
}
