using MyInventory.ViewModel;
using System.Windows.Controls;

namespace MyInventory.Pages
{
    /// <summary>
    /// Interaction logic for ItemsList.xaml
    /// </summary>
    public partial class ItemsList : Page
    {
        public ItemsList(MainViewModel mainVM)
        {
            InitializeComponent();
            DataContext = new ItemsListViewModel(mainVM);
        }
    }
}
