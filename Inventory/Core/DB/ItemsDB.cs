using Microsoft.Win32.SafeHandles;
using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyInventory.Core.DB
{
    public class ItemsDB : DB<ItemEntry>
    {
        public ItemsDB() : base("item_entries.bin") { }

        private ItemEntry CreateItem()
        {
            ItemEntry itemEntry = new ItemEntry();
            itemEntry.ID = _autoIncrementId;
            _autoIncrementId++;
            _db.Add(itemEntry);
            return itemEntry;
        }

        public void RemoveItem(ItemEntry itemEntry)
        {
            _db.Remove(itemEntry);
        }

        public ItemEntry AddItem(ItemEntry newItem)
        {
            ItemEntry item;
            if (_db.Exists((item) => item.ID == newItem.ID))
            {
                item = _db.Find((item) => item.ID == newItem.ID);
            }
            else
            {
                item = CreateItem();
            }
            item.ItemType = newItem.ItemType;
            item.InvNumber = newItem.InvNumber != null ? newItem.InvNumber : "";
            item.Location = newItem.Location != null ? newItem.Location : "";
            item.Quantity = newItem.Quantity;
            item.RegistrationDate = newItem.RegistrationDate;
            item.ExpirationDate = newItem.ExpirationDate;

            return item;
        }
    }
}
