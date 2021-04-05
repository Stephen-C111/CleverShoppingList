using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CleverShoppingList.Models
{
    [Table("ArchivedTrips")]
    class ArchivedTrip : ViewModelBase
    {
        int id;
        DateTime tripDate;
        decimal cost;

        [AutoIncrement, PrimaryKey]
        public int ID { get => id; set { SetProperty(ref id, value); } }
        public DateTime TripDate { get => tripDate; set { SetProperty(ref tripDate, value); } }
        public decimal Cost { get => cost; set { SetProperty(ref cost, value); } }

        //Like Recipes, ArchivedTrips are simple, and act as an object for ArchivedItems to group up with.
        public ArchivedTrip()
        {
            //blank constructor for SQLite
        }
        public ArchivedTrip(DateTime date, decimal price = 0)
        {
            TripDate = date;
            Cost = price; //This is for debug purposes, normal trips have their cost calculated at check-out time.
            
        }
    }
}
