﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleverShoppingList
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TabsPage();
            
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
