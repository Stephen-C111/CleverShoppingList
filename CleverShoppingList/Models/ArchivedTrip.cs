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
        DateTime date;

        [AutoIncrement, PrimaryKey]
        public int ID { get => id; set { SetProperty(ref id, value); } }
        public DateTime Date { get => date; set { SetProperty(ref date, value); } }

        //Like Recipes, ArchivedTrips are simple, and act as an object for ArchivedItems to group up with, and in fact require no values to be constructed.
        ArchivedTrip()
        {
            date = DateTime.Today;
        }
    }
}
