using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rolling
{
    class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double PriceTon { get; set; }
        public int Amount { get; set; }

        public Product(string name, double price, double priceton, int amount)
        {
            Name = name;
            Price = price;
            PriceTon = priceton;
            Amount = amount;
        }
    }
}
