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
        public MainPage()
        {
            InitializeComponent();
        }

        private void listItemView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Open edit dialog
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddItem());

            //Random r = new Random();
            ////Create test items
            //    Item i = new Item("test item" + r.Next(1, 999), 4.99m);
            //    await TabsViewModel.tvm.Conn.InsertAsync(i);
            //    
            //    ListItem li = new ListItem(i.ID, Priority.High, -1);
            //    await TabsViewModel.tvm.Conn.InsertAsync(li);
            ////Update the visuals for both lists
            //await ListViewModel.lvm.UpdateList();
            //ItemViewModel.ivm.UpdateItemList();
        }

        private void X_Clicked(object sender, EventArgs e)
        {
            //Delete the listitem with a warning dialog.
        }

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            
        }

        private async void CheckOut_Clicked(object sender, EventArgs e)
        {
            //Dialog confirming the user's action.
            if (await DisplayAlert("Check out?", "All checked items will be removed from the list and archived for your reports. You can re-add these items again with the drop-down menu while adding items.", "Yes", "No"))
            {
                    ArchivedTrip t = new ArchivedTrip();
                    //Create an ArchivedTrip and use its id to connect ArchivedItems to it.
                    await TabsViewModel.tvm.Conn.InsertAsync(t);
                    foreach (ListItem i in ListViewModel.lvm.ListItems)
                    {
                        if (i.Check)
                        {
                            //Archive the ListItem into an ArchivedItem.
                            decimal price = i.UseSale ? i.SalePrice : i.Price;
                            ArchivedItem a = new ArchivedItem(t.ID, i.Name, i.Amount, price);
                            await TabsViewModel.tvm.Conn.InsertAsync(a);
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
                    //update the visuals for both lists.
                    await ListViewModel.lvm.UpdateList();
                    ItemViewModel.ivm.UpdateItemList();
            }
            
        }
    }
}
