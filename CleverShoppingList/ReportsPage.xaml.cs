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

        private async void Monthly_Spending_Clicked(object sender, EventArgs e)
        {
            await ReportsViewModel.rpvm.GetSpendingForAllMonths();
            ReportsViewModel.rpvm.ExpensiveItems = false;
            ReportsViewModel.rpvm.MonthlySpending = true;
            ReportsViewModel.rpvm.SelectOption = false;
        }

        private async void DebugAddYearOfSpending_Clicked(object sender, EventArgs e)
        {
            Random r = new Random();
            for (int i = 0; i < 12; i++)
            {
                ArchivedTrip t = new ArchivedTrip(DateTime.Today.AddMonths(i), r.Next(200, 1001));
                await TabsViewModel.tvm.Conn.InsertAsync(t);
            }
        }
    }
}