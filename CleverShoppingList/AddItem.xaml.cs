using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CleverShoppingList.ViewModels;
using CleverShoppingList.Models;

namespace CleverShoppingList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItem : ContentPage
    {
        
        public AddItem()
        {
            InitializeComponent();
            
        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Create Item?", "It will be added to your permanent item list for future shopping trips, and to your temporary shopping list.", "Yes", "No"))
            {
                Item i = new Item(AddItemViewModel.aivm.Name, AddItemViewModel.aivm.Price);
                await TabsViewModel.tvm.Conn.InsertAsync(i);
                ListItem li = new ListItem(i.ID, AddItemViewModel.aivm.ItemPriority, -1);
                await TabsViewModel.tvm.Conn.InsertAsync(li);
                //Update the visuals for both lists
                await ListViewModel.lvm.UpdateList();
                ItemViewModel.ivm.UpdateItemList();
                await Navigation.PopModalAsync();
            }
            
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Leave Screen?", "Your choices will not be saved.", "Yes", "No"))
            {
                await Navigation.PopModalAsync();
            }
        }

        private async void AddSelected_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Finished Selecting?", "Your choices will be added to the list. Anything you typed in the top section will not be saved.", "Yes", "No"))
            {
                foreach (Item i in AddItemViewModel.aivm.ItemList)
                {
                    if (i.Selected)
                    {
                        //All selected items will be added to the list, and marked Selected = false.
                        i.Selected = false;
                        ListItem li = new ListItem(i.ID, Priority.Normal, -1);
                        await TabsViewModel.tvm.Conn.InsertAsync(li);
                    }
                }
                await ListViewModel.lvm.UpdateList();
                
                await Navigation.PopModalAsync();
            }
        }

        private void itemView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Item i = itemView.SelectedItem as Item;
            i.Selected = !i.Selected;
        }
    }
}