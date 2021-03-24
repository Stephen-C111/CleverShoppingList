using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CleverShoppingList.Models
{
    [Table("Items")]
    class Item : ViewModelBase
    {
        int id;
        string name;
        decimal price;
        int purchased;
        decimal totalCost;

        [AutoIncrement, PrimaryKey]
        public int ID { get => id; set { SetProperty(ref id, value); } }
        public string Name { get => name; set { SetProperty(ref name, value); } }
        public decimal Price { get => price; set { SetProperty(ref price, value); } }
        public int Purchased { get => purchased; set { SetProperty(ref purchased, value); } }
        public decimal TotalCost { get => totalCost; set { SetProperty(ref totalCost, value); } }

        public Item()
        {

        }
        public Item(string name, decimal price = 0) //Every Item must have a name, and price will default to 0, for users that don't know the item's price.
        {
            this.name = name;
            this.price = price;
        }
    }
}
