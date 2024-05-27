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
using System.Windows.Shapes;

namespace Dictionar
{
    /// <summary>
    /// Interaction logic for LoginAdministrator.xaml
    /// </summary>
    public partial class LoginAdministrator : Window
    {
        public LoginAdministrator()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password; // PasswordBox folosește proprietatea .Password

            if (IsAuthenticated(username, password))
            {
                MessageBox.Show("Autentificare reușită!");
                AdministratorMode administratorWindow = new AdministratorMode();
                administratorWindow.Show(); // Afișează noua fereastră
                this.Close();
                // Navighează către o altă fereastră dacă este necesar
            }
            else
            {
                MessageBox.Show("Autentificare eșuată. Verificați numele de utilizator și parola.");
            }
        }

        private bool IsAuthenticated(string username, string password)
        {
            try
            {
                var lines = File.ReadAllLines("C:\\FACULTATE\\MVP\\Dictionar\\Dictionar\\users2.txt"); 
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && parts[0].Trim() == username && parts[1].Trim() == password)
                    {
                        return true;
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"A apărut o eroare la citirea fișierului: {ex.Message}");
            }
            return false;
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); 
            this.Close();
        }
    }
}
