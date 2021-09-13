using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyInventory.Core.DB
{
    abstract public class DB<T> where T : class
    {
        protected List<T> _db;
        protected string _fileName;
        protected int _autoIncrementId = 1;

        public List<T> GetDB()
        {
            return _db;
        }

        public DB(string fileName)
        {
            if (fileName == null)
                return;
            _db = new List<T>();
            _fileName = $"profile{AppSettings.CurrentProfile.ID}\\db_{fileName}";

            if (!File.Exists(_fileName))
                return;

            Load();
        }

        public void Load()
        {
            using (FileStream fs = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
            {
                var bf = new BinaryFormatter();
                _autoIncrementId = (int)bf.Deserialize(fs);
                _db = (List<T>)bf.Deserialize(fs);
            }
        }

        public void Save()
        {
            using (FileStream fs = new FileStream(_fileName, FileMode.Create, FileAccess.Write))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, _autoIncrementId);
                bf.Serialize(fs, _db);
            }
        }
    }
}
