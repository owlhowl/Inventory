using System;
using System.Collections.Generic;
using System.Text;

namespace MyInventory.Model
{
    [Serializable]
    public class Stocktaking
    {
        public int ID { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public void Begin()
        {
            if(BeginDate == new DateTime())
                BeginDate = DateTime.Now;
        }

        public void End()
        {
            if(EndDate == new DateTime())
                EndDate = DateTime.Now;
        }
    }
}
