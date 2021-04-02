using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CleverShoppingList.Models;

namespace CleverShoppingList.ViewModels
{
    class TabsViewModel : ViewModelBase
    {
        public static TabsViewModel tvm;
        SQLiteAsyncConnection conn;

        public SQLiteAsyncConnection Conn { get => conn; }

        public TabsViewModel()
        {
            tvm = this;
            InitializeDatabaseOrCreate();
        }

        async void InitializeDatabaseOrCreate()
        {
            //The path to the database is created and connected to locally.
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = System.IO.Path.Combine(path, "CleverListDB.db3");
            conn = new SQLiteAsyncConnection(path);

            //Create tables if non-existant
            await Conn.RunInTransactionAsync(conn =>
            {
                conn.CreateTable<Item>();
                conn.CreateTable<ListItem>();
                conn.CreateTable<Recipe>();
                conn.CreateTable<ArchivedItem>();
                conn.CreateTable<ArchivedTrip>();
            });


            
           
        }
    }
}
