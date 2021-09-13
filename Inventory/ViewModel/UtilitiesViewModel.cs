using MyInventory.Core.DB;
using MyInventory.Core.Services;
using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace MyInventory.ViewModel
{
    class UtilitiesViewModel : NotifyPropertyChanged
    {
        private ItemsDB _itemsDB;
        private ItemTypesDB _itemTypesDB;
        private ItemChangesDB _itemChangesDB;

        public ObservableCollection<ItemEntry> Items { get; set; }
        public ObservableCollection<ItemType> ItemTypes { get; set; }

        public ItemEntry NewItem { get; set; }
        public ItemType NewItemType { get; set; }

        public ItemEntry SelectedItem { get; set; }
        public ItemType SelectedItemType { get; set; }

        public RelayCommand AddNewItemType { get; set; }
        public RelayCommand RemoveSelectedItemType { get; set; }
        public RelayCommand AddNewItem { get; set; }
        public RelayCommand RemoveSelectedItem { get; set; }

        public UtilitiesViewModel(MainViewModel mainVM)
        {
            _itemsDB = new ItemsDB();
            _itemTypesDB = new ItemTypesDB();
            _itemChangesDB = new ItemChangesDB();

            ClearNewItem();
            ClearNewItemType();
            UpdateLists();

            AddNewItemType = new RelayCommand(
                () =>
                {
                    if (string.IsNullOrWhiteSpace(NewItemType.Name))
                        mainVM.ShowMessage("Введите наименование типа!");
                    else if (string.IsNullOrWhiteSpace(NewItemType.MeasureUnit))
                        mainVM.ShowMessage("Введите единицу измерения!");
                    else
                    {
                        _itemTypesDB.AddItemType(NewItemType);
                        _itemTypesDB.Save();
                        ClearNewItemType();
                        UpdateLists();
                        mainVM.StatusText = "Тип успешно добавлен.";
                    }
                },
                () => true);

            RemoveSelectedItemType = new RelayCommand(
                () =>
                {
                    if (SelectedItemType == null)
                        mainVM.ShowMessage("Выберите тип для удаления!");
                    else if (_itemsDB.GetDB().Exists((item) => item.ItemType.ID == SelectedItemType.ID))
                        mainVM.ShowMessage("Невозможно удалить используемый тип!");
                    else
                    {
                        _itemTypesDB.RemoveItemType(SelectedItemType);
                        _itemTypesDB.Save();
                        UpdateLists();
                        mainVM.StatusText = "Тип успешно удален.";
                    }
                },
                () => true);

            AddNewItem = new RelayCommand(
                () =>
                {
                    if (NewItem.ItemType == null)
                        mainVM.ShowMessage("Выберите тип имущества!");
                    else
                    {
                        ItemEntry newItem = _itemsDB.AddItem(NewItem);
                        _itemsDB.Save();
                        _itemChangesDB.AddItemChange(newItem, ActionType.Created);
                        _itemChangesDB.Save();
                        ClearNewItem();
                        UpdateLists();
                        mainVM.ShowMessage("Имущество успешно добавлено.");
                    }
                },
                () => true);

            RemoveSelectedItem = new RelayCommand(
                () => 
                {
                    if (SelectedItem == null)
                        mainVM.ShowMessage("Выберите имущество для удаления!");
                    else
                    {
                        _itemsDB.RemoveItem(SelectedItem);
                        _itemsDB.Save();
                        _itemChangesDB.AddItemChange(SelectedItem, ActionType.Removed);
                        _itemChangesDB.Save();
                        UpdateLists();
                        mainVM.ShowMessage("Имущество успешно удалено.");
                    }
                },
                () => true);
        }

        private void ClearNewItemType()
        {
            NewItemType = new ItemType();
            OnPropertyChanged(nameof(NewItemType));
        }

        private void ClearNewItem()
        {
            NewItem = new ItemEntry();
            NewItem.RegistrationDate = DateTime.Today;
            NewItem.ExpirationDate = DateTime.Today;
            OnPropertyChanged(nameof(NewItem));
        }

        private void UpdateLists()
        {
            Items = new ObservableCollection<ItemEntry>(_itemsDB.GetDB());
            OnPropertyChanged(nameof(Items));
            ItemTypes = new ObservableCollection<ItemType>(_itemTypesDB.GetDB());
            OnPropertyChanged(nameof(ItemTypes));
        }
    }
}
