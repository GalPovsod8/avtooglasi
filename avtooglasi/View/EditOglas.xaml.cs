using avtooglasi.Classes;
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

namespace avtooglasi.View
{
    /// <summary>
    /// Interaction logic for EditOglas.xaml
    /// </summary>
    public partial class EditOglas : Window
    {

        private static EditOglas? _instance;
        private readonly ViewModel _viewModel;

        public EditOglas(ViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            this.Closing += EditOglasWindow_Closing;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectedOglas))
            {
                if (_viewModel.SelectedOglas == null)
                {
                    this.Close();
                }
                else
                {
                    _viewModel.UpdateCurrentOglas();
                }
            }
        }

        private void EditOglasWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            _instance = null;
        }

        public static EditOglas? GetInstance(ViewModel viewModel)
        {
            if (_instance == null || !_instance.IsLoaded)
            {
                _instance = new EditOglas(viewModel);
            }
            return _instance;

        }
    }
}
