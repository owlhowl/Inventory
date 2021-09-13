using ClosedXML.Excel;
using MyInventory.Core;
using MyInventory.Core.DB;
using MyInventory.Core.Services;
using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MyInventory.ViewModel
{
    public class StocktakingHistoryViewModel : NotifyPropertyChanged
    {
        private ItemChangesDB _itemChangesDB;

        public ObservableCollection<ItemChange> ItemChanges { get; set; }

        public string FilterText { get; set; }
        public List<string> Filters { get; set; }
        public string SelectedFilter { get; set; }

        public RelayCommand ApplyFilter { get; set; }
        public RelayCommand ResetFilter { get; set; }
        public RelayCommand ExportToExcel { get; set; }

        public StocktakingHistoryViewModel(MainViewModel mainVM)
        {
            _itemChangesDB = new ItemChangesDB();
            LoadItemChanges();

            Filters = new List<string>
            {
                "По наименованию",
                "По действию",
                "По месту",
                "По номеру",
                "По ид. имущества",
                "По ид. инвентаризации"
            };
            FilterText = "";
            SelectedFilter = Filters[0];

            ApplyFilter = new RelayCommand(
                () => FilterItemChanges(),
                () => true);
            ResetFilter = new RelayCommand(() =>
            {
                LoadItemChanges();
                FilterText = "";
                OnPropertyChanged(nameof(FilterText));
            }, () => true);

            ExportToExcel = new RelayCommand(
                () =>
                {
                    var workbook = new XLWorkbook();
                    var ws = workbook.AddWorksheet("Имущество");
                    int row = 1;
                    foreach (ItemChange itemChange in ItemChanges)
                    {
                        ws.Cell("A" + row.ToString()).Value = "№ п/п";
                        ws.Cell("B" + row.ToString()).Value = "Наименование";
                        ws.Cell("C" + row.ToString()).Value = "№ имущ.";
                        ws.Cell("D" + row.ToString()).Value = "Действие";
                        ws.Cell("E" + row.ToString()).Value = "№ инв.";
                        ws.Cell("F" + row.ToString()).Value = "Дата и время";
                        ws.Cell("G" + row.ToString()).Value = "Местонахождение";
                        ws.Cell("H" + row.ToString()).Value = "Инв. номер";
                        ws.Cell("I" + row.ToString()).Value = "Количество";
                        ws.Cell("J" + row.ToString()).Value = "Ед. изм.";
                        ws.Cell("K" + row.ToString()).Value = "Дата постановки на учет";
                        ws.Cell("L" + row.ToString()).Value = "Дата снятия с учета";
                    }
                    row++;
                    foreach (ItemChange itemChange in ItemChanges)
                    {
                        ws.Cell("A" + row.ToString()).Value = itemChange.ID;
                        ws.Cell("B" + row.ToString()).Value = itemChange.Name;
                        ws.Cell("C" + row.ToString()).Value = itemChange.ItemID;
                        ws.Cell("D" + row.ToString()).Value = itemChange.Action;
                        ws.Cell("E" + row.ToString()).Value = itemChange.StocktakingID;
                        ws.Cell("F" + row.ToString()).Value = itemChange.ActionDate;
                        ws.Cell("G" + row.ToString()).Value = itemChange.Location;
                        ws.Cell("H" + row.ToString()).Value = itemChange.InvNumber;
                        ws.Cell("I" + row.ToString()).Value = itemChange.Quantity;
                        ws.Cell("J" + row.ToString()).Value = itemChange.MeasureUnit;
                        ws.Cell("K" + row.ToString()).Value = itemChange.RegistrationDate;
                        ws.Cell("L" + row.ToString()).Value = itemChange.ExpirationDate;
                        row++;
                    }
                    string fileName;
                    int i = 1;
                    do
                    {
                        fileName = $"profile{AppSettings.CurrentProfile.ID}\\История_изменений_{i}.xlsx";
                        i++;
                    }
                    while (File.Exists(fileName));
                    workbook.SaveAs(fileName);
                    mainVM.ShowMessage($"Файл сохранен: {fileName}");
                },
                () => true);
        }

        private void FilterItemChanges()
        {
            List<ItemChange> filteredItemChanges = new List<ItemChange>();
            List<ItemChange> allItemChanges = _itemChangesDB.GetDB();
            switch (SelectedFilter)
            {
                case "По наименованию":
                    foreach (ItemChange itemChange in allItemChanges)
                        if (itemChange.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                            filteredItemChanges.Add(itemChange);
                    break;
                case "По месту":
                    foreach (ItemChange itemChange in allItemChanges)
                        if (itemChange.Location.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                            filteredItemChanges.Add(itemChange);
                    break;
                case "По инв. номеру":
                    foreach (ItemChange itemChange in allItemChanges)
                        if (itemChange.InvNumber.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                            filteredItemChanges.Add(itemChange);
                    break;
                case "По действию":
                    foreach (ItemChange itemChange in allItemChanges)
                        if (itemChange.Action.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                            filteredItemChanges.Add(itemChange);
                    break;
                case "По ид. имущества":
                    foreach (ItemChange itemChange in allItemChanges)
                        if (itemChange.ItemID.ToString() == FilterText)
                            filteredItemChanges.Add(itemChange);
                    break;
                case "По ид. инвентаризации":
                    foreach (ItemChange itemChange in allItemChanges)
                        if (itemChange.StocktakingID.ToString() == FilterText)
                            filteredItemChanges.Add(itemChange);
                    break;
            }
            ItemChanges.Clear();
            foreach (ItemChange item in filteredItemChanges)
                ItemChanges.Add(item);
        }

        private void LoadItemChanges()
        {
            ItemChanges = new ObservableCollection<ItemChange>(_itemChangesDB.GetDB());
            OnPropertyChanged(nameof(ItemChanges));
        }
    }
}
