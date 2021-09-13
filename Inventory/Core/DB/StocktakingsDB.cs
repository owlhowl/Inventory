using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MyInventory.Core.DB
{
    public class StocktakingsDB : DB<Stocktaking>
    {
        public StocktakingsDB() : base("stocktakings.bin") { }

        public Stocktaking CreateStocktaking()
        {
            Stocktaking stocktaking = new Stocktaking();
            stocktaking.ID = _autoIncrementId;
            _autoIncrementId++;
            _db.Add(stocktaking);
            stocktaking.Begin();
            return stocktaking;
        }

        public void RemoveStocktaking(Stocktaking stocktaking)
        {
            _db.Remove(stocktaking);
        }
    }
}
