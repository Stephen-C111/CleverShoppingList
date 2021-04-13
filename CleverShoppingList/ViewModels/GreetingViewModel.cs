using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;
using System.Threading.Tasks;

namespace CleverShoppingList.ViewModels
{
    class GreetingViewModel : ViewModelBase
    {
        public static GreetingViewModel gvm;
        SQLiteAsyncConnection conn;
        bool passwordExists = false;
        bool noPassword = true;

        public bool PasswordExists { get => passwordExists; set { SetProperty(ref passwordExists, value); } }
        public bool NoPassword { get => noPassword; set { SetProperty(ref noPassword, value); } }

        public GreetingViewModel()
        {
            gvm = this;

            InitializeDatabaseOrCreate();

            
        }


        async void InitializeDatabaseOrCreate()
        {
            //The path to the database is created and connected to locally.
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = System.IO.Path.Combine(path, "CleverListDB.db3");
            conn = new SQLiteAsyncConnection(path, false);

            //Create tables if non-existant
            await conn.RunInTransactionAsync(conn =>
            {
                conn.CreateTable<Item>();
                conn.CreateTable<ListItem>();
                conn.CreateTable<Recipe>();
                conn.CreateTable<ArchivedItem>();
                conn.CreateTable<ArchivedTrip>();
            });

            if (await SecureStorage.GetAsync("app_pass") == "" || await SecureStorage.GetAsync("app_pass") == null)
            {
                PasswordExists = false;
                NoPassword = true;
            }
            else
            {
                PasswordExists = true;
                NoPassword = false;
            }


        }
    }
}
