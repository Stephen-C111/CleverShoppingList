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
        int maxAmount;
        decimal totalPrice;
        decimal maxPrice;


        public List<ListItem> ListItems { get => listItems; set { SetProperty(ref listItems, value); } }
        public int TotalAmount { get => totalAmount; set { SetProperty(ref totalAmount, value); } }
        public int MaxAmount { get => maxAmount; set { SetProperty(ref maxAmount, value); } }
        public decimal TotalPrice { get => totalPrice; set { SetProperty(ref totalPrice, value); } }
        public decimal MaxPrice { get => maxPrice; set { SetProperty(ref maxPrice, value); } }


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

            CountPrices();
        }

        public void CountPrices()
        {
            
            decimal total = 0;
            decimal max = 0;
            int amount = 0;

            foreach (ListItem i in ListItems)
            {
                if (i.Check)
                {
                    total += i.UseSale ?  i.SalePrice * i.Amount : i.Price * i.Amount;
                    amount++;
                }
                max += i.UseSale ? i.SalePrice * i.Amount : i.Price * i.Amount;
            }

            TotalPrice = total;
            MaxPrice = max;
            TotalAmount = amount;
            MaxAmount = ListItems.Count;
        }

    }
}
