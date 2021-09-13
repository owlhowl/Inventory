using System;
using System.Collections.Generic;
using System.Text;

namespace MyInventory.Model
{
    [Serializable]
    public class ItemChange
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int StocktakingID { get; set; }
        public int Quantity { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        public string MeasureUnit { get; set; }
        public string InvNumber { get; set; }
        public string Location { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
