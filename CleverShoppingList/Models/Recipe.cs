using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CleverShoppingList.Models
{
    [Table("Recipes")]
    public class Recipe : ViewModelBase
    {
        int id;
        string name;
        int purchased;
        decimal price;
        
        [AutoIncrement, PrimaryKey]
        public int ID { get => id; set { SetProperty(ref id, value); } }
        public string Name { get => name; set { SetProperty(ref name, value); } }
        public int Purchased { get => purchased; set { SetProperty(ref purchased, value); } }
        public decimal Price { get => price; set { SetProperty(ref price, value); } }
        

        //A recipe is a simple object, used only as an object for ListItems to group up with.
        public Recipe()
        {
            this.name = "New Recipe";
        }
        public Recipe(string name)
        {
            this.name = name;
        }
         
    }
}
