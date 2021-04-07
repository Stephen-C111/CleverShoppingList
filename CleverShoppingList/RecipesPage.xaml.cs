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
    public partial class RecipesPage : ContentPage
    {

        bool lockAdd = false;

        public RecipesPage()
        {
            InitializeComponent();
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddEditRecipe(-1)); //-1 indicates a new recipe should be created.
            RecipeViewModel.rvm.Editing = false;
            RecipeViewModel.rvm.NotEditing = true;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            RecipeViewModel.rvm.SelectedRecipe = recipeListView.SelectedItem as Recipe;
            RecipeViewModel.rvm.Editing = true;
            RecipeViewModel.rvm.NotEditing = false;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            RecipeViewModel.rvm.Editing = false;
            RecipeViewModel.rvm.NotEditing = true;
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddEditRecipe(RecipeViewModel.rvm.SelectedRecipe.ID, true));
            RecipeViewModel.rvm.Editing = false;
            RecipeViewModel.rvm.NotEditing = true;
        }

        private async void AddToList_Clicked(object sender, EventArgs e)
        {
            if (lockAdd) //prevent the user from spamming the "add to list" button
            {
                return;
            }
            lockAdd = true;
            //Query purpose: Get every ListItem whose OwnerID links to the currently selected recipe.
            var query = from x in TabsViewModel.tvm.Conn.Table<ListItem>()
                        where x.OwnerID == RecipeViewModel.rvm.SelectedRecipe.ID
                        select x;
            var list = await query.ToListAsync();
            foreach (ListItem li in list)
            {
                //Create a new ListItem that is intended to store the name of the recipe it was created from. 
                ListItem newLi = new ListItem(li.ForeignID, Priority.Normal, -1, li.Amount, RecipeViewModel.rvm.SelectedRecipe.Name);

                await TabsViewModel.tvm.Conn.InsertAsync(newLi);
            }
            //Update the number of times this recipe has been added to the shopping list, which is used in sorting.
            RecipeViewModel.rvm.SelectedRecipe.Purchased++;
            //Update the recipe to save the new Purchased value.
            await TabsViewModel.tvm.Conn.UpdateAsync(RecipeViewModel.rvm.SelectedRecipe);
            //Ensure that the shopping list updates to show the new ListItems.
            await ListViewModel.lvm.UpdateList();
            RecipeViewModel.rvm.Editing = false;
            RecipeViewModel.rvm.NotEditing = true;
            await DisplayAlert("Recipe Added.", "Your recipe has been added to the shopping list.", "OK");
            lockAdd = false;
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete Recipe?", "Delete " + RecipeViewModel.rvm.SelectedRecipe.Name + "? This data cannot be recovered once deleted.", "Yes", "No"))
            {
                await TabsViewModel.tvm.Conn.DeleteAsync(RecipeViewModel.rvm.SelectedRecipe);
                await RecipeViewModel.rvm.UpdateList();
                RecipeViewModel.rvm.Editing = false;
                RecipeViewModel.rvm.NotEditing = true;
            }
        }
    }
}