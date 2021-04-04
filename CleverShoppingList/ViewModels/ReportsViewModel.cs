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
    class ReportsViewModel : ViewModelBase
    {
        public static ReportsViewModel rpvm;

        bool selectOption = true;
        bool monthlySpending;
        bool expensiveItems;
        ObservableCollection<Item> itemList = new ObservableCollection<Item>();

        public bool SelectOption { get => selectOption; set { SetProperty(ref selectOption, value); } }
        public bool MonthlySpending { get => monthlySpending; set { SetProperty(ref monthlySpending, value); } }
        public bool ExpensiveItems { get => expensiveItems; set { SetProperty(ref expensiveItems, value); } }
        public ObservableCollection<Item> ItemList { get => itemList; set { SetProperty(ref itemList, value); } }

        public ReportsViewModel()
        {
            rpvm = this;
        }

        public void GetSpendingForThisMonth()
        {

        }

        public void GetSpendingForAllMonths()
        {

        }

        public async Task GetItemsByExpense()
        {
            ItemList = new ObservableCollection<Item>(await TabsViewModel.tvm.Conn.QueryAsync<Item>("SELECT ID, Name, Price, Purchased, TotalCost FROM Items ORDER BY TotalCost DESC"));
        }
    }
}
