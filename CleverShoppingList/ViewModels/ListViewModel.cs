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
    class ListViewModel : ViewModelBase
    {
        public static ListViewModel lvm;
        List<ListItem> listItems = new List<ListItem>();
        int totalAmount;
        decimal totalPrice;


        public List<ListItem> ListItems { get => listItems; set { SetProperty(ref listItems, value); } }
        public int TotalAmount { get => totalAmount; set { SetProperty(ref totalAmount, value); } }
        public decimal TotalPrice { get => totalPrice; set { SetProperty(ref totalPrice, value); } }


        public ListViewModel()
        {
            lvm = this;
            Task.Run(() => UpdateList()).Wait();
        }

        public async Task UpdateList()
        {
            var list = await TabsViewModel.tvm.Conn.Table<ListItem>().ToListAsync();
            foreach (ListItem li in list)
            {
                await li.LinkToForeignItem();
            }
            ListItems = list;
        }
    }
}
