using MyInventory.Core;
using MyInventory.Core.DB;
using MyInventory.Core.Services;
using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyInventory.ViewModel
{
    public class StocktakingViewModel : NotifyPropertyChanged
    {
        private ItemsDB _itemsDB;
        private StocktakingsDB _stocktakingsDB;
        private ItemChangesDB _itemChangesDB;
        private ProfilesDB _profilesDB;

        public string FilterText { get; set; }
        public List<string> Filters { get; set; }
        public string SelectedFilter { get; set; }
        public RelayCommand ApplyFilter { get; set; }
        public RelayCommand ResetFilter { get; set; }

        public ObservableCollection<ItemEntry> Items { get; set; }
        public ItemEntry SelectedItem { get; set; }
        public string StocktakingButtonText { get; set; }
        public string ActiveStocktakingText { get; set; }
        public bool IsActive { get => AppSettings.CurrentProfile.ActiveStocktaking != null; }
        public RelayCommand Stocktaking { get; set; }
        public RelayCommand CancelStocktaking { get; set; }

        public StocktakingViewModel(MainViewModel mainVM)
        {
            _itemsDB = new ItemsDB();
            _stocktakingsDB = new StocktakingsDB();
            _profilesDB = new ProfilesDB();
            _itemChangesDB = new ItemChangesDB();

            LoadItems();
            Refresh();


            Stocktaking = new RelayCommand(
                () => 
                {
                    if (IsActive)
                        End();
                    else
                        Begin();
                }, 
                () => true);

            CancelStocktaking = new RelayCommand(
                () =>
                {
                    AppSettings.CurrentProfile.ActiveStocktaking = null;
                    LoadItems();
                    Refresh();
                },
                () => IsActive);

            Filters = new List<string>
            {
                "По наименованию",
                "По месту",
                "По номеру"
            };
            FilterText = "";
            SelectedFilter = Filters[0];

            ApplyFilter = new RelayCommand(
                () => FilterItems(),
                () => true);
            ResetFilter = new RelayCommand(() =>
            {
                LoadItems();
                FilterText = "";
                OnPropertyChanged(nameof(FilterText));
            }, () => true);

            mainVM.OnWindowClose += SaveOnExit;
        }

        private void SaveOnExit(object sender, EventArgs e)
        {
            _profilesDB.UpdateProfile(AppSettings.CurrentProfile);
            _profilesDB.Save();
        }

        private void Begin()
        {
            AppSettings.CurrentProfile.ActiveStocktaking = _stocktakingsDB.CreateStocktaking();
            AppSettings.CurrentProfile.ItemsOnEdit = _itemsDB.GetDB();

            _profilesDB.UpdateProfile(AppSettings.CurrentProfile);
            _profilesDB.Save();
            _stocktakingsDB.Save();
            LoadItems();
            Refresh();
        }

        private void End()
        {
            _itemsDB.Load();
            AppSettings.CurrentProfile.ActiveStocktaking.End();
            foreach (ItemEntry item in Items)
            {
                if (IsItemChanged(item))
                {
                    _itemChangesDB.AddItemChange(item, ActionType.Changed, AppSettings.CurrentProfile.ActiveStocktaking);
                    _itemsDB.AddItem(item);
                }
            }

            _stocktakingsDB.Save();
            _itemChangesDB.Save();
            _itemsDB.Save();
            AppSettings.CurrentProfile.ActiveStocktaking = null;
            _profilesDB.UpdateProfile(AppSettings.CurrentProfile);
            _profilesDB.Save();
            LoadItems();
            Refresh();
        }

        private bool IsItemChanged(ItemEntry item)
        {
            ItemEntry oldItem = _itemsDB.GetDB().Find((i) => i.ID == item.ID);

            if (item.Quantity != oldItem.Quantity ||
                item.InvNumber != oldItem.InvNumber ||
                item.ExpirationDate != oldItem.ExpirationDate ||
                item.Location != oldItem.Location)
                return true;
            return false;
        }

        private void Refresh()
        {
            if (IsActive)
            {
                StocktakingButtonText = "Завершить";
                ActiveStocktakingText = $"начато {AppSettings.CurrentProfile.ActiveStocktaking.BeginDate.ToShortDateString()} в {AppSettings.CurrentProfile.ActiveStocktaking.BeginDate.ToShortTimeString()}";
            }
            else
            {
                StocktakingButtonText = "Начать";
                ActiveStocktakingText = "нет";
            }

            OnPropertyChanged(nameof(ActiveStocktakingText));
            OnPropertyChanged(nameof(StocktakingButtonText));
            OnPropertyChanged(nameof(IsActive));
        }

        private void LoadItems()
        {
            if (IsActive)
                Items = new ObservableCollection<ItemEntry>(AppSettings.CurrentProfile.ItemsOnEdit);
            else
                Items = new ObservableCollection<ItemEntry>(_itemsDB.GetDB());
            OnPropertyChanged(nameof(Items));
        }

        private void FilterItems()
        {
            List<ItemEntry> filteredItems = new List<ItemEntry>();
            List<ItemEntry> allItems = _itemsDB.GetDB();
            switch (SelectedFilter)
            {
                case "По наименованию":
                    foreach (ItemEntry item in allItems)
                        if (item.ItemType.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                            filteredItems.Add(item);
                    break;
                case "По месту":
                    foreach (ItemEntry item in allItems)
                        if (item.Location.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                            filteredItems.Add(item);
                    break;
                case "По номеру":
                    foreach (ItemEntry item in allItems)
                        if (item.InvNumber.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                            filteredItems.Add(item);
                    break;
            }
            Items.Clear();
            foreach (ItemEntry item in filteredItems)
                Items.Add(item);
        }
    }
}
