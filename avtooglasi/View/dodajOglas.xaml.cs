using avtooglasi.Classes;
using avtooglasi.Model;
using Microsoft.Win32;
using System.Windows;

namespace avtooglasi
{
    public partial class dodajOglas : Window
    {
        public dodajOglas(ViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
