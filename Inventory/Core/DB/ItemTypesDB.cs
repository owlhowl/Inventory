using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyInventory.Core.DB
{
    public class ItemTypesDB : DB<ItemType>
    {
        public ItemTypesDB() : base("item_types.bin"){ }

        private ItemType CreateItemType()
        {
            ItemType itemType = new ItemType();
            itemType.ID = _autoIncrementId;
            _autoIncrementId++;
            _db.Add(itemType);
            return itemType;
        }

        public void RemoveItemType(ItemType itemType)
        {
            _db.Remove(itemType);
        }

        public void AddItemType(ItemType newItemType)
        {
            ItemType itemType = CreateItemType();
            itemType.Name = newItemType.Name;
            itemType.MeasureUnit = newItemType.MeasureUnit;
            itemType.Description = newItemType.Description;
        }
    }
}
