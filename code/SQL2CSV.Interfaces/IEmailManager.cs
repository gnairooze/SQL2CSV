using System;
using System.Collections.Generic;
using System.Text;

namespace SQL2CSV.Interfaces
{
    public interface IEmailManager
    {
        void SendEmail(string fileName, string compressedFilename);
    }
}
