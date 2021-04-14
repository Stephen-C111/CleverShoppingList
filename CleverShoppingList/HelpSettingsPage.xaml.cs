using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;

namespace CleverShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpSettingsPage : ContentPage
    {
        public HelpSettingsPage()
        {
            InitializeComponent();

            CheckForPass();
        }

        async void CheckForPass()
        {
            PasswordSpan.Text = await SecureStorage.GetAsync("app_pass") == "" || await SecureStorage.GetAsync("app_pass") == null ? "No" : "Yes";
        }

        private void Settings_Clicked(object sender, EventArgs e)
        {
            HelpSettingsViewModel.hsvm.Help = false;
            HelpSettingsViewModel.hsvm.Settings = true;
        }

        private void Help_Clicked(object sender, EventArgs e)
        {
            HelpSettingsViewModel.hsvm.Help = true;
            HelpSettingsViewModel.hsvm.Settings = false;
        }

        private async void EditPass_Clicked(object sender, EventArgs e)
        {
            if (await SecureStorage.GetAsync("app_pass") != null && await SecureStorage.GetAsync("app_pass") != "")
            {
                //Request previous password first.
                HelpSettingsViewModel.hsvm.NewPass = false;
                HelpSettingsViewModel.hsvm.NoEditPass = false;
                HelpSettingsViewModel.hsvm.RequestPass = true;
            }
            else
            {
                //Allow to create a new password, and warn the user about the risk of losing the password.
                HelpSettingsViewModel.hsvm.NewPass = true;
                HelpSettingsViewModel.hsvm.NoEditPass = false;
                HelpSettingsViewModel.hsvm.RequestPass = false;
                await DisplayAlert("Warning!", "This application does not connect to any external services. This means that if you ever lose your password, you will be locked out of the app and lose your data. It is suggested that you store the password in a safe location. Please only use this feature if you are comfortable with this risk, as it will be irreversible.", "OK");
            }
        }

        private async void ConfirmPass_Clicked(object sender, EventArgs e)
        {
            if (RequestPassEntry.Text == await SecureStorage.GetAsync("app_pass"))
            {
                await DisplayAlert("Password Accepted.", "If you do decide to change your password now, please make sure the new one is saved in a secure place.", "OK");
                HelpSettingsViewModel.hsvm.NewPass = true;
                HelpSettingsViewModel.hsvm.NoEditPass = false;
                HelpSettingsViewModel.hsvm.RequestPass = false;
            }
            else
            {
                await DisplayAlert("Incorrect Password.", "Please try again.", "OK");
            }
        }

        private async void ConfirmNewPass_Clicked(object sender, EventArgs e)
        {
            if (NewPassEntry.Text == ConfirmNewPassEntry.Text && NewPassEntry.Text != "" && NewPassEntry.Text != null)
            {
                if (await DisplayAlert("Warning!", "This password and any locked data is non-recoverable, make sure you write it down in a safe place. Do you still want to apply this new password?", "Yes", "No"))
                {
                    try
                    {
                        await SecureStorage.SetAsync("app_pass", NewPassEntry.Text);
                        HelpSettingsViewModel.hsvm.NewPass = false;
                        HelpSettingsViewModel.hsvm.NoEditPass = true;
                        HelpSettingsViewModel.hsvm.RequestPass = false;
                        await DisplayAlert("Password Confirmed.", "You will be prompted for this password at app launch from now on.", "OK");
                        CheckForPass();
                    }
                    catch
                    {
                        await DisplayAlert("Error!", "Something went wrong adding your password. Try to use normal alpha-numerical symbols. (a-z A-Z 0-9)", "OK");
                        //To ensure the password isn't corrupted somehow, which would lock the user out permanently:
                        await SecureStorage.SetAsync("app_pass", "");
                        CheckForPass();
                    }
                    
                }
            }
            else
            {
                await DisplayAlert("Invalid input.", "The fields are either empty, or they do not match.", "OK");
                NewPassEntry.Text = "";
                ConfirmNewPassEntry.Text = "";
            }
        }

        private async void DisablePass_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Disable Password?", "You and potentially unauthorized users will no longer need to use a password to open the app.", "Yes", "No"))
            {
                await SecureStorage.SetAsync("app_pass", "");
                HelpSettingsViewModel.hsvm.NewPass = false;
                HelpSettingsViewModel.hsvm.NoEditPass = true;
                HelpSettingsViewModel.hsvm.RequestPass = false;
                CheckForPass();
            }
        }
    }
}