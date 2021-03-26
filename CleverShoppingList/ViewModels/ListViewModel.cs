﻿using System;
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
        ListItem selectedItem;
        bool editing = false;


        public List<ListItem> ListItems { get => listItems; set { SetProperty(ref listItems, value); } }
        public int TotalAmount { get => totalAmount; set { SetProperty(ref totalAmount, value); } }
        public int MaxAmount { get => maxAmount; set { SetProperty(ref maxAmount, value); } }
        public decimal TotalPrice { get => totalPrice; set { SetProperty(ref totalPrice, value); } }
        public decimal MaxPrice { get => maxPrice; set { SetProperty(ref maxPrice, value); } }
        public ListItem SelectedItem { get => selectedItem; set { SetProperty(ref selectedItem, value); } }
        public bool Editing { get => editing; set { SetProperty(ref editing, value); } }

        public List<Priority> PriorityList
        {
            get
            {
                var array = Enum.GetValues(typeof(Priority));
                var list = new List<Priority>();
                list.AddRange((IEnumerable<Priority>)array);
                return list;
            }
        }

        public ListViewModel()
        {
            lvm = this;
            Task.Run(() => UpdateList()).Wait();
        }

        public async Task UpdateList()
        {
            var qlist = from x in TabsViewModel.tvm.Conn.Table<ListItem>()
                       orderby x.Priority descending
                       select x;
            var list = await qlist.ToListAsync();
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
