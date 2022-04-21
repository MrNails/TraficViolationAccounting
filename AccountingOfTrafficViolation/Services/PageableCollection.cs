using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingOfTraficViolation.Services
{
    public class PageableCollection<T> : INotifyPropertyChanged
    {
        private IEnumerable<T> collection;
        private int currentPage;
        private int maxRecordCount;
        private int collLength;
        private int maxPageCount;

        public event PropertyChangedEventHandler PropertyChanged;

        public PageableCollection(IEnumerable<T> collection, int recordCount)
        {
            if (recordCount < 1)
            {
                throw new ArgumentOutOfRangeException("recordCount", "Количество записей на странице не может быть меньше 1.");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            this.collection = collection;
            collLength = collection.Count();

            CurrentPage = 0;
            maxRecordCount = recordCount;

            if (collLength % maxRecordCount == 0)
            {
                maxPageCount = collLength / maxRecordCount;
            }
            else
            {
                maxPageCount = collLength / maxRecordCount + 1;
            }
        }

        public IEnumerable<T> Page 
        { 
            get { return collection.TakeWhile((o, i) => currentPage * maxRecordCount > i && (currentPage + 1) * maxRecordCount <= i); } 
        }

        public int CurrentPage
        {
            get { return currentPage + 1; }
            private set 
            {
                if (value < 0)
                {
                    currentPage = 0;
                }
                else if (value >= maxPageCount)
                {
                    currentPage = maxPageCount - 1;
                }
                else
                {
                    currentPage = value;
                }

                OnPropertyChanged("Page");
            }
        }

        public void FirstPage()
        {
            CurrentPage = 0;
        }
        public void NextPage()
        {
            CurrentPage++;
        }
        public void PreviousPage()
        {
            CurrentPage--;
        }
        public void LastPage()
        {
            CurrentPage = maxPageCount;
        }

        public void SetCollection(IEnumerable<T> collection)
        {
            this.collection = collection;
        }

        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
