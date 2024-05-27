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
    /// Interaction logic for SearchMode.xaml
    /// </summary>
    public partial class SearchMode : Window
    {
        private ServiciuDictionar serviciuDictionar=new ServiciuDictionar();
        public SearchMode()
        {
            InitializeComponent();
        }
        private void OnCautaClick(object sender, RoutedEventArgs e)
        {
            var cuvantCautat = txtCautare.Text;
            var cuvant = serviciuDictionar.IncarcaCuvinte().FirstOrDefault(c => c.Nume.ToLower() == cuvantCautat.ToLower());

            if (cuvant != null)
            {
                lblDescriere.Text = $"Descriere: {cuvant.Descriere}";
                lblCategorie.Text = $"Categorie: {cuvant.Categorie}";
            }
            else
            {
                MessageBox.Show("Cuvantul nu a fost gasit.");
            }
        }

        private void txtCautare_TextChanged(object sender, TextChangedEventArgs e)
        {
            lstSugestii.Items.Clear(); // Golește lista de sugestii existente

            var textCautat = txtCautare.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(textCautat))
            {
                var sugestii = serviciuDictionar.IncarcaCuvinte()
                    .Where(c => c.Nume.ToLower().StartsWith(textCautat))
                    .Select(c => c.Nume)
                    .ToList();

                sugestii.ForEach(s => lstSugestii.Items.Add(s));
            }
        }

        private void AfiseazaDetaliiCuvant(Cuvant cuvant)
        {
            if (cuvant != null)
            {
                lblDescriere.Text = $"Descriere: {cuvant.Descriere}";
                lblCategorie.Text = $"Categorie: {cuvant.Categorie}";

                // Verifică dacă calea imaginii există și actualizează sursa controlului Image
                if (!string.IsNullOrWhiteSpace(cuvant.CaleImagine) && File.Exists(cuvant.CaleImagine))
                {
                    imgCuvant.Source = new BitmapImage(new Uri(cuvant.CaleImagine));
                }
                else
                {

                    //imgCuvant.Source = new BitmapImage(new Uri("Images/notfound.png", UriKind.Relative));
                    imgCuvant.Source = new BitmapImage(new Uri("pack://application:,,,/Images/notfound.png"));
                }
            }
        }
        private void lstSugestii_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = lstSugestii.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedItem))
            {
                var cuvant = serviciuDictionar.IncarcaCuvinte().FirstOrDefault(c => c.Nume == selectedItem);
                AfiseazaDetaliiCuvant(cuvant);
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
       

    }
}
