using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CleverShoppingList.Models
{
    [Table("Recipes")]
    class Recipe : ViewModelBase
    {
        int id;
        string name;
        
        [AutoIncrement, PrimaryKey]
        public int ID { get => id; set { SetProperty(ref id, value); } }
        public string Name { get => name; set { SetProperty(ref name, value); } }

        //A recipe is a simple object, used only as an object for ListItems to group up with.
        Recipe(string name)
        {
            this.name = name;
        }
         
    }
}
