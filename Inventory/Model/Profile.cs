using System;
using System.Collections.Generic;
using System.Text;

namespace MyInventory.Model
{
    [Serializable]
    public class Profile
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public Stocktaking ActiveStocktaking { get; set; }
        public List<ItemEntry> ItemsOnEdit { get; set; }
    }
}
