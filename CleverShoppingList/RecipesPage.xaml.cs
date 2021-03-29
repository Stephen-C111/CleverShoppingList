using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
    }
}