using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dictionar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonAdministrativ_Click(object sender, RoutedEventArgs e)
        {
            LoginAdministrator administratorLogin = new LoginAdministrator();
            administratorLogin.Show(); // Afișează noua fereastră
            this.Close();
        }

        private void ButtonCautare_Click(object sender, RoutedEventArgs e)
        {
            SearchMode cautareCuvantWindow = new SearchMode();
            cautareCuvantWindow.Show();
            this.Close();
        }

        private void ButtonDivertisment_Click(object sender, RoutedEventArgs e)
        {
            DivertismentMode divertismentWindow = new DivertismentMode();
            divertismentWindow.Show();
            this.Close();
        }

      
    }
}


