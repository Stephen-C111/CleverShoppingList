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
    class AddItemViewModel : ViewModelBase
    {
        public static AddItemViewModel aivm;
        string name;
        decimal price;
        Priority itemPriority = Priority.Normal;
        ObservableCollection<Item> itemList = new ObservableCollection<Item>();

        public ObservableCollection<Item> ItemList { get => itemList; set { SetProperty(ref itemList, value); } }
        public string Name { get => name; set { SetProperty(ref name, value); } }
        public decimal Price { get => price; set { SetProperty(ref price, value); } }
        public Priority ItemPriority { get => itemPriority; set { SetProperty(ref itemPriority, value); } }

        public AddItemViewModel()
        {
            aivm = this;
        }

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

        public async void UpdateItemList()
        {
            List<Item> list = await TabsViewModel.tvm.Conn.Table<Item>().ToListAsync();

            ItemList = new ObservableCollection<Item>(list);
        }

        public async void SearchItemList(string query)
        {
            List<Item> list = await TabsViewModel.tvm.Conn.QueryAsync<Item>("SELECT * FROM Items WHERE Name LIKE '%" + query + "%'");

            ItemList = new ObservableCollection<Item>(list);
        }
    }
}
