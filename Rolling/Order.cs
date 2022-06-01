using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rolling
{
    class Order
    {
        public int Number { get; set; }
        public string Client { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Product { get; set; }
        public int Amount { get; set; }
        public string Delivery { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }

        public Order(int number, string client, string email, string phone, string adress, string product, int amount, string delivery, double price, DateTime date)
        {
            Number = number;
            Client = client;
            Email = email;
            Phone = phone;
            Adress = adress;
            Product = product;
            Amount = amount;
            Delivery = delivery;
            Price = price;
            Date = date;
        }
    }
}
