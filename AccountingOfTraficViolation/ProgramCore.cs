using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingOfTraficViolation
{
    enum UserRole : byte
    {
        Debug = 0,
        Admin,
        User
    }
    public interface ILogger
    {
        string ErrorMessage { get; set; }

        void Log();
    }


    public class FileLogger : ILogger
    {
        private string errorMessage;
        private string fileName;
        private string filePath;

        public FileLogger(string fileName) : this(fileName, null, null)
        { }
        public FileLogger(string fileName, string errorMessage) : this(fileName, null, errorMessage)
        { }
        public FileLogger(string fileName, string filePath, string errorMessage)
        {
            ErrorMessage = errorMessage;
            FilePath = filePath;
            FileName = fileName;
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errorMessage = "";
                }
                else
                {
                    errorMessage = value;
                }
            }
        }
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    fileName = value;
                }
            }
        }
        public string FilePath
        {
            get { return filePath; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    filePath = Environment.CurrentDirectory;
                }
                else
                {
                    filePath = value;
                }
            }
        }

        public void Log()
        {
            using (var streamWriter = new StreamWriter(Path.Combine(FilePath, FileName), true))
            {
                streamWriter.Write($"{Environment.NewLine}{DateTime.Now:G}: {ErrorMessage}");
            }
        }
    }
}
