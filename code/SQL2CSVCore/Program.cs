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

                int settingsListCounter = 0;

                foreach (var setting in helper.Settings)
                {
                    settingsListCounter++;
                    Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.fff")} - start setting {settingsListCounter}");
                    try
                    {
                        SQL2CSV.Interfaces.IEmailManager emailManager = new SQL2CSV.Business.EmailManager(setting.EmailSetting);
                        SQL2CSV.Interfaces.IFileManager fileManager = new SQL2CSV.Business.FileManager(setting);


                        SQL2CSV.Business.Manager manager = new SQL2CSV.Business.Manager(setting, fileManager, emailManager);

                        manager.ReadData();

                        Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.fff")} - end setting {settingsListCounter}");
                        Console.WriteLine("--------------------------------------------");
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText("Log.txt", ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("Log.txt", ex.ToString());
            }
        }
    }
}
