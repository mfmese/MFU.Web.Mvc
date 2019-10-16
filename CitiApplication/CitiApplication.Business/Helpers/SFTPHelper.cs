using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Citibank.MFU.Web.Business.Helper
{
    internal interface ISFTPHelper
    {
        //TODO : I know i have a problem the signature
        string Read(string path);
    }

    internal class SFTPHelper : ISFTPHelper
    {
        private readonly ISFTPHelper fileStreamSource;

        private string WORKING_DIRECTORY = @"C:\Program Files\Ipswitch\WS_FTP Professional\";
        private string APPLICATION_NAME = "wsftppro.exe";
        private string APPLICATION_PARAMETER_FORMAT = "-s \"ftp:{0}\" -d \"local:{1}\" -ascii";
        private const string TEMP_DIRECTORY = "Project.Temp";
        private const string TEMP_FILE_FORMAT = @"{0}\{1}\{2}";
        private const string TEMP_FILE_EXTENSION = ".txt";
        private const string CONFIGURATION_NAME = "Citibank.MFU";
        private const string APPLICATION_FULL_PATH_KEY = "C:\\citi_projects\\WS_FTP 12\\wsftppro.exe";
        private const string SFTP_APPLICATION_PARAMETER_FORMAT = "-s \"ftp:{0}\" -d \"local:{1}\" -ascii";

        public SFTPHelper()
        {
            String ApplicationFullPath = APPLICATION_FULL_PATH_KEY;
            if (!File.Exists(ApplicationFullPath))
            {
                throw new Exception(string.Format("Sftp application could not found : [Application Path : {0}]", ApplicationFullPath));
            }
            WORKING_DIRECTORY = Path.GetDirectoryName(ApplicationFullPath) + "\\";
            APPLICATION_NAME = Path.GetFileName(ApplicationFullPath);
        }

        public SFTPHelper(ISFTPHelper fileStreamSource) : this()
        {
            this.fileStreamSource = fileStreamSource;
        }

        public string Read(string path)
        {
            string downloadedFile = DownloadFile(path);
            string content = ReadFile(downloadedFile);
            DeleteFile(downloadedFile);

            return content;
        }

        private void DeleteFile(string downloadedFile)
        {
            File.Delete(downloadedFile);
        }

        private string ReadFile(string downloadedFile)
        {
            return fileStreamSource.Read(downloadedFile);
        }

        private string DownloadFile(string sourcePath)
        {
            Assembly executionAssembly = Assembly.GetExecutingAssembly();
            string executionPath = Path.GetDirectoryName(executionAssembly.Location);

            string tempFileName = Guid.NewGuid().ToString() + TEMP_FILE_EXTENSION;
            string tempDestinationPath = string.Format(TEMP_FILE_FORMAT, executionPath, TEMP_DIRECTORY, tempFileName);
            string parameter = string.Format(APPLICATION_PARAMETER_FORMAT, sourcePath, tempDestinationPath);

            Process process = new Process();
            ProcessStartInfo processStartInfo = new ProcessStartInfo(WORKING_DIRECTORY + APPLICATION_NAME, parameter);
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.WorkingDirectory = WORKING_DIRECTORY;
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                string message = string.Format("{0} file can not download", sourcePath);
                throw new Exception(message);
            }

            return tempDestinationPath;
        }
    }
}
