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
        public RecipesPage()
        {
            InitializeComponent();
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddEditRecipe(-1)); //-1 indicates a new recipe should be created.
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

        private void AddToList_Clicked(object sender, EventArgs e)
        {

        }
    }
}