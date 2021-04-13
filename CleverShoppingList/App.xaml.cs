using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleverShoppingList
{
    public partial class App : Application
    {
        public static char preferredSymbol = '$';
        public static float salesTax = 8.25f;
        public App()
        {
            InitializeComponent();

            MainPage = new GreetingPage();
            
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
