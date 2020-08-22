using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AccountingOfTraficViolation
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex currentAppInstance;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //bool createdNew;
            //currentAppInstance = new Mutex(true, "Accounting of trafic violation", out createdNew);

            //if (!createdNew)
            //{
            //    this.Shutdown();
            //}
        }
    }
}
