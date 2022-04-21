using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using AccountingOfTrafficViolation.Models;
using Microsoft.Office.Interop.Word;

namespace AccountingOfTrafficViolation.Services
{
    public enum DocumentSaveType : byte
    {
        DOC = 0,
        DOCX,
        PDF
    }

    public class WordSaver : IDisposable
    {
        private bool documentIsClose;
        private string path;

        private Application application;
        private Document document;

        private bool disposed;

        public WordSaver(string path)
        {
            disposed = false;

            application = new Application();

            this.path = path;
        }

        public Document Document
        {
            get { return document; }
            private set
            {
                document = value;
            }
        }

        public bool Replace(string textToReplace, string replaceText, WdReplace replaceMethod)
        {
            Find find = Document.Content.Find;
            bool res = find.Execute(textToReplace, Missing.Value, Missing.Value, 
                                     Missing.Value, Missing.Value, Missing.Value, 
                                     Missing.Value,  WdFindWrap.wdFindContinue, Missing.Value, 
                                     replaceText, replaceMethod, Missing.Value, Missing.Value, 
                                     Missing.Value, Missing.Value);

            Marshal.FinalReleaseComObject(find);

            return res;
        }
        public void Replace<T>(T _object, WdReplace replaceMethod)
        {
            Type specifiedType = typeof(T);

            foreach (var propertyInfo in specifiedType.GetProperties())
            {
                if (!propertyInfo.GetMethod.IsVirtual && !propertyInfo.GetMethod.IsAbstract &&
                    propertyInfo.GetMethod.IsPublic && propertyInfo.CanRead)
                {
                    string name = propertyInfo.Name;
                    string value = propertyInfo.GetValue(_object).ToString();

                    if (name.Length > 255 || value.Length > 255)
                    {
                        continue;
                    }

                    Replace(name, value, replaceMethod);
                }
            }
        }
        public void Replace<T>(T _object, WdReplace replaceMethod, 
                               Func<string, string> propertyNameAction)
        {
            if (propertyNameAction == null)
            {
                throw new ArgumentNullException("propertyNameAction");
            }

            Type specifiedType = typeof(T);

            foreach (var propertyInfo in specifiedType.GetProperties())
            {
                if (!propertyInfo.GetMethod.IsVirtual && !propertyInfo.GetMethod.IsAbstract &&
                    propertyInfo.GetMethod.IsPublic && propertyInfo.CanRead)
                {
                    string name = propertyNameAction(propertyInfo.Name);
                    string value = propertyInfo.GetValue(_object).ToString();

                    if (name.Length > 255 || value.Length > 255)
                    {
                        continue;
                    }

                    Replace(name, value, replaceMethod);
                }
            }
        }
        public void Replace<T>(T _object, WdReplace replaceMethod, 
                               Func<object, string> propertyValueAction)
        {
            if (propertyValueAction == null)
            {
                throw new ArgumentNullException("propertyValueAction");
            }

            Type specifiedType = typeof(T);

            foreach (var propertyInfo in specifiedType.GetProperties())
            {
                if (!propertyInfo.GetMethod.IsVirtual && !propertyInfo.GetMethod.IsAbstract &&
                    propertyInfo.GetMethod.IsPublic && propertyInfo.CanRead)
                {
                    string name = propertyInfo.Name;
                    string value = propertyValueAction(propertyInfo.GetValue(_object));

                    if (name.Length > 255 || value.Length > 255)
                    {
                        continue;
                    }

                    Replace(name, value, replaceMethod);
                }
            }
        }
        public void Replace<T>(T _object, WdReplace replaceMethod,
                       Func<string, object, string> propertyValueWithNameAction)
        {
            if (propertyValueWithNameAction == null)
            {
                throw new ArgumentNullException("propertyValueAction");
            }

            Type specifiedType = typeof(T);

            foreach (var propertyInfo in specifiedType.GetProperties())
            {
                if (!propertyInfo.GetMethod.IsVirtual && !propertyInfo.GetMethod.IsAbstract &&
                    propertyInfo.GetMethod.IsPublic && propertyInfo.CanRead)
                {
                    string name = propertyInfo.Name;
                    string value = propertyValueWithNameAction(propertyInfo.Name, propertyInfo.GetValue(_object));

                    if (name.Length > 255 || value.Length > 255)
                    {
                        continue;
                    }

                    Replace(name, value, replaceMethod);
                }
            }
        }
        public void Replace<T>(T _object, WdReplace replaceMethod, 
                               Func<string, string> propertyNameAction, 
                               Func<object, string> propertyValueAction)
        {
            if (propertyNameAction == null)
            {
                throw new ArgumentNullException("propertyNameAction");
            }

            if (propertyValueAction == null)
            {
                throw new ArgumentNullException("propertyValueAction");
            }

            Type specifiedType = typeof(T);

            foreach (var propertyInfo in specifiedType.GetProperties())
            {
                if (!propertyInfo.GetMethod.IsVirtual && !propertyInfo.GetMethod.IsAbstract &&
                    propertyInfo.GetMethod.IsPublic && propertyInfo.CanRead)
                {
                    string name = propertyNameAction(propertyInfo.Name);
                    string value = propertyValueAction(propertyInfo.GetValue(_object));

                    if (name.Length > 255 || value.Length > 255)
                    {
                        continue;
                    }

                    Replace(name, value, replaceMethod);
                }
            }
        }
        public void Replace<T>(T _object, WdReplace replaceMethod,
                       Func<string, string> propertyNameAction,
                       Func<string, object, string> propertyValueWithNameAction)
        {
            if (propertyNameAction == null)
            {
                throw new ArgumentNullException("propertyNameAction");
            }

            if (propertyValueWithNameAction == null)
            {
                throw new ArgumentNullException("propertyValueWithNameAction");
            }

            if (_object == null)
            {
                throw new ArgumentNullException("_object");
            }

            Type specifiedType = typeof(T);

            foreach (var propertyInfo in specifiedType.GetProperties())
            {
                if (!propertyInfo.GetMethod.IsVirtual && !propertyInfo.GetMethod.IsAbstract &&
                    propertyInfo.GetMethod.IsPublic && propertyInfo.CanRead)
                {
                    string name = propertyNameAction(propertyInfo.Name);
                    string value = propertyValueWithNameAction(propertyInfo.Name, propertyInfo.GetValue(_object));

                    if (name.Length > 255 || value.Length > 255)
                    {
                        continue;
                    }

                    Replace(name, value, replaceMethod);
                }
            }
        }

