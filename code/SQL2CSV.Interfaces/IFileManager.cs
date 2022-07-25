using System;
using System.Collections.Generic;
using System.Text;

namespace SQL2CSV.Interfaces
{
    public interface IFileManager
    {
        string FileName { get; }
        string FileFullPath { get; }

        void DeleteFile();

        void CreateFile(int batchCount, List<string> columnNames);
        
        //Create file must be called first to set the filename location and then append data will add data to this filename
        void AppendData(string data);

        //Create file must be called first to set the filename location and then compress data will compress this file
        string CompressWithBuffer();
    }
}
