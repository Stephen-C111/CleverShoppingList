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
            //Find all ArchivedTrips for date.Month and date.Year
            List<ArchivedTrip> TripList = await TabsViewModel.tvm.Conn.QueryAsync<ArchivedTrip>
                ("SELECT ID, Date, Cost FROM ArchivedTrips "/*WHERE Date >= Date(" + date.Year + "-" + date.Month +"-01)" +
                " AND Date < Date(" + nextMonth.Year + "-" + nextMonth.Month + "-01)"*/);
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

        public void GetSpendingForAllMonths()
        {
            //First, figure out which months are actually linked to ArchivedTrips.

            //Then, for each unique month, create a SpendingMonth and add it to an observablecollection
        }

        public async Task GetItemsByExpense()
        {
            ItemList = new ObservableCollection<Item>(await TabsViewModel.tvm.Conn.QueryAsync<Item>("SELECT ID, Name, Price, Purchased, TotalCost FROM Items ORDER BY TotalCost DESC"));
        }
    }
}