using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;
using System.Threading.Tasks;

namespace CleverShoppingList.ViewModels
{
    class ItemViewModel : ViewModelBase
    {
        public static ItemViewModel ivm;
        ObservableCollection<Item> itemList = new ObservableCollection<Item>();
        bool editing;
        bool notEditing = true;
        bool adding;
        bool notAdding = true;
        Item selectedItem;

        //add item
        string itemName;
        decimal itemPrice;

        public ObservableCollection<Item> ItemList { get => itemList; set { SetProperty(ref itemList, value); } }
        public bool Editing { get => editing; set { SetProperty(ref editing, value); } }
        public bool NotEditing { get => notEditing; set { SetProperty(ref notEditing, value); } }
        public bool Adding { get => adding; set { SetProperty(ref adding, value); } }
        public bool NotAdding { get => notAdding; set { SetProperty(ref notAdding, value); } }
        public Item SelectedItem { get => selectedItem; set { SetProperty(ref selectedItem, value); } }

        //add item
        public string ItemName { get => itemName; set { SetProperty(ref itemName, value); } }
        public decimal ItemPrice { get => itemPrice; set { SetProperty(ref itemPrice, value); } }

        public ItemViewModel()
        {
            ivm = this;
            UpdateItemList();
        }

        public async void UpdateItemList()
        {
            List<Item> list = await TabsViewModel.tvm.Conn.Table<Item>().ToListAsync();
            
            ItemList = new ObservableCollection<Item>(list);
        }
    }
}
