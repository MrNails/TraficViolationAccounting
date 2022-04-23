using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;

namespace AccountingOfTrafficViolation
{
    public interface ILogger
    {
        void Log(string errorMessage);
    }

    enum UserRole : byte
    {
        Debug = 0,
        Admin,
        User
    }

    public class FileLogger : ILogger
    {
        private string errorMessage;
        private string fileName;
        private string filePath;

        public FileLogger(string fileName) : this(fileName, string.Empty)
        { }
        public FileLogger(string fileName, string filePath)
        {
            FilePath = filePath;
            FileName = fileName;
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

        public void Log(string errorMessage)
        {
            using (var streamWriter = new StreamWriter(Path.Combine(FilePath, FileName), true))
            {
                streamWriter.Write($"{Environment.NewLine}{Environment.NewLine}{DateTime.Now:G}: {errorMessage}");
            }
        }
    }
}
