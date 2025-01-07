using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using avtooglasi.Model;
using Microsoft.Win32;

namespace avtooglasi.Classes
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Oglas> _avtoOglasi = new ObservableCollection<Oglas>();
        public event PropertyChangedEventHandler? PropertyChanged;
        private BitmapImage? bitMapImage;
        private string _newZnamkaName;
        public string NewZnamkaName
        {
            get => _newZnamkaName;
            set
            {
                if (CheckForDuplicateZnamka(value))
                {
                    _newZnamkaName = value;
                    OnPropertyChanged(nameof(NewZnamkaName));
                    DodajZnamko.RaiseCanExecuteChanged();
                }
                else
                {
                    MessageBox.Show("Ta znamka že obstja. Ponovite vnos in ne duplicirajte.");
                }
            }
        }

        private ObservableCollection<string> _availableZnamke;
        public ObservableCollection<string> AvailableZnamke
        {
            get => _availableZnamke;
            set
            {
                _availableZnamke = value;
                OnPropertyChanged(nameof(AvailableZnamke));
            }
        }

        private string _selectedZnamkaForEdit;
        public string SelectedZnamkaForEdit
        {
            get => _selectedZnamkaForEdit;
            set
            {
                _selectedZnamkaForEdit = value;
                OnPropertyChanged(nameof(SelectedZnamkaForEdit));
                UrediZnamko.RaiseCanExecuteChanged();
                OdstraniZnamko.RaiseCanExecuteChanged();
            }
        }

        private string _editedZnamkaName;
        public string EditedZnamkaName
        {
            get => _editedZnamkaName;
            set
            {
                if (CheckForDuplicateZnamka(value))
                {
                    _editedZnamkaName = value;
                    OnPropertyChanged(nameof(EditedZnamkaName));
                    UrediZnamko.RaiseCanExecuteChanged();
                }
                else
                {
                    MessageBox.Show("Ta znamka že obstja. Ponovno uredite in ne duplicirajte.");
                }
            }
        }

        public Command OdstraniCommand { get; }
        public Command ShraniXmlCommand { get; }
        public Command NaloziXmlCommand { get; }
        public Command AddNewOglas { get; }
        public Command AddImage { get; }
        public Command UrediCommand { get; }
        public Command DodajZnamko { get; }
        public Command UrediZnamko { get; }
        public Command OdstraniZnamko { get; }

        public ObservableCollection<Oglas> AvtoOglasi
        {
            get { return _avtoOglasi; }
            set
            {
                _avtoOglasi = value;
                OnPropertyChanged(nameof(AvtoOglasi));
            }
        }

        private Oglas? _selectedOglas;
        public Oglas? SelectedOglas
        {
            get { return _selectedOglas; }
            set
            {
                if (_selectedOglas != value)
                {
                    _selectedOglas = value;
                    OnPropertyChanged(nameof(SelectedOglas));
                    OdstraniCommand.RaiseCanExecuteChanged();
                    UrediCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private Oglas _newOglas = new Oglas();
        public Oglas NewOglas
        {
            get { return _newOglas; }
            set
            {
                if (_newOglas != value)
                {
                    _newOglas = value;
                    OnPropertyChanged(nameof(NewOglas));
                }
            }
        }

        private TipPonudbe _selectedTipPonudbe;
        public TipPonudbe SelectedTipPonudbe
        {
            get { return _selectedTipPonudbe; }
            set
            {
                if (_selectedTipPonudbe != value)
                {
                    _selectedTipPonudbe = value;
                    OnPropertyChanged(nameof(SelectedTipPonudbe));
                }
            }
        }

        private KaroserijskaIzvedba _selectedKaroserijskaIzvedba;
        public KaroserijskaIzvedba SelectedKaroserijskaIzvedba
        {
            get { return _selectedKaroserijskaIzvedba; }
            set
            {
                if (_selectedKaroserijskaIzvedba != value)
                {
                    _selectedKaroserijskaIzvedba = value;
                    OnPropertyChanged(nameof(SelectedKaroserijskaIzvedba));
                }
            }
        }

        private Starost _selectedStarost;
        public Starost SelectedStarost
        {
            get { return _selectedStarost; }
            set
            {
                if (_selectedStarost != value)
                {
                    _selectedStarost = value;
                    OnPropertyChanged(nameof(SelectedStarost));
                }
            }
        }

        private Znamka _selectedZnamka;
        public Znamka SelectedZnamka
        {
            get { return _selectedZnamka; }
            set
            {
                if (_selectedZnamka != value)
                {
                    _selectedZnamka = value;
                    OnPropertyChanged(nameof(SelectedZnamka));
                }
            }
        }

        public IEnumerable<TipPonudbe> TipPonudbeValues => Enum.GetValues(typeof(TipPonudbe)).Cast<TipPonudbe>();
        public IEnumerable<Starost> StarostValues => Enum.GetValues(typeof(Starost)).Cast<Starost>();
        public IEnumerable<KaroserijskaIzvedba> KaroserijskaIzvedbaValues => Enum.GetValues(typeof(KaroserijskaIzvedba)).Cast<KaroserijskaIzvedba>();
        public IEnumerable<Znamka> ZnamkaValues => Enum.GetValues(typeof(Znamka)).Cast<Znamka>();

        public ViewModel()
        {
            _availableZnamke = new ObservableCollection<string>();

            OdstraniCommand = new Command(obj => RemoveOglas(), obj => CanRemoveOrEditOglas());
            ShraniXmlCommand = new Command(obj => SaveXmlDocument());
            NaloziXmlCommand = new Command(obj => LoadXmlDocument());
            AddNewOglas = new Command(obj => DodajOglas());
            AddImage = new Command(obj => NaloziSliko());
            DodajZnamko = new Command(obj => DodajZnamkoExecute(), obj => CanDodajZnamko());
            UrediZnamko = new Command(obj => UrediZnamkoExecute(), obj => CanUrediZnamko());
            OdstraniZnamko = new Command(obj => OdstraniZnamkoExecute(), obj => CanOdstraniZnamko());

            try
            {
                _avtoOglasi.Add(new Oglas("RS7", "Dobro ohranjen nov avto!", 67500, "Bojan Novak", TipPonudbe.Prodaja, Starost.Novo, KaroserijskaIzvedba.Karavan, Znamka.Vse_znamke, "/Assets/Images/rs7audi.jpg"));
                _avtoOglasi.Add(new Oglas("C3e", "Lep avto, ki dobro pelje!", 25000, "Maja Kovač", TipPonudbe.Prodaja, Starost.Novo, KaroserijskaIzvedba.Pick_up, Znamka.Vse_znamke, "/Assets/Images/avto_c3.jpg"));
                _avtoOglasi.Add(new Oglas("C3e", "Lep avto, ki dobro pelje!", 25000, "Maja Kovač", TipPonudbe.Prodaja, Starost.Novo, KaroserijskaIzvedba.Pick_up, Znamka.Vse_znamke, "/Assets/Images/avto_ford_fiesta.jpg"));
                _avtoOglasi.Add(new Oglas("Civic", "Lep avto, ki dobro pelje!", 25000, "Maja Kovač", TipPonudbe.Prodaja, Starost.Novo, KaroserijskaIzvedba.Pick_up, Znamka.Vse_znamke, "/Assets/Images/avto_honda_civic.jpg"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Prišlo je do napake: {ex.Message}");
            }

            LoadZnamkaFromSettings();
        }

        private bool CanDodajZnamko()
        {
            return !string.IsNullOrWhiteSpace(NewZnamkaName) &&
                   !AvailableZnamke.Contains(NewZnamkaName);
        }

        private void DodajZnamkoExecute()
        {
            if (CanDodajZnamko())
            {
                try
                {
                    Properties.Settings.Default.Znamke.Add(NewZnamkaName);
                    Properties.Settings.Default.Save();

                    AvailableZnamke.Add(NewZnamkaName);
                    NewZnamkaName = string.Empty;

                    MessageBox.Show("Znamka uspešno dodana!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Napaka pri dodajanju znamke: {ex.Message}");
                }
            }
        }

        private bool CanUrediZnamko()
        {
            return !string.IsNullOrWhiteSpace(EditedZnamkaName) &&
                   !string.IsNullOrWhiteSpace(SelectedZnamkaForEdit) &&
                   !AvailableZnamke.Contains(EditedZnamkaName);
        }

        private void UrediZnamkoExecute()
        {
            if (CanUrediZnamko())
            {
                try
                {
                    int index = Properties.Settings.Default.Znamke.IndexOf(SelectedZnamkaForEdit);
                    if (index != -1)
                    {
                        Properties.Settings.Default.Znamke[index] = EditedZnamkaName;
                        Properties.Settings.Default.Save();

                        index = AvailableZnamke.IndexOf(SelectedZnamkaForEdit);
                        if (index != -1)
                        {
                            AvailableZnamke[index] = EditedZnamkaName;
                        }

                        EditedZnamkaName = string.Empty;
                        SelectedZnamkaForEdit = null;

                        MessageBox.Show("Znamka uspešno posodobljena!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Napaka pri urejanju znamke: {ex.Message}");
                }
            }
        }

        private bool CanOdstraniZnamko()
        {
            return !string.IsNullOrWhiteSpace(SelectedZnamkaForEdit);
        }

        private void OdstraniZnamkoExecute()
        {
            if (CanOdstraniZnamko())
            {
                try
                {
                    if (MessageBox.Show($"Ali res želite izbrisati znamko '{SelectedZnamkaForEdit}'?",
                        "Potrditev brisanja", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        Properties.Settings.Default.Znamke.Remove(SelectedZnamkaForEdit);
                        Properties.Settings.Default.Save();

                        AvailableZnamke.Remove(SelectedZnamkaForEdit);

                        var oglisiToRemove = AvtoOglasi.Where(o => o.Znamka.ToString() == SelectedZnamkaForEdit).ToList();
                        foreach (var oglas in oglisiToRemove)
                        {
                            AvtoOglasi.Remove(oglas);
                        }

                        SelectedZnamkaForEdit = null;

                        MessageBox.Show("Znamka uspešno izbrisana!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Napaka pri brisanju znamke: {ex.Message}");
                }
            }
        }

        private void RemoveOglas()
        {
            if (SelectedOglas != null)
            {
                _avtoOglasi.Remove(SelectedOglas);
                SelectedOglas = null;
            }
        }

        private bool CanRemoveOrEditOglas()
        {
            return SelectedOglas != null;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DodajOglas()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewOglas.Naziv) ||
                    string.IsNullOrWhiteSpace(NewOglas.Opis) ||
                    NewOglas.Cena <= 0 ||
                    string.IsNullOrWhiteSpace(NewOglas.Prodajalec) ||
                    NewOglas?.Ponudba == null ||
                    NewOglas?.AvtoStarost == null ||
                    NewOglas?.KaroserijskaIzvedba == null ||
                    NewOglas?.Znamka == null)
                {
                    MessageBox.Show("Prosimo, izpolnite vsa polja pravilno!");
                    return;
                }

                _avtoOglasi.Add(NewOglas);
                MessageBox.Show($"Oglas: {NewOglas.Naziv} uspešno dodan!");

                NewOglas = new Oglas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Napaka: {ex.Message}");
            }
        }


        public void NaloziSliko()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                NewOglas.ThumbnailLink = openFileDialog.FileName;
                bitMapImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }


        public void UrediOglas()
        {
            SelectedOglas.Naziv = "Urejen oglas";
        }

        public void LoadXmlDocument()
        {
            var dialog = new OpenFileDialog();
            dialog.FileName = "Document";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "XML documents (.xml)|*.xml";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Oglas>));

                    using (StreamReader reader = new StreamReader(dialog.FileName))
                    {
                        var loadedOglasiList = (ObservableCollection<Oglas>)xmlSerializer.Deserialize(reader);

                        if (loadedOglasiList != null)
                        {
                            AvtoOglasi.Clear();

                            foreach (var oglas in loadedOglasiList)
                            {
                                AvtoOglasi.Add(oglas);
                            }

                            MessageBox.Show("Podatki so bili uspešno naloženi iz XML datoteke!");
                        }
                        else
                        {
                            MessageBox.Show("Napaka pri nalaganju XML podatkov iz datoteke.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Napaka pri nalaganju XML datoteke: {ex.Message}");
                }
            }
        }

        public void SaveXmlDocument()
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Document";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "XML documents (.xml)|*.xml";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Oglas>));

                    using (StreamWriter writer = new StreamWriter(dialog.FileName))
                    {
                        xmlSerializer.Serialize(writer, AvtoOglasi);
                    }

                    MessageBox.Show("Podatki so shranjeni v XML datoteko.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Napaka pri shranjevanju XML: " + ex.Message);
                }
            }
        }

        private void LoadZnamkaFromSettings()
        {
            try
            {
                if (Properties.Settings.Default.Znamke != null)
                {
                    AvailableZnamke.Clear();
                    foreach (string znamka in Properties.Settings.Default.Znamke)
                    {
                        AvailableZnamke.Add(znamka);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Napaka pri nalaganju znamk: {ex.Message}");
            }
        }

        private bool CheckForDuplicateZnamka(string znamkaName)
        {
            foreach (var znamka in _availableZnamke)
            {
                if (znamkaName == znamka)
                {
                    return false;
                }
            }
            return true;
        }
    }
}