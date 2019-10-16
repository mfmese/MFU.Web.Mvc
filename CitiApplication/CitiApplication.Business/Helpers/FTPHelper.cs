using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Citibank.MFU.Web.Business.Helper
{
    internal interface IFTPHelper
    {
        //TODO : I know i have a problem the signature
        string Read(string path);
    }

    internal class FTPHelper : IFTPHelper
    {
        public string Read(string path)
        {
            if (!File.Exists(path))
            {
                string message = string.Format("'{0}' not found", path);
                throw new FileNotFoundException(message, path);
            }

            FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            StreamReader reader = new StreamReader(fileStream);

            string source = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();

            return source;
        }
    }
}
