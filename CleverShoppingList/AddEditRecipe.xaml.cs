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
        public AddEditRecipe(int id, bool editing = false)
        {
            InitializeComponent();

            if (id != -1) //-1 indicates a new recipe must be created.
            {
                Task.Run(() => AddEditRecipeViewModel.aervm.GetRecipe(id)).Wait();
                Task.Run(() => AddEditRecipeViewModel.aervm.GetIngredients(AddEditRecipeViewModel.aervm.CurrentRecipe.ID)).Wait();
            }
            else
            {
                AddEditRecipeViewModel.aervm.CurrentRecipe = new Recipe();
                TabsViewModel.tvm.Conn.InsertAsync(AddEditRecipeViewModel.aervm.CurrentRecipe);
            }

            if (editing)
            {
                AddEditRecipeViewModel.aervm.Header = "Edit Recipe";
            }
            else
            {
                AddEditRecipeViewModel.aervm.Header = "Add Recipe";
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

        private async void Finished_Clicked(object sender, EventArgs e)
        {
            decimal price = 0;
            foreach (ListItem x in AddEditRecipeViewModel.aervm.Ingredients)
            {
                price += x.Price * x.Amount;
            }
            AddEditRecipeViewModel.aervm.CurrentRecipe.Price = price;
            await TabsViewModel.tvm.Conn.UpdateAsync(AddEditRecipeViewModel.aervm.CurrentRecipe);
            await RecipeViewModel.rvm.UpdateList();
            await Navigation.PopModalAsync();
        }

        private void AddFromList_Clicked(object sender, EventArgs e)
        {
            AddEditRecipeViewModel.aervm.ShowItemsList = true;
            AddEditRecipeViewModel.aervm.NoShowItemsList = false;
        }

        private async void AddChoices_Clicked(object sender, EventArgs e)
        {
            //Add selected choices to the recipe.
            foreach (Item x in AddEditRecipeViewModel.aervm.ItemList)
            {
                if (x.Selected)
                {
                    ListItem li = new ListItem(x.ID, Priority.Normal, AddEditRecipeViewModel.aervm.CurrentRecipe.ID);
                    await li.LinkToForeignItem();
                    await TabsViewModel.tvm.Conn.InsertAsync(li);
                }
                x.Selected = false;
            }
            await AddEditRecipeViewModel.aervm.GetIngredients(AddEditRecipeViewModel.aervm.CurrentRecipe.ID);
            AddEditRecipeViewModel.aervm.ShowItemsList = false;
            AddEditRecipeViewModel.aervm.NoShowItemsList = true;
        }

        private void itemView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Item i = itemView.SelectedItem as Item;
            i.Selected = !i.Selected;
        }

        private void ingredientList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            AddEditRecipeViewModel.aervm.SelectedIngredient = ingredientList.SelectedItem as ListItem;
            AddEditRecipeViewModel.aervm.Editing = true;
            AddEditRecipeViewModel.aervm.NotEditing = false;
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Remove Item?", "You can re-add it later, if you need to.", "Yes", "No"))
            {
                await TabsViewModel.tvm.Conn.DeleteAsync(AddEditRecipeViewModel.aervm.SelectedIngredient);
                await AddEditRecipeViewModel.aervm.GetIngredients(AddEditRecipeViewModel.aervm.CurrentRecipe.ID);
            }
        }

        private void FinishEdit_Clicked(object sender, EventArgs e)
        {
            TabsViewModel.tvm.Conn.UpdateAllAsync(AddEditRecipeViewModel.aervm.Ingredients);
            AddEditRecipeViewModel.aervm.Editing = false;
            AddEditRecipeViewModel.aervm.NotEditing = true;
        }
    }
}