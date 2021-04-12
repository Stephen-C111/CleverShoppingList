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
    public class HelpSettingsViewModel : ViewModelBase
    {
        public static HelpSettingsViewModel hsvm;

        bool help = true;
        bool settings;
        bool noEditPass = true;
        bool newPass;
        bool requestPass;

        public bool Help { get => help; set { SetProperty(ref help, value); } }
        public bool Settings { get => settings; set { SetProperty(ref settings, value); } }
        public bool NoEditPass { get => noEditPass; set { SetProperty(ref noEditPass, value); } }
        public bool NewPass { get => newPass; set { SetProperty(ref newPass, value); } }
        public bool RequestPass { get => requestPass; set { SetProperty(ref requestPass, value); } }

        public HelpSettingsViewModel()
        {
            hsvm = this;
            
        }
    }
}
