using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using avtooglasi.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace avtooglasi.View.UserControls
{
    public partial class SearchFilterControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<string>? SearchRequested;
        public event EventHandler<FilterEventArgs>? FiltersChanged;
        StringCollection znamke = Properties.Settings.Default.Znamke;

        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private ObservableCollection<string> _tipPonudbeValues;
        public ObservableCollection<string> TipPonudbeValues
        {
            get { return _tipPonudbeValues; }
            set
            {
                _tipPonudbeValues = value;
                OnPropertyChanged(nameof(TipPonudbeValues));
            }
        }

        private string _selectedTipPonudbe;
        public string SelectedTipPonudbe
        {
            get { return _selectedTipPonudbe; }
            set
            {
                _selectedTipPonudbe = value;
                OnPropertyChanged(nameof(SelectedTipPonudbe));
            }
        }

        private ObservableCollection<string> _starostValues;
        public ObservableCollection<string> StarostValues
        {
            get { return _starostValues; }
            set
            {
                _starostValues = value;
                OnPropertyChanged(nameof(StarostValues));
            }
        }

        private string _selectedStarost;
        public string SelectedStarost
        {
            get { return _selectedStarost; }
            set
            {
                _selectedStarost = value;
                OnPropertyChanged(nameof(SelectedStarost));
            }
        }

        private ObservableCollection<string> _availableZnamke;
        public ObservableCollection<string> AvailableZnamke
        {
            get { return _availableZnamke; }
            set
            {
                _availableZnamke = value;
                OnPropertyChanged(nameof(AvailableZnamke));
            }
        }

        private string _selectedZnamka;
        public string SelectedZnamka
        {
            get { return _selectedZnamka; }
            set
            {
                _selectedZnamka = value;
                OnPropertyChanged(nameof(SelectedZnamka));
            }
        }

        private ObservableCollection<string> _karoserijskaIzvedbaValues;
        public ObservableCollection<string> KaroserijskaIzvedbaValues
        {
            get { return _karoserijskaIzvedbaValues; }
            set
            {
                _karoserijskaIzvedbaValues = value;
                OnPropertyChanged(nameof(KaroserijskaIzvedbaValues));
            }
        }

        private string _selectedKaroserijskaIzvedba;
        public string SelectedKaroserijskaIzvedba
        {
            get { return _selectedKaroserijskaIzvedba; }
            set
            {
                _selectedKaroserijskaIzvedba = value;
                OnPropertyChanged(nameof(SelectedKaroserijskaIzvedba));
            }
        }

        public class FilterEventArgs : EventArgs
        {
            public string TipPonudbe { get; set; }
            public string Starost { get; set; }
            public string Znamka { get; set; }
            public string KaroserijskaIzvedba { get; set; }
        }

        public SearchFilterControl()
        {
            InitializeComponent();
            DataContext = this;

            _tipPonudbeValues = new ObservableCollection<string> { "Vse", "Prodaja", "Nakup" };
            _starostValues = new ObservableCollection<string> { "Vse", "Novo", "Rabljeno" };
            _availableZnamke = new ObservableCollection<string>(znamke.Cast<string>());
            _karoserijskaIzvedbaValues = new ObservableCollection<string> { "Vse", "Limuzina", "Karavan", "SUV" };

            _selectedTipPonudbe = "Vse";
            _selectedStarost = "Vse";
            _selectedZnamka = "Vse";
            _selectedKaroserijskaIzvedba = "Vse";
            _availableZnamke.Add("Vse");
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbSearch.Text))
            {
                SearchRequested?.Invoke(this, tbSearch.Text);
            }
            else
            {
                MsgDisplayer("Iskalni niz ne sme biti prazen. Vnesite poizvedbo.");
            }
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            var filterArgs = new FilterEventArgs
            {
                TipPonudbe = SelectedTipPonudbe,
                Starost = SelectedStarost,
                Znamka = SelectedZnamka,
                KaroserijskaIzvedba = SelectedKaroserijskaIzvedba
            };

            FiltersChanged?.Invoke(this, filterArgs);
        }

        public void MsgDisplayer(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
