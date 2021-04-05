using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;

namespace CleverShoppingList
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static MainPage mp;
        Random r;

        public MainPage()
        {
            mp = this;
            InitializeComponent();
            r = new Random();
        }

        private void listItemView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListViewModel.lvm.SelectedItem = listItemView.SelectedItem as ListItem;
            ListViewModel.lvm.CountPrices();
            ListViewModel.lvm.Editing = true;
            ListViewModel.lvm.NotEditing = false;
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddItem());
        }

        private async void X_Clicked(object sender, EventArgs e)
        {
            //Delete the listitem with a warning dialog.
            if (await DisplayAlert("Remove Item?", "You can re-add the item later, if needed.", "Yes", "No"))
            {
                await TabsViewModel.tvm.Conn.DeleteAsync(ListViewModel.lvm.SelectedItem);
                await ListViewModel.lvm.UpdateList();
                ListViewModel.lvm.Editing = false;
                ListViewModel.lvm.NotEditing = true;
            }
        }

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
        }

        private async void CheckOut_Clicked(object sender, EventArgs e)
        {
            //Dialog confirming the user's action.
            if (await DisplayAlert("Check out?", "All checked items will be removed from the list and archived for your reports. You can re-add these items again with the drop-down menu while adding items.", "Yes", "No"))
            {
                ArchivedTrip t = new ArchivedTrip(DateTime.Today);
                //Create an ArchivedTrip and use its id to connect ArchivedItems to it.
                await TabsViewModel.tvm.Conn.InsertAsync(t);
                foreach (ListItem i in ListViewModel.lvm.ListItems)
                {
                    if (i.InCart)
                    {
                        i.InCart = false;
                       // Archive the ListItem into an ArchivedItem.
                            decimal price = i.Price;
                        ArchivedItem a = new ArchivedItem(t.ID, i.Name, i.Amount, price * i.Amount);
                        await TabsViewModel.tvm.Conn.InsertAsync(a);
                        t.Cost += a.Price;
                        //Update the foreign item with x bought and x spent.
                        var foreignList = from f in TabsViewModel.tvm.Conn.Table<Item>()
                                          where i.ForeignID == f.ID
                                          select f;
                        Item foreign = await foreignList.FirstAsync();
                        foreign.TotalCost += a.Price;
                        foreign.Purchased += a.Amount;
                        await TabsViewModel.tvm.Conn.UpdateAsync(foreign);
                        //Delete the ListItem.
                            await TabsViewModel.tvm.Conn.DeleteAsync(i);
                    }
                }
                await TabsViewModel.tvm.Conn.UpdateAsync(t);
                //update the visuals for both lists.

                await ListViewModel.lvm.UpdateList();
                ItemViewModel.ivm.UpdateItemList();
            }
            
        }

        private async void StopEditing_Clicked(object sender, EventArgs e)
        {
            await TabsViewModel.tvm.Conn.UpdateAllAsync(ListViewModel.lvm.ListItems);
            ListViewModel.lvm.Editing = false;
            ListViewModel.lvm.NotEditing = true;
            
            //await ListViewModel.lvm.UpdateList();
        }

        private void Stepper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ListViewModel.lvm.Editing)
            {

               
                ListViewModel.lvm.CountPrices();
            }




        }

        

        private async void DebugAdd_Clicked(object sender, EventArgs e)
        {
            //This is debugging code made to help test adding items in rapid succession, or large groups.
            Item i = new Item("Test Item " + r.Next(50, 1000), 4.99m);
            await TabsViewModel.tvm.Conn.InsertAsync(i);
            ListItem li = new ListItem(i.ID, Priority.Low, -1, r.Next(2, 11));
            await TabsViewModel.tvm.Conn.InsertAsync(li);
            //Update the visuals for both lists
            await ListViewModel.lvm.UpdateList();
            ItemViewModel.ivm.UpdateItemList();
            
        }
    }
}
