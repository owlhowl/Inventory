using MyInventory.Core;
using MyInventory.Core.DB;
using MyInventory.Core.Services;
using MyInventory.Model;
using MyInventory.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyInventory.ViewModel
{
    public class ProfileCreationViewModel : NotifyPropertyChanged
    {
        private ProfilesDB _profilesDB;

        public ObservableCollection<Profile> Profiles { get; set; }
        public Profile NewProfile { get; set; }
        public Profile SelectedProfile { get; set; }
        public string AdminCreatePassword { get; set; }
        public string AdminRemovePassword { get; set; }

        public RelayCommand AddNewProfile { get; set; }
        public RelayCommand RemoveSelectedProfile { get; set; }
        public RelayCommand Back { get; set; }

        public ProfileCreationViewModel(MainViewModel mainVM)
        {
            _profilesDB = new ProfilesDB();
            NewProfile = new Profile();
            UpdateList();
            AdminCreatePassword = "";
            AdminRemovePassword = "";
            AddNewProfile = new RelayCommand(
                () => 
                {
                    if (string.IsNullOrWhiteSpace(NewProfile.Name))
                        mainVM.ShowMessage("Введите имя профиля!");
                    else if (string.IsNullOrWhiteSpace(NewProfile.Password))
                        mainVM.ShowMessage("Введите пароль!");
                    else if (AppSettings.AdminPassword != AdminCreatePassword)
                        mainVM.ShowMessage("Неверный пароль администратора!");
                    else
                    {
                        mainVM.ShowMessage("Профиль добавлен.");
                        _profilesDB.AddProfile(NewProfile);
                        _profilesDB.Save();
                        UpdateList();
                    }    
                }, 
                () => true);

            RemoveSelectedProfile = new RelayCommand(
                () =>
                {
                    if (SelectedProfile == null)
                        mainVM.ShowMessage("Выберите профиль!");
                    else if (AppSettings.AdminPassword != AdminRemovePassword)
                        mainVM.ShowMessage("Неверный пароль администратора!");
                    else
                    {
                        mainVM.ShowMessage("Профиль удален.");
                        _profilesDB.RemoveProfile(SelectedProfile);
                        _profilesDB.Save();
                        UpdateList();
                    }
                },
                () => true);

            Back = new RelayCommand(
                () => mainVM.CurrentView = new ProfileSelection(mainVM), 
                () => true);
        }

        private void UpdateList()
        {
            Profiles = new ObservableCollection<Profile>(_profilesDB.GetDB());
            OnPropertyChanged(nameof(Profiles));
        }
    }
}
