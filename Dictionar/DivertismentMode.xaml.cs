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

namespace Dictionar
{
    /// <summary>
    /// Interaction logic for DivertismentMode.xaml
    /// </summary>
    public partial class DivertismentMode : Window
    {
        private List<Cuvant> cuvinte;
        private int indexCurent = 0;
        private int raspunsuriCorecte = 0;
        private Random random = new Random();
        public DivertismentMode()
        {
            InitializeComponent();
            IncarcaCuvinte();
            AfiseazaCuvant();
        }
        private void IncarcaCuvinte()
        {
            // Presupunem că metoda `IncarcaCuvinte` din `ServiciuDictionar` îți încarcă toate cuvintele disponibile
            var serviciu = new ServiciuDictionar();
            cuvinte = serviciu.IncarcaCuvinte();

            // Amestecă cuvintele pentru a avea o ordine aleatorie
            cuvinte = cuvinte.OrderBy(x => random.Next()).Take(5).ToList();
        }

        private void AfiseazaCuvant()
        {
            if (indexCurent < cuvinte.Count)
            {
                var cuvantCurent = cuvinte[indexCurent];
                txtIntrebareCurenta.Text = $"Întrebarea {indexCurent + 1} din {cuvinte.Count}";

                // Decide dacă arată descrierea sau imaginea
                if (string.IsNullOrEmpty(cuvantCurent.CaleImagine) || !System.IO.File.Exists(cuvantCurent.CaleImagine) ||cuvantCurent.CaleImagine== "C:\\FACULTATE\\MVP\\Dictionar\\Dictionar\\Images\\notfound.png")
                {
                    IndiciuContent.Content = cuvantCurent.Descriere; // Afișează descrierea
                }
                else
                {
                    var image = new System.Windows.Controls.Image
                    {
                        Source = new BitmapImage(new Uri(cuvantCurent.CaleImagine)),
                        Stretch = System.Windows.Media.Stretch.Uniform
                    };
                    IndiciuContent.Content = image; // Afișează imaginea
                }

                // Actualizează starea butoanelor
                btnPrevious.IsEnabled = indexCurent > 0;
                btnNext.Visibility = indexCurent < cuvinte.Count - 1 ? Visibility.Visible : Visibility.Collapsed;
                btnFinish.Visibility = indexCurent == cuvinte.Count - 1 ? Visibility.Visible : Visibility.Collapsed;
            }
            btnVerify.IsEnabled = true;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            //VerificaRaspuns();
            indexCurent++;
            AfiseazaCuvant();
            txtRaspuns.Clear();
           
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (indexCurent > 0)
            {
                indexCurent--;
                AfiseazaCuvant();
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            //VerificaRaspuns();
            MessageBox.Show($"Jocul s-a terminat! Ai ghicit corect {raspunsuriCorecte} cuvinte din 5.");
            ResetareJoc();
        }

        private void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            var raspuns = txtRaspuns.Text.Trim().ToLower();
            var cuvantCurent = cuvinte[indexCurent];

            if (raspuns == cuvantCurent.Nume.ToLower())
            {
                MessageBox.Show("Răspuns corect!", "Verificare", MessageBoxButton.OK, MessageBoxImage.Information);
                raspunsuriCorecte++;
            }
            else
            {
                MessageBox.Show($"Răspuns greșit! Cuvântul corect era: {cuvantCurent.Nume}", "Verificare", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Opțional: Dezactivează butonul de verificare după utilizare pentru a preveni răspunsuri multiple
            btnVerify.IsEnabled = false;
        }
        private void ResetareJoc()
        {
            indexCurent = 0;
            raspunsuriCorecte = 0;
            IncarcaCuvinte();
            AfiseazaCuvant();
        }

    }
}
