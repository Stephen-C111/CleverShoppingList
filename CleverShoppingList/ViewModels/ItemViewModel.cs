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
        List<Item> itemList = new List<Item>();


        List<Item> ItemList { get => itemList; set { SetProperty(ref itemList, value); } }
    }
}
