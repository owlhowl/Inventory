using MyInventory.Core;
using MyInventory.Core.DB;
using MyInventory.Core.Services;
using MyInventory.Model;
using MyInventory.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyInventory.ViewModel
{
    public class ProfileSelectionViewModel : NotifyPropertyChanged
    {
        private ProfilesDB _profilesDB;

        public List<Profile> Profiles { get; set; }

        public Profile SelectedProfile { get; set; }
        public string EnteredPassword { get; set; }

        public RelayCommand Login { get; set; }
        public RelayCommand OpenCreateProfilePage { get; set; }

        public ProfileSelectionViewModel(MainViewModel mainVM)
        {
            _profilesDB = new ProfilesDB();
            Profiles = _profilesDB.GetDB();

            AppSettings.CurrentProfile = Profiles.Find((p) => p.ID == 0);

            Login = new RelayCommand(
                () => 
                {
                    if (SelectedProfile == null)
                        mainVM.ShowMessage("Выберите профиль!");
                    else if (EnteredPassword != SelectedProfile.Password)
                        mainVM.ShowMessage("Неверный пароль!");
                    else
                    {
                        AppSettings.CurrentProfile = SelectedProfile;
                        mainVM.CurrentView = new ItemsList(mainVM);
                        mainVM.ItemsPageOpened = true;
                    }
                }, 
                () => true);

            OpenCreateProfilePage = new RelayCommand(
                () => mainVM.CurrentView = new ProfileCreation(mainVM), 
                () => true);
        }
    }
}
