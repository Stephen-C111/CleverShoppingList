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

        public List<ListItem> ListItems { get => listItems; set { SetProperty(ref listItems, value); } }

        public ListViewModel()
        {
            lvm = this;
            Task.Run(() => UpdateList()).Wait();
        }

        public async Task UpdateList()
        {
            var list = from i in TabsViewModel.tvm.Conn.Table<ListItem>()
                       where i.ID > 0
                       select i;
            ListItems = await list.ToListAsync();
            foreach (ListItem li in ListItems)
            {
                await li.LinkToForeignItem();
            }
        }
    }
}
