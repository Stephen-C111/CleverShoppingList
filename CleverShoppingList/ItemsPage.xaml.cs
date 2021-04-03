using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;

namespace CleverShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
        }

        private void itemView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ItemViewModel.ivm.SelectedItem = itemView.SelectedItem as Item;
            ItemViewModel.ivm.Editing = true;
            ItemViewModel.ivm.NotEditing = false;
        }

        

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete Item?", "This action cannot be undone, and this item will not be included in your report for most expensive items.", "Yes", "No"))
            {
                await TabsViewModel.tvm.Conn.DeleteAsync(ItemViewModel.ivm.SelectedItem);
                ItemViewModel.ivm.Editing = false;
                ItemViewModel.ivm.NotEditing = true;
                ItemViewModel.ivm.UpdateItemList();
                //Updating twice ensures that deleted items do not appear in the program.
                await ListViewModel.lvm.UpdateList();
                await ListViewModel.lvm.UpdateList();
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            await TabsViewModel.tvm.Conn.UpdateAllAsync(ItemViewModel.ivm.ItemList);
            ItemViewModel.ivm.Editing = false;
            ItemViewModel.ivm.NotEditing = true;
            await ListViewModel.lvm.UpdateList();
        }

        private void AddNewItem_Clicked(object sender, EventArgs e)
        {
            ItemViewModel.ivm.Adding = true;
            ItemViewModel.ivm.NotAdding = false;
        }

        private async void SaveNewItem_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Create Item?", "You can add this new item to your shopping list at any time.", "Yes", "No")){
                Item i = new Item(ItemViewModel.ivm.ItemName, ItemViewModel.ivm.ItemPrice);
                await TabsViewModel.tvm.Conn.InsertAsync(i);
                ItemViewModel.ivm.Adding = false;
                ItemViewModel.ivm.NotAdding = true;
                ItemViewModel.ivm.UpdateItemList();
            }
            
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            ItemViewModel.ivm.Adding = false;
            ItemViewModel.ivm.NotAdding = true;
        }
    }
}