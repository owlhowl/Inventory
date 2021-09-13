using Microsoft.Win32.SafeHandles;
using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyInventory.Core.DB
{
    public class ItemChangesDB : DB<ItemChange>
    {
        public ItemChangesDB() : base("item_changes.bin") { }

        private ItemChange CreateItemChange()
        {
            ItemChange itemChange = new ItemChange();
            itemChange.ID = _autoIncrementId;
            _autoIncrementId++;
            _db.Add(itemChange);
            return itemChange;
        }

        internal void AddItemChange(ItemEntry changedItem, ActionType itemAction, Stocktaking currentStocktaking = null)
        {
            ItemChange itemChange = CreateItemChange();
            switch (itemAction)
            {
                case ActionType.Created:
                    itemChange.Action = "Создано";
                    itemChange.ActionDate = DateTime.Now;
                    break;
                case ActionType.Changed:
                    itemChange.StocktakingID = currentStocktaking.ID;
                    itemChange.Action = "Изменено";
                    itemChange.ActionDate = currentStocktaking.EndDate;
                    break;
                case ActionType.Removed:
                    itemChange.Action = "Удалено";
                    itemChange.ActionDate = DateTime.Now;
                    break;
            }
            itemChange.ItemID = changedItem.ID;
            itemChange.Quantity = changedItem.Quantity;
            itemChange.InvNumber = changedItem.InvNumber != null ? changedItem.InvNumber : "";
            itemChange.Location = changedItem.Location != null ? changedItem.Location : "";
            itemChange.ExpirationDate = changedItem.ExpirationDate;
            itemChange.RegistrationDate = changedItem.RegistrationDate;
            itemChange.Name = changedItem.ItemType.Name;
            itemChange.MeasureUnit = changedItem.ItemType.MeasureUnit;
        }
    }
}
