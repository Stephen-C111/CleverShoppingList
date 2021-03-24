using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;

namespace CleverShoppingList
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        //This class is used to easily update information on screen.
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
