using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AccountingOfTraficViolation.Models;

namespace AccountingOfTraficViolation
{
    public class CaseVM
    {
        private TVAContext _TVAContext;
        private RelayCommand addCommand;
        private RelayCommand updateCommand;
        private RelayCommand removeCommand;

        public CaseVM()
        {
            _TVAContext = new TVAContext();

            addCommand = new RelayCommand(_case =>
            {

            });
            updateCommand = new RelayCommand(_case =>
            {
 
            });
            removeCommand = new RelayCommand(_case =>
            {

            });
        }

        public RelayCommand AddCommand
        {
            get { return addCommand; }
        }
        public RelayCommand UpdateCommand
        {
            get { return updateCommand; }
        }
        public RelayCommand RemoveCommand
        {
            get { return removeCommand; }
        }
    }
}
