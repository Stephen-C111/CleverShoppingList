using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;
using System.Threading.Tasks;

namespace CleverShoppingList.Models
{
    public enum Priority { Low, Normal, High, Urgent }
    [Table("ListItems")]
    class ListItem : ViewModelBase
    {
        int id;
        int ownerID = -1; //refers to a recipe that contains this listitem. If it is part of the main shopping list, its ID will = -1
        int foreignID; //refers to a stored item that this listitem represents.
        Item foreignItem; //This will be populated using the foreignID
        Priority priority;
        bool check;
        int amount; 
        decimal price;
        string recipeName;
        
        

        [AutoIncrement, PrimaryKey]
        public int ID { get => id; set { SetProperty(ref id, value); } }
        public int OwnerID { get => ownerID; set { SetProperty(ref ownerID, value); } }
        public int ForeignID { get => foreignID; set { SetProperty(ref foreignID, value); } }
        public decimal Price { get => price; set { SetProperty(ref price, value); } }
        public string Name { get => foreignItem != null ? foreignItem.Name : "NULL"; }
        public Priority Priority { get => priority; set { SetProperty(ref priority, value); } }
        public bool Check { get => check; set { SetProperty(ref check, value); TabsViewModel.tvm.Conn.UpdateAsync(this); ListViewModel.lvm.CountPrices(); } }
        public int Amount { get => amount; set { SetProperty(ref amount, value); } }
        public string RecipeName { get => recipeName; set { SetProperty(ref recipeName, value); } }
        public bool HasRecipe { get => ownerID == -1 ? false : true; }
        

        //ListItems require the foreignID for an item in Items to reference from, as well as a priority. 
        //The ownerID defaults to -1 to indicate it belongs to the shopping list. Otherwise, it's part 
        //of a recipe.
        public ListItem()
        {

        }
        public ListItem(int foreignID, Priority priority, int ownerID = -1, int amount = 1)
        {
            this.ForeignID = foreignID;
            this.Priority = priority;
            this.OwnerID = ownerID;
            this.Amount = amount;
            
        }

        async public Task LinkToForeignItem()
        {
            try
            {
                var query = from x in TabsViewModel.tvm.Conn.Table<Item>()
                            where x.ID == foreignID
                            select x;
                
                foreignItem = await query.FirstAsync();
                if (foreignItem.Name == null)
                {
                    await TabsViewModel.tvm.Conn.DeleteAsync(this);
                    return;
                }
                this.Price = foreignItem.Price;
            }
            catch
            {
                await TabsViewModel.tvm.Conn.DeleteAsync(this);
                return;
            }
            
        }
    }
}
