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
            Random r = new Random();
            //Create test items
                Item i = new Item("test item" + r.Next(1, 999), 4.99m);
                await TabsViewModel.tvm.Conn.InsertAsync(i);
                //var item = from x in TabsViewModel.tvm.Conn.Table<Item>()
                //           where x.Name == "test item"
                //           select x;
                //Item i2 = await item.FirstAsync();
                ListItem li = new ListItem(i.ID, Priority.High, -1);
                await TabsViewModel.tvm.Conn.InsertAsync(li);
            await ListViewModel.lvm.UpdateList();
        }
    }
}
