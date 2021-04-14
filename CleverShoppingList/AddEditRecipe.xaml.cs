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
        int picked = 0;
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
                //Add the newly created Recipe into the database file.
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

                ItemNameEntry.Text = "";
                PriceEntry.Text = "0";
            }
        }

        private async void Finished_Clicked(object sender, EventArgs e)
        {
            decimal price = 0; //Tallies price of ingredients.
            foreach (ListItem x in AddEditRecipeViewModel.aervm.Ingredients)
            {
                price += x.Price * x.Amount;
            }
            AddEditRecipeViewModel.aervm.CurrentRecipe.Price = price;
            //Update the recipe to reflect the price of ingredients.
            await TabsViewModel.tvm.Conn.UpdateAsync(AddEditRecipeViewModel.aervm.CurrentRecipe);
            //Update the recipe list to ensure they're all displayed correctly.
            await RecipeViewModel.rvm.UpdateList();
            await Navigation.PopModalAsync();
        }

        private void AddFromList_Clicked(object sender, EventArgs e)
        {
            AddEditRecipeViewModel.aervm.ShowItemsList = true;
            AddEditRecipeViewModel.aervm.NoShowItemsList = false;
            AddEditRecipeViewModel.aervm.Editing = false;
        }

        private async void AddChoices_Clicked(object sender, EventArgs e)
        {
            //Add selected choices to the recipe.
            foreach (Item x in AddEditRecipeViewModel.aervm.ItemList)
            {
                if (x.Selected)
                {
                    //Create a new listitem with ownerID = to this recipe's ID. 
                    ListItem li = new ListItem(x.ID, Priority.Normal, AddEditRecipeViewModel.aervm.CurrentRecipe.ID);
                    //Get information about name and price from the Items list.
                    await li.LinkToForeignItem();
                    //Save the new listitem.
                    await TabsViewModel.tvm.Conn.InsertAsync(li);
                }
                x.Selected = false;
            }
            await AddEditRecipeViewModel.aervm.GetIngredients(AddEditRecipeViewModel.aervm.CurrentRecipe.ID);
            AddChoicesButton.IsEnabled = false;
            picked = 0;
            AddEditRecipeViewModel.aervm.ShowItemsList = false;
            AddEditRecipeViewModel.aervm.NoShowItemsList = true;
        }

        private void itemView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            Item i = itemView.SelectedItem as Item;
            
            //Keep track of picked items
            if (i.Selected)
            {
                if (picked > 0) //Just in case, don't allow picked to go below 0.
                {
                    picked--;
                }
                i.Selected = false;
            }
            else
            {
                picked++;
                i.Selected = true;
            }

            //only allow adding if > 0 items clicked.
            if (picked == 0)
            {
                AddChoicesButton.IsEnabled = false;
            }
            else
            {
                AddChoicesButton.IsEnabled = true;
            }
            
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
                AddEditRecipeViewModel.aervm.Editing = false;
            }
        }

        private void FinishEdit_Clicked(object sender, EventArgs e) //This button refers to the "Finish Editing" button when editing ingredients
        {
            TabsViewModel.tvm.Conn.UpdateAllAsync(AddEditRecipeViewModel.aervm.Ingredients);
            AddEditRecipeViewModel.aervm.Editing = false;
            AddEditRecipeViewModel.aervm.NotEditing = true;
        }

        private void CancelAddFromList_Clicked(object sender, EventArgs e)
        {
            foreach (Item x in AddEditRecipeViewModel.aervm.ItemList)
            {
                x.Selected = false;
                picked = 0;
            }

            AddEditRecipeViewModel.aervm.ShowItemsList = false;
            AddEditRecipeViewModel.aervm.NoShowItemsList = true;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PriceEntry.Text != "" && ItemNameEntry.Text != "")
            {
                NewItemButton.IsEnabled = true;
            }
            else
            {
                NewItemButton.IsEnabled = false;
            }
        }

        private void Search_Clicked(object sender, EventArgs e)
        {
            AddEditRecipeViewModel.aervm.SearchItemList(SearchEntry.Text);
        }
    }
}