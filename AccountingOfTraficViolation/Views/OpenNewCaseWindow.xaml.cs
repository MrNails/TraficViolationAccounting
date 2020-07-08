using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для OpenNewCaseWindow.xaml
    /// </summary>
    public partial class OpenNewCaseWindow : Window
    {
        public OpenNewCaseWindow()
        {
            InitializeComponent();
        }

        private void GeneralInfoClick(object sender, RoutedEventArgs e)
        {
            //GeneralInfoAddWindow generalInfoAddWindow = new GeneralInfoAddWindow(generalInfo);

            //if (generalInfoAddWindow.ShowDialog() == true)
            //{
            //    generalInfo = generalInfoAddWindow.GeneralInfo;
            //    GeneralInfoProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
            //}
        }
        private void AccidentPlaceClick(object sender, RoutedEventArgs e)
        {
            //AccidentPlaceWinow accidentPlaceWinow = new AccidentPlaceWinow();
            //if (accidentPlaceWinow.ShowDialog() == true)
            //{
            //    AccidentPlaceProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
            //}
        }
        private void RoadConditionClick(object sender, RoutedEventArgs e)
        {
            RoadConditionProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
        }
        private void ParticipanInfoClick(object sender, RoutedEventArgs e)
        {
            ParticipanInfoProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
        }
        private void VehicleClick(object sender, RoutedEventArgs e)
        {
            VehicleProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
        }
        private void VictimClick(object sender, RoutedEventArgs e)
        {
            VictimProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
        }
    }
}
