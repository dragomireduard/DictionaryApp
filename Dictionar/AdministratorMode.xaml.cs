using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Dictionar
{
    /// <summary>
    /// Interaction logic for AdministratorMode.xaml
    /// </summary>
    public partial class AdministratorMode : Window
    {
        private ServiciuDictionar serviciuDictionar = new ServiciuDictionar();
        private Cuvant cuvantInEditare = null;

        public AdministratorMode()
        {
            InitializeComponent();
            IncarcaCuvinteSiCategorii();
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNume.Text) || string.IsNullOrWhiteSpace(txtDescriere.Text))
            {
                MessageBox.Show("Numele și descrierea sunt necesare.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCaleImagine.Text) || !File.Exists(txtCaleImagine.Text))
            {
                txtCaleImagine.Text = "C:\\FACULTATE\\MVP\\Dictionar\\Dictionar\\Images\\notfound.png";

            }


            if (cuvantInEditare != null)
            {
                // Logica de actualizare a cuvântului existent
                string numeVechi = cuvantInEditare.Nume;
                cuvantInEditare.Nume = txtNume.Text;
                cuvantInEditare.Descriere = txtDescriere.Text;
                cuvantInEditare.Categorie = cmbCategorie.Text;
                cuvantInEditare.CaleImagine = txtCaleImagine.Text;

                serviciuDictionar.ModificaCuvant(numeVechi, cuvantInEditare);

                // Resetarea UI și ieșirea din modul de editare
                ResetUI();
            }
            else
            {
                var cuvant = new Cuvant
                {
                    Nume = txtNume.Text,
                    Descriere = txtDescriere.Text,
                    Categorie = cmbCategorie.Text,
                    CaleImagine = txtCaleImagine.Text
                };


                serviciuDictionar.AdaugaCuvant(cuvant);


                if (!cmbCategorie.Items.Contains(cmbCategorie.Text))
                {
                    cmbCategorie.Items.Add(cmbCategorie.Text);
                }


                IncarcaCuvinte();


                txtNume.Clear();
                txtDescriere.Clear();
                cmbCategorie.SelectedIndex = -1;
            }
        }

       

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            if (lvCuvinte.SelectedItem is Cuvant cuvantSelectat)
            {
                serviciuDictionar.StergeCuvant(cuvantSelectat.Nume); 
                IncarcaCuvinte();
            }
            ResetUI();
        }

        private void LvCuvinte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCuvinte.SelectedItem is Cuvant cuvantSelectat)
            {
                cuvantInEditare = cuvantSelectat;
                txtNume.Text = cuvantSelectat.Nume;
                txtDescriere.Text = cuvantSelectat.Descriere;
                cmbCategorie.Text = cuvantSelectat.Categorie;
                txtCaleImagine.Text = cuvantSelectat.CaleImagine;
                // Setează textul butonului de adăugare pentru a reflecta modul de editare
                btnAdauga.Content = "Salvează Modificările";
            }
        }
        private void IncarcaCuvinte()
        {
            lvCuvinte.ItemsSource = serviciuDictionar.IncarcaCuvinte();
        }
        private void TxtNume_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNume.Text == "Nume")
            {
                txtNume.Text = "";
                txtNume.Foreground = Brushes.Black;
            }
        }

        private void TxtNume_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNume.Text))
            {
                txtNume.Text = "Nume";
                txtNume.Foreground = Brushes.Gray;
            }
        }

        private void TxtDescriere_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDescriere.Text == "Descriere")
            {
                txtDescriere.Text = "";
                txtDescriere.Foreground = Brushes.Black;
            }
        }

        private void TxtDescriere_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescriere.Text))
            {
                txtDescriere.Text = "Descriere";
                txtDescriere.Foreground = Brushes.Gray;
            }
        }
        private void IncarcaCuvinteSiCategorii()
        {
            var cuvinte = serviciuDictionar.IncarcaCuvinte();
            lvCuvinte.ItemsSource = cuvinte;

            var categoriiUnice = cuvinte.Select(c => c.Categorie).Distinct().ToList();

            cmbCategorie.Items.Clear();
            foreach (var categorie in categoriiUnice)
            {
                if (categorie != "")
                {
                    cmbCategorie.Items.Add(categorie);
                }
            }

            // Încarcă categoriile adiționale din fișier, dacă există
            IncarcaCategorii();
        }

        private void IncarcaCategorii()
        {
            if (File.Exists("categorii.txt"))
            {
                var categorii = File.ReadAllLines("categorii.txt");
                foreach (var categorie in categorii)
                {
                    if (!cmbCategorie.Items.Contains(categorie)&& categorie!="")
                    {
                        cmbCategorie.Items.Add(categorie);
                    }
                }
            }
        }

        private void SalveazaCategorii()
        {
            var categorii = new HashSet<string>();
            foreach (var item in cmbCategorie.Items)
            {
                categorii.Add(item.ToString());
            }
            File.WriteAllLines("categorii.txt", categorii);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            SalveazaCategorii(); // Asigură-te că toate categoriile sunt salvate la închiderea ferestrei
        }

        private void ResetUI()
        {
            cuvantInEditare = null;
            txtNume.Clear();
            txtDescriere.Clear();
            cmbCategorie.SelectedIndex = -1;
            btnAdauga.Content = "Adaugă";
            IncarcaCuvinteSiCategorii(); // Reîncarcă lista pentru a reflecta modificările
        }

        private void BtnAlegeImagine_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagini (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|Toate fișierele (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
            {
                txtCaleImagine.Text = openFileDialog.FileName;
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
