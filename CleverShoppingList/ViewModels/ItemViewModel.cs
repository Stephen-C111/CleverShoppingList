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
        List<Item> itemList = new List<Item>();
        public List<Item> ItemList { get => itemList; set { SetProperty(ref itemList, value); } }


        public ItemViewModel()
        {
            ivm = this;
            UpdateItemList();
        }

        public async void UpdateItemList()
        {
            var list = await TabsViewModel.tvm.Conn.Table<Item>().ToListAsync();
            
            ItemList = list;
        }
    }
}
