using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CleverShoppingList.Models
{
    [Table("ArchivedItems")]
    class ArchivedItem : ViewModelBase
    {
        int id;
        int tripID;
        string name;
        int amount;
        decimal price;

        [AutoIncrement, PrimaryKey]
        public int ID { get => id; set { SetProperty(ref id, value); } }
        public int TripID { get => tripID; set { SetProperty(ref tripID, value); } }
        public string Name { get => name; set { SetProperty(ref name, value); } }
        public int Amount { get => amount; set { SetProperty(ref amount, value); } }
        public decimal Price { get => price; set { SetProperty(ref price, value); } }

        //ArchivedItems are structurally simpler than a ListItem and only contain necessary values for reporting purposes.
        //The price value should check to see if the sale price is not null, and use that value if so.
        ArchivedItem(int tripID, string name, int amount, decimal price)
        {
            this.tripID = tripID;
            this.name = name;
            this.amount = amount;
            this.price = price;
        }
    }
}
