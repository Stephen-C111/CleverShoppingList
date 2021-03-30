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
    public partial class AddEditRecipe : ContentPage
    {
        public AddEditRecipe(int id)
        {
            InitializeComponent();

            if (id != -1) //-1 indicates a new recipe must be created.
            {
                Task.Run(() => AddEditRecipeViewModel.aervm.GetRecipe(id)).Wait();
            }
            else
            {
                AddEditRecipeViewModel.aervm.CurrentRecipe = new Recipe();
                TabsViewModel.tvm.Conn.InsertAsync(AddEditRecipeViewModel.aervm.CurrentRecipe);
            }
        }

        private async void NewItem_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Create Item?", "It will be added to your permanent item list for future shopping trips, and to your recipe.", "Yes", "No"))
            {
                Item i = new Item(AddEditRecipeViewModel.aervm.NewItemName, AddEditRecipeViewModel.aervm.NewItemPrice);
                await TabsViewModel.tvm.Conn.InsertAsync(i);
                ListItem li = new ListItem(i.ID, Priority.Normal, AddEditRecipeViewModel.aervm.CurrentRecipe.ID);
                await li.LinkToForeignItem();
                await TabsViewModel.tvm.Conn.InsertAsync(li);
                //Update the visuals for item list
                ItemViewModel.ivm.UpdateItemList();
                await AddEditRecipeViewModel.aervm.GetIngredients(AddEditRecipeViewModel.aervm.CurrentRecipe.ID);
                
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            await TabsViewModel.tvm.Conn.UpdateAsync(AddEditRecipeViewModel.aervm.CurrentRecipe);
            await RecipeViewModel.rvm.UpdateList();
            await Navigation.PopModalAsync();
        }
    }
}