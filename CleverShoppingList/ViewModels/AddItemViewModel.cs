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

        public ObservableCollection<Item> ItemList { get => ItemViewModel.ivm.ItemList; }
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

        
    }
}
