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

        //Like Recipes, ArchivedTrips are simple, and act as an object for ArchivedItems to group up with, and in fact require no values to be constructed.
        public ArchivedTrip()
        {
            TripDate = DateTime.Today;
            
        }
    }
}
