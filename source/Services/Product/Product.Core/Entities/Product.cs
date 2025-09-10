using Product.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public Product(string name, decimal price, int stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
        }        
        public void Update(string name, decimal price, int stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
        }
        public void StockDecrease(int count)
        {
            Stock -= count;
        }
        public void StockIncrease(int count)
        {
            Stock += count;
        }
    }
}
