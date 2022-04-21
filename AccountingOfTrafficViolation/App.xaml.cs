using System;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading;
using System.Windows;
using AccountingOfTrafficViolation.Services;

namespace AccountingOfTrafficViolation
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex currentAppInstance;
        private ILogger logger;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool createdNew;
            currentAppInstance = new Mutex(true, "Accounting of trafic violation", out createdNew);

            if (!createdNew)
                this.Shutdown();

            logger = new FileLogger("Errors.txt");

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            LoadSettings();
        }

        private void LoadSettings()
        {
            var fPath = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
            if (!File.Exists(fPath))
            {
                MessageBox.Show("Отсутствует файл appsettings.json", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                
                Shutdown();
                
                return;
            }
            
            var jObj = JsonObject.Parse(File.ReadAllText(fPath));

            var connectionStrings = jObj["ConnectionStrings"].AsObject();

            foreach (var connectionString in connectionStrings)
                GlobalSettings.ConnectionStrings[connectionString.Key] = connectionString.Value.GetValue<string>();
            
        }
        
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Возникла ошибка, смотри подробности в файле Errors.txt в папке приложения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            logger.ErrorMessage = $"Сообщение: {e.Exception.Message}\nВнутренняя ошибка: " +
                                  $"{(e.Exception.InnerException != null ? e.Exception.InnerException.Message : string.Empty)}" +
                                  $"\nStackTrace: {e.Exception.StackTrace}";
            logger.Log();

            e.Handled = true;
        }
    }
}
