using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionar
{
    
    class ServiciuDictionar
    {
        private string caleFisier = "dictionar.txt";

        public ServiciuDictionar()
        {
            if (!File.Exists(caleFisier))
            {
                File.Create(caleFisier).Close();
            }
        }

        public void AdaugaCuvant(Cuvant cuvant)
        {
            var linie = $"{cuvant.Nume},{cuvant.Descriere},{cuvant.Categorie},{cuvant.CaleImagine}";
            File.AppendAllText(caleFisier, linie + Environment.NewLine);
        }

        public void ModificaCuvant(string nume, Cuvant cuvantNou)
        {
            var cuvinte = IncarcaCuvinte();
            var index = cuvinte.FindIndex(c => c.Nume == nume);
            if (index != -1)
            {
                cuvinte[index] = cuvantNou;
                RescrieFisier(cuvinte);
            }
        }

        public void StergeCuvant(string nume)
        {
            var cuvinte = IncarcaCuvinte();
            cuvinte.RemoveAll(c => c.Nume == nume);
            RescrieFisier(cuvinte);
        }

        public List<Cuvant> IncarcaCuvinte()
        {
            var linii = File.ReadAllLines(caleFisier);
            var cuvinte = linii.Select(linia =>
            {
                var parti = linia.Split(',');
                return new Cuvant
                {
                    Nume = parti[0],
                    Descriere = parti[1],
                    Categorie = parti[2],
                    CaleImagine = parti.Length > 3 ? parti[3] : "C:\\FACULTATE\\MVP\\Dictionar\\Dictionar\\Images\\notfound.png"
                };
            }).ToList();
            return cuvinte;
        }

        private void RescrieFisier(List<Cuvant> cuvinte)
        {
            File.WriteAllLines(caleFisier, cuvinte.Select(c => $"{c.Nume},{c.Descriere},{c.Categorie},{c.CaleImagine}"));
        }
    }
}