        public void OpenDocument()
        {
            if (!FileIsOpen(path))
            {
                Document = application.Documents.Open(path, Missing.Value, false, Missing.Value, Missing.Value,
                                                      Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                      Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                documentIsClose = false;
            }
            else
            {
                throw new Exception("Текущий документ используеться другим пользователем.");
            }

        }

        public void ReloadDocument()
        {
            if (!documentIsClose && Document != null)
            {
                Document.Reload();
            }
        }

        public void SaveDocument()
        {
            if (document != null && !documentIsClose)
            {
                Document.Save();
            }
        }
        public void SaveDocumentAs(string path, DocumentSaveType documentSaveType)
        {
            string docNameWithoutExtension = null;

            if (path.LastIndexOf('.') != -1)
            {
                docNameWithoutExtension = path.Remove(path.LastIndexOf('.'));
            }
            else
            {
                docNameWithoutExtension = path;
            }

            switch (documentSaveType)
            {
                case DocumentSaveType.DOC:
                    Document.SaveAs2(docNameWithoutExtension, WdSaveFormat.wdFormatDocument);
                    break;
                case DocumentSaveType.DOCX:
                    Document.SaveAs2(docNameWithoutExtension, WdSaveFormat.wdFormatXMLDocument);
                    break;
                case DocumentSaveType.PDF:
                    Document.SaveAs2(docNameWithoutExtension, WdSaveFormat.wdFormatPDF);
                    break;
                default:
                    break;
            }
        }

        public void CloseDocument()
        {
            if (!documentIsClose && Document != null)
            {
                documentIsClose = true;

                Document.Close(SaveChanges: WdSaveOptions.wdDoNotSaveChanges, Missing.Value, Missing.Value);
            }
        }

        private bool FileIsOpen(string filePath)
        {
            System.IO.FileStream fileStream = null;
            bool isNotSharedException = true;

            try
            {
                fileStream = System.IO.File.Open(filePath,
                System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                isNotSharedException = false;
                throw;
            }
            catch (System.IO.FileNotFoundException)
            {
                isNotSharedException = false;
                throw;
            }
            catch (System.IO.IOException) when (isNotSharedException)
            {
                return true;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }

            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    CloseDocument();
                    application.Quit(SaveChanges: WdSaveOptions.wdDoNotSaveChanges, Missing.Value, Missing.Value);
                }

                if (document != null && application != null)
                {
                    Marshal.FinalReleaseComObject(application);
                    Marshal.FinalReleaseComObject(Document);
                }

                disposed = true;
            }
        }

        ~WordSaver()
        {
            Dispose();
        }
    }
}
