using avtooglasi.Classes;
using avtooglasi.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using System.Windows;

namespace avtooglasi.View
{
    public partial class settings : Window
    {
        public settings(ViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            if (Properties.Settings.Default.Znamke == null)
            {
                Properties.Settings.Default.Znamke = new StringCollection();
                Properties.Settings.Default.Save();
            }
        }

        private void AddBrand(string brandName)
        {
            if (string.IsNullOrWhiteSpace(brandName))
            {
                MessageBox.Show("Ime znamke ne sme biti prazno.", "Napaka", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Properties.Settings.Default.Znamke.Contains(brandName))
            {
                MessageBox.Show("Znamka že obstaja.", "Napaka", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Properties.Settings.Default.Znamke.Add(brandName);
            Properties.Settings.Default.Save();

            MessageBox.Show($"Znamka '{brandName}' je bila uspešno dodana.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditBrand(string oldBrand, Znamka newBrand)
        {
            if (Properties.Settings.Default.Znamke.Contains(newBrand.ToString()))
            {
                MessageBox.Show("Znamka že obstaja.", "Napaka", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int index = Properties.Settings.Default.Znamke.IndexOf(oldBrand);
            if (index != -1)
            {
                Properties.Settings.Default.Znamke[index] = newBrand.ToString();
                Properties.Settings.Default.Save();

                var vm = DataContext as ViewModel;
                if (vm != null)
                {
                    foreach (var oglas in vm.AvtoOglasi)
                    {
                        if (oglas.Znamka.ToString() == oldBrand)
                            oglas.Znamka = newBrand;
                    }
                }

                MessageBox.Show($"Znamka '{oldBrand}' je bila uspešno posodobljena v '{newBrand}'.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void DeleteBrand(string brandName)
        {
            if (MessageBox.Show($"Ali res želite izbrisati znamko '{brandName}' in vse povezane oglase?",
                "Potrditev brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Properties.Settings.Default.Znamke.Remove(brandName);
                Properties.Settings.Default.Save();

                var vm = DataContext as ViewModel;
                if (vm != null)
                {
                    vm.AvtoOglasi = new ObservableCollection<Oglas>(vm.AvtoOglasi.Where(oglas => oglas.Znamka.ToString() != brandName));
                }

                MessageBox.Show($"Znamka '{brandName}' in povezani oglasi so bili uspešno izbrisani.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
