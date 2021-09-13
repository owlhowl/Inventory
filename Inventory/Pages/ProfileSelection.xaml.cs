using MyInventory.ViewModel;
using System.Windows.Controls;

namespace MyInventory.Pages
{
    /// <summary>
    /// Interaction logic for ProfileSelection.xaml
    /// </summary>
    public partial class ProfileSelection : Page
    {
        public ProfileSelection(MainViewModel mainVM)
        {
            InitializeComponent();
            DataContext = new ProfileSelectionViewModel(mainVM);
        }
    }
}
