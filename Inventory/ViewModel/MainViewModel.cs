using MyInventory.Core;
using MyInventory.Core.Services;
using MyInventory.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyInventory.ViewModel
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private Page _currentView;
        private string _statusText;
        private bool _itemsPageOpened;

        public RelayCommand OpenItemsPage { get; set; }
        public RelayCommand OpenStocktakingPage { get; set; }
        public RelayCommand OpenUtilitiesPage { get; set; }
        public RelayCommand OpenStocktakingHistory { get; set; }
        public RelayCommand OpenProfileSelectionPage { get; set; }
        public bool ItemsPageOpened { get => _itemsPageOpened; set { _itemsPageOpened = value; OnPropertyChanged(nameof(ItemsPageOpened)); } }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public Page CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                StatusText = "";
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public MainViewModel()
        {
            CurrentView = new ProfileSelection(this);

            OpenItemsPage = new RelayCommand(
                () =>
                {
                    if (!(CurrentView is ItemsList))
                        CurrentView = new ItemsList(this);
                },
                () => IsEditable());

            OpenStocktakingPage = new RelayCommand(
                () =>
                {
                    if (!(CurrentView is Stocktaking))
                        CurrentView = new Stocktaking(this);
                },
                () => IsEditable());

            OpenUtilitiesPage = new RelayCommand(
                () =>
                {
                    if (!(CurrentView is Utilities))
                        CurrentView = new Utilities(this);
                },
                () => IsEditable());

            OpenStocktakingHistory = new RelayCommand(
                () =>
                {
                    if (!(CurrentView is StocktakingHistory))
                        CurrentView = new StocktakingHistory(this);
                },
                () => IsEditable());

            OpenProfileSelectionPage = new RelayCommand(
                () =>
                {
                    if (!(CurrentView is ProfileSelection))
                        CurrentView = new ProfileSelection(this);
                },
                () => true);
        }

        private bool IsEditable()
        {
            if (AppSettings.CurrentProfile == null || AppSettings.CurrentProfile.ID == 0)
                return false;
            return true;
        }

        public void ShowMessage(string message)
        {
            StatusText = message;
        }

        public event EventHandler OnWindowClose;

        public void WindowClose()
        {
            OnWindowClose?.Invoke(null, new EventArgs());
        }
    }
}