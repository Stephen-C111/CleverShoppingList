using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;

namespace CleverShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
        }

        private async void ExpensiveItems_Clicked(object sender, EventArgs e)
        {
            await ReportsViewModel.rpvm.GetItemsByExpense();
            ReportsViewModel.rpvm.ExpensiveItems = true;
            ReportsViewModel.rpvm.MonthlySpending = false;
            ReportsViewModel.rpvm.SelectOption = false;
        }

        private void Monthly_Spending_Clicked(object sender, EventArgs e)
        {

            ReportsViewModel.rpvm.ExpensiveItems = false;
            ReportsViewModel.rpvm.MonthlySpending = true;
            ReportsViewModel.rpvm.SelectOption = false;
        }
    }
}