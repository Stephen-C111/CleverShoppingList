using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;

namespace CleverShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GreetingPage : ContentPage
    {
        public GreetingPage()
        {
            InitializeComponent();
        }

        private async void Continue_Clicked(object sender, EventArgs e)
        {
            if (GreetingViewModel.gvm.PasswordExists)
            {
                if (PassEntry.Text == await SecureStorage.GetAsync("app_pass"))
                {
                    PassEntry.Text = "";
                    await Navigation.PushModalAsync(new TabsPage());
                }
                else
                {
                    //Post-Evaluation: Consider adding a biometric authentication feature.
                    await DisplayAlert("Incorrect Password.", "The password you entered was incorrect. If you don't remember the password, there is unfortunately no way to recover it, as you were warned. You may reinstall the application to reset the app, losing your data, if you don't think you'll remember it.", "OK");
                }
            }
            else
            {
                await Navigation.PushModalAsync( new TabsPage() );
            }
        }
    }
}