using MyInventory.Core.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MyInventory.Model;
using MyInventory.Core.Services;
using ClosedXML.Excel;
using System.IO;
using MyInventory.Core;

namespace MyInventory.ViewModel
{
    public class ItemsListViewModel : NotifyPropertyChanged
    {
        private ItemsDB _itemsDB;

        public ObservableCollection<ItemEntry> Items { get; set; }
        public ItemEntry SelectedItem { get; set; }

        public string FilterText { get; set; }
        public List<string> Filters { get; set; }
        public string SelectedFilter { get; set; }

        public RelayCommand ApplyFilter { get; set; }
        public RelayCommand ResetFilter { get; set; }
        public RelayCommand ExportToExcel { get; set; }

        public ItemsListViewModel(MainViewModel mainVM)
        {
            _itemsDB = new ItemsDB();
            LoadItems();

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
            ResetFilter = new RelayCommand(
                () =>
            {
                LoadItems();
                FilterText = "";
                OnPropertyChanged(nameof(FilterText));
            }, () => true);

            ExportToExcel = new RelayCommand(
                () =>
                {
                    var workbook = new XLWorkbook();
                    var ws = workbook.AddWorksheet("Имущество");
                    int row = 1;
                    foreach (ItemEntry item in Items)
                    {
                        ws.Cell("A" + row.ToString()).Value = "ID";
                        ws.Cell("B" + row.ToString()).Value = "Наименование";
                        ws.Cell("C" + row.ToString()).Value = "Местонахождение";
                        ws.Cell("D" + row.ToString()).Value = "Инв. номер";
                        ws.Cell("E" + row.ToString()).Value = "Количество";
                        ws.Cell("F" + row.ToString()).Value = "Ед. изм.";
                        ws.Cell("G" + row.ToString()).Value = "Дата постановки на учет";
                        ws.Cell("H" + row.ToString()).Value = "Дата снятия с учета";
                    }
                    row++;
                    foreach (ItemEntry item in Items)
                    {
                        ws.Cell("A" + row.ToString()).Value = item.ID;
                        ws.Cell("B" + row.ToString()).Value = item.ItemType.Name;
                        ws.Cell("C" + row.ToString()).Value = item.Location;
                        ws.Cell("D" + row.ToString()).Value = item.InvNumber;
                        ws.Cell("E" + row.ToString()).Value = item.Quantity;
                        ws.Cell("F" + row.ToString()).Value = item.ItemType.MeasureUnit;
                        ws.Cell("G" + row.ToString()).Value = item.RegistrationDate;
                        ws.Cell("H" + row.ToString()).Value = item.ExpirationDate;
                        row++;
                    }
                    string fileName;
                    int i = 1;
                    do
                    {
                        fileName = $"profile{AppSettings.CurrentProfile.ID}\\Список_имущества_{i}.xlsx";
                        i++;
                    }
                    while (File.Exists(fileName));
                    workbook.SaveAs(fileName);
                    mainVM.ShowMessage($"Файл сохранен: {fileName}");
                },
                () => true);
        }

        private void LoadItems()
        {
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
