using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SQL2CSV.Business
{
    public class FileManager : SQL2CSV.Interfaces.IFileManager
    {
        #region properties
        private readonly Interfaces.Models.Settings _Settings;
        private string _FileFullPath;
        private string _FileName;

        public string FileName
        {
            get
            {
                return _FileName;
            }
        }

        public string FileFullPath 
        { 
            get 
            { 
                return _FileFullPath; 
            } 
        }

        public string AbsoluteOutputDirectory
        {
            get
            {
                string? executionDirectory = System.IO.Path.GetDirectoryName(Environment.CurrentDirectory);
                if (executionDirectory == null)
                {
                    return string.Empty;
                }
                string absoluteOutputDirectory = Path.GetFullPath(Path.Combine(executionDirectory, this._Settings.FileSetting.RelativeFilePath));

                return absoluteOutputDirectory;
            }
        }
        #endregion

        public FileManager(Interfaces.Models.Settings settings)
        {
            _Settings = settings;
            _FileFullPath = string.Empty;
            _FileName = string.Empty;
        }
        public void DeleteFile()
        {
            if (!this._Settings.FileSetting.DeleteFile)
            {
                return;
            }

            string fileSearch = this._Settings.FileSetting.FileName.Replace(Interfaces.Models.FileSettings.DATE_PARAMETER, "*").Replace(Interfaces.Models.FileSettings.BATCH_PARAMETER, "*") + ".csv*";

            string[] fileNames = Directory.GetFiles(AbsoluteOutputDirectory, fileSearch);

            foreach (var fileName in fileNames)
            {
                string fileFullPath = Path.Combine(AbsoluteOutputDirectory, fileName);

                File.Delete(fileFullPath);

                Console.WriteLine($"{fileFullPath} deleted.");
            }
        }
        public void CreateFile(int batchCount, List<string> columnNames)
        {
            _FileName = CalculateFileName(this._Settings.FileSetting.FileName, batchCount);
            
            _FileFullPath = Path.Combine(AbsoluteOutputDirectory, FileName);
            File.AppendAllText(FileFullPath, string.Join(this._Settings.CSVSetting.Separator, columnNames.ToArray()), Encoding.UTF8);
            File.AppendAllText(FileFullPath, Environment.NewLine);

            Console.WriteLine("File {0} created with column names.", FileName);
        }

        private string CalculateFileName(string filename, int batchCount)
        {
            if (filename.Contains(Interfaces.Models.FileSettings.DATE_PARAMETER))
            {
                filename = filename.Replace(Interfaces.Models.FileSettings.DATE_PARAMETER, DateTime.Now.ToUniversalTime().ToString(this._Settings.FileSetting.DateFormat));
            }

            if (filename.Contains(Interfaces.Models.FileSettings.BATCH_PARAMETER))
            {
                filename = filename.Replace(Interfaces.Models.FileSettings.BATCH_PARAMETER, batchCount.ToString(this._Settings.FileSetting.BatchFormat));
            }

            return filename + ".csv";
        }

        public void AppendData(string data)
        {
            File.AppendAllText(FileFullPath, data, Encoding.UTF8);
        }
        
        public string CompressWithBuffer()
        {
            if (!this._Settings.FileSetting.Compress)
            {
                return FileFullPath;
            }

            Console.WriteLine($"Compression started.");

            string compressedFileName = FileFullPath + ".gz";

            using (FileStream sourceFile = File.OpenRead(FileFullPath))
            using (FileStream destinationFile = File.Create(compressedFileName))
            using (GZipStream output = new(destinationFile, CompressionMode.Compress))
            {
                sourceFile.CopyTo(output);
            }

            Console.WriteLine($"Compression completed to {compressedFileName}");

            return compressedFileName;
        }
    }
}
