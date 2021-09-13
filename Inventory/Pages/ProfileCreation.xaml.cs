using MyInventory.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyInventory.Pages
{
    /// <summary>
    /// Interaction logic for ProfileCreation.xaml
    /// </summary>
    public partial class ProfileCreation : Page
    {
        public ProfileCreation(MainViewModel mainVM)
        {
            InitializeComponent();
            DataContext = new ProfileCreationViewModel(mainVM);
        }
    }
}
