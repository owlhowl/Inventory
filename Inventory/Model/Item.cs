using System;
using System.Collections.Generic;
using System.Text;

namespace MyInventory.Model
{
    [Serializable]
    public class ItemType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MeasureUnit { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    public class ItemEntry
    {
        public int ID { get; set; }
        public ItemType ItemType { get; set; }
        public int Quantity { get; set; }
        public string InvNumber { get; set; }
        public string Location { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
