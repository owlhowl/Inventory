using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyInventory.Core.DB
{
    public class ProfilesDB : DB<Profile>
    {
        public ProfilesDB() : base(null)
        {
            _db = new List<Profile>();
            _fileName = "profiles_db.bin";

            if (!File.Exists(_fileName))
            {
                _db.Add(new Profile() { ID = 0, Name = "NotSelected", Password = null });
                Save();
            }
            else
            {
                Load();
            }
        }

        private Profile CreateProfile()
        {
            Profile profile = new Profile();
            profile.ID = _autoIncrementId;
            _autoIncrementId++;
            _db.Add(profile);
            Directory.CreateDirectory("profile" + profile.ID.ToString());
            return profile;
        }

        public void AddProfile(Profile newProfile)
        {
            Profile profile = CreateProfile();
            profile.Name = newProfile.Name;
            profile.Password = newProfile.Password;
        }

        public void UpdateProfile(Profile profile)
        {
            Profile oldProfile = new Profile();
            if (!_db.Exists((p) => p.ID == profile.ID) || profile.ID == 0)
                return;
                
            oldProfile = _db.Find((p) => p.ID == profile.ID);

            oldProfile.Name = profile.Name;
            oldProfile.Password = profile.Password;
            oldProfile.ActiveStocktaking = profile.ActiveStocktaking;
            oldProfile.ItemsOnEdit = profile.ItemsOnEdit;
        }

        public void RemoveProfile(Profile profile)
        {
            _db.Remove(profile);
        }
    }
}
