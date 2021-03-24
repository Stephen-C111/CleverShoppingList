﻿using System;
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
        decimal salePrice;
        

        [AutoIncrement, PrimaryKey]
        public int ID { get => id; set { SetProperty(ref id, value); } }
        public int OwnerID { get => ownerID; set { SetProperty(ref ownerID, value); } }
        public int ForeignID { get => foreignID; set { SetProperty(ref foreignID, value); } }
        public decimal Price { get => foreignItem.Price; }
        public string Name { get => foreignItem.Name; }
        public Priority Priority { get => priority; set { SetProperty(ref priority, value); } }
        public bool Check { get => check; set { SetProperty(ref check, value); } }
        public int Amount { get => amount; set { SetProperty(ref amount, value); } }
        public decimal SalePrice { get => salePrice; set { SetProperty(ref salePrice, value); } }

        //ListItems require the foreignID for an item in Items to reference from, as well as a priority. 
        //The ownerID defaults to -1 to indicate it belongs to the shopping list. Otherwise, it's part 
        //of a recipe.
        public ListItem()
        {

        }
        public ListItem(int foreignID, Priority priority, int ownerID = -1)
        {
            this.foreignID = foreignID;
            this.priority = priority;
            this.ownerID = ownerID;
             
        }

        async public Task LinkToForeignItem()
        {
            var query = from x in TabsViewModel.tvm.Conn.Table<Item>()
                        where x.ID == foreignID
                        select x;
            foreignItem = await query.FirstAsync();
        }
    }
}
