using MyInventory.ViewModel;
using System.ComponentModel;
using System.Windows;

namespace MyInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected void Window_Closing(object sender, CancelEventArgs e)
        {
            MainViewModel mainVM = DataContext as MainViewModel;
            mainVM.WindowClose();
        }
    }
}
