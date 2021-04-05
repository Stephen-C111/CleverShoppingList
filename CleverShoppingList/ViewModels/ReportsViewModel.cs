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
        ObservableCollection<SpendingMonth> months = new ObservableCollection<SpendingMonth>();
        SpendingMonth current;
        SpendingMonth last;

        public bool SelectOption { get => selectOption; set { SetProperty(ref selectOption, value); } }
        public bool MonthlySpending { get => monthlySpending; set { SetProperty(ref monthlySpending, value); } }
        public bool ExpensiveItems { get => expensiveItems; set { SetProperty(ref expensiveItems, value); } }
        public ObservableCollection<Item> ItemList { get => itemList; set { SetProperty(ref itemList, value); } }
        public ObservableCollection<SpendingMonth> Months { get => months; set { SetProperty(ref months, value); } }
        public SpendingMonth Current { get => current; set { SetProperty(ref current, value); } }
        public SpendingMonth Last { get => last; set { SetProperty(ref last, value); } }

        public ReportsViewModel()
        {
            rpvm = this;

            Task.Run(() => GetSpendingForMainDisplay()).Wait();
        }

        public async Task<SpendingMonth> ConvertTripsIntoMonth(DateTime date, decimal comparePrice = 0m)
        {
            decimal total = 0;
            DateTime nextMonth = date.AddMonths(1);

            //need to "pretty up" the Month value if it isn't double digits, because "4" is not the same as "04".
            string m1 = "";
            string m2 = "";
            if (date.Month < 10)
            {
                m1 = "0" + date.Month;
            }
            else
            {
                m1 = "" + date.Month;
            }
            if (nextMonth.Month < 10)
            {
                m2 = "0" + nextMonth.Month;
            }
            else
            {
                m2 = "" + nextMonth.Month;
            }

            //Find all ArchivedTrips for date.Month and date.Year
            List<ArchivedTrip> TripList = await TabsViewModel.tvm.Conn.QueryAsync<ArchivedTrip>
                ("SELECT ID, TripDate, Cost FROM ArchivedTrips WHERE TripDate BETWEEN '" + date.Year + "-" + m1 + "-01 00:00:00' AND '" + nextMonth.Year + "-" + m2 + "-01 00:00:00'");

            //Add all trip costs to a single monthly cost.
            foreach (ArchivedTrip trip in TripList)
            {
                total += trip.Cost;
            }

            return new SpendingMonth(date, total, comparePrice);

        }

        public async void GetSpendingForMainDisplay()
        {
            DateTime firstOfThisMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            Last = await ConvertTripsIntoMonth(firstOfThisMonth.AddMonths(-1));
            Current = await ConvertTripsIntoMonth(firstOfThisMonth, Last.Spent);
            
        }

        public async Task GetSpendingForAllMonths()
        {
            //First, figure out which months are actually linked to ArchivedTrips.
            var tripsList = await TabsViewModel.tvm.Conn.QueryAsync<ArchivedTrip>("SELECT ID, TripDate, Cost FROM ArchivedTrips ORDER BY TripDate");
            List<YearMonth> uniqueMonths = new List<YearMonth>();
            Months.Clear();
            foreach (ArchivedTrip trip in tripsList)
            {
                bool unique = true;
                YearMonth ym = new YearMonth(trip.TripDate.Year, trip.TripDate.Month);
                foreach (YearMonth x in uniqueMonths)
                {
                     if (ym.year == x.year && ym.month == x.month)
                    {
                        unique = false;
                    }
                    
                }
                if (unique) { uniqueMonths.Add(ym); }
            }
            //Then, for each unique month, create a SpendingMonth and add it to an observablecollection
            foreach (YearMonth y in uniqueMonths)
            {
                if (Months.Count == 0)
                {
                    Months.Add(await ConvertTripsIntoMonth(new DateTime(y.year, y.month, 1)));
                }
                else
                {
                    Months.Add(await ConvertTripsIntoMonth(new DateTime(y.year, y.month, 1), Months[Months.Count - 1].Spent));
                }
                
            }
        }

        public async Task GetItemsByExpense()
        {
            ItemList = new ObservableCollection<Item>(await TabsViewModel.tvm.Conn.QueryAsync<Item>("SELECT ID, Name, Price, Purchased, TotalCost FROM Items ORDER BY TotalCost DESC"));
        }
    }

    struct YearMonth { 
        
        public int year; public int month;
        
        public YearMonth(int year, int month)
        {
            this.year = year;
            this.month = month;
        }
    }
}
