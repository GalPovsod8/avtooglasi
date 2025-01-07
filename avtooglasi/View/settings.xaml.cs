using avtooglasi.Classes;

using System.Windows;

namespace avtooglasi.View
{
    public partial class settings : Window
    {
        public settings(ViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
