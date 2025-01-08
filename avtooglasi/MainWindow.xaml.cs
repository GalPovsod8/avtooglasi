using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using avtooglasi.Classes;
using avtooglasi.View;
using avtooglasi.View.UserControls;

namespace avtooglasi
{
    public partial class MainWindow : Window
    {
        private oglasiDisplay _oglasiDisplayWindow;

        ViewModel vm = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();
            InitializeSettings();
            DataContext = vm;
            searchFilterControl.SearchRequested += SearchFilterControl_SearchRequested;
            searchFilterControl.FiltersChanged += SearchFilterControl_FiltersChanged;
        }

        private void SearchFilterControl_SearchRequested(object sender, string searchQuery)
        {
            searchQuery = searchQuery.ToLower();

            var filteredResults = vm.AvtoOglasi.Where(oglas =>
                oglas.Naziv.ToLower().Contains(searchQuery) ||
                oglas.Znamka.ToLower().Contains(searchQuery)
            ).ToList();

            if (filteredResults.Count == 0)
            {
                MessageBox.Show("Ni oglasov za iskani niz.", "Rezultat iskanja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                lvAvtoOglasiBigDisplay.ItemsSource = filteredResults;
            }
        }

        private void SearchFilterControl_FiltersChanged(object sender, SearchFilterControl.FilterEventArgs e)
        {
            var filteredResults = vm.AvtoOglasi.Where(oglas =>
                (e.TipPonudbe == "Vse" || Convert.ToString(oglas.Ponudba) == e.TipPonudbe) &&
                (e.Starost == "Vse" || Convert.ToString(oglas.AvtoStarost) == e.Starost) &&
                (e.Znamka == "Vse" || oglas.Znamka == e.Znamka) &&
                (e.KaroserijskaIzvedba == "Vse" || Convert.ToString(oglas.KaroserijskaIzvedba) == e.KaroserijskaIzvedba)
            ).ToList();

            lvAvtoOglasiBigDisplay.ItemsSource = filteredResults;
        }

        private void InitializeSettings()
        {
            if (Properties.Settings.Default.Znamke == null)
            {
                Properties.Settings.Default.Znamke = new StringCollection();
                Properties.Settings.Default.Save();
            }
        }

        private void ToggleSidebar_Checked(object sender, RoutedEventArgs e)
        {
            if (Sidebar.Visibility == Visibility.Visible)
            {
                Sidebar.Visibility = Visibility.Collapsed;
                SidebarColumnWidth.Width = new GridLength(0);
                SidebarColumn2Width.Width = new GridLength(0);
                SidebarColumnWidth.MinWidth = 0;
                SidebarColumn2Width.MinWidth = 0;
                BtnToggleSidebar.Content = ">";
            }
            else
            {
                Sidebar.Visibility = Visibility.Visible;
                SidebarColumnWidth.Width = new GridLength(2, GridUnitType.Star);
                SidebarColumn2Width.Width = new GridLength(2, GridUnitType.Star);
                SidebarColumnWidth.MinWidth = 280;
                SidebarColumn2Width.MinWidth = 280;
                BtnToggleSidebar.Content = "X";
            }
        }

        private void onClickCloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void showMoreInfo()
        {
            if (lvAvtoOglasi.SelectedItem != null)
            {
                if (_oglasiDisplayWindow == null || !_oglasiDisplayWindow.IsVisible)
                {
                    _oglasiDisplayWindow = new oglasiDisplay(vm);
                    _oglasiDisplayWindow.Closed += (s, args) => _oglasiDisplayWindow = null;
                    _oglasiDisplayWindow.Show();
                }
                else
                {
                    _oglasiDisplayWindow.Focus();
                }
            }
        }

        private void lvAvtoOglasi_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMoreInfo();
        }

        private void OnClickShowMore(object sender, RoutedEventArgs e)
        {
            showMoreInfo();
        }

        private void onClickOpenDodajOglasWindow(object sender, RoutedEventArgs e)
        {
            dodajOglas dodajOglas = new dodajOglas(vm);
            dodajOglas.Owner = this;
            dodajOglas.ShowDialog();
        }

        private void onClickOpenEditOglas(object sender, RoutedEventArgs e)
        {
            EditOglas editOglas = new EditOglas(vm);
            editOglas.Owner = this;
            editOglas.ShowDialog();
        }

        private void onClickOpenSettings(object sender, RoutedEventArgs e)
        {
            settings settings = new settings(vm);
            settings.Owner = this;
            settings.ShowDialog();
        }
    }
}