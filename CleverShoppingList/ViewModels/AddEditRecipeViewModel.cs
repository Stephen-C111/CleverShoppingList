using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CleverShoppingList.Models;
using CleverShoppingList.ViewModels;
using System.Threading.Tasks;

namespace CleverShoppingList.ViewModels
{
    class AddEditRecipeViewModel : ViewModelBase
    {
        public static AddEditRecipeViewModel aervm;

        Recipe currentRecipe;
        ObservableCollection<ListItem> ingredients = new ObservableCollection<ListItem>();
        string newItemName;
        decimal newItemPrice;

        public Recipe CurrentRecipe { get => currentRecipe; set { SetProperty(ref currentRecipe, value); } }
        public ObservableCollection<ListItem> Ingredients { get => ingredients; set { SetProperty(ref ingredients, value); } }
        public string NewItemName { get => newItemName; set { SetProperty(ref newItemName, value); } }
        public decimal NewItemPrice { get => newItemPrice; set { SetProperty(ref newItemPrice, value); } }

        public AddEditRecipeViewModel()
        {
            aervm = this;
            
            
        }

        public async Task GetRecipe(int id)
        {
            var query = from r in TabsViewModel.tvm.Conn.Table<Recipe>()
                        where r.ID == id
                        select r;
            CurrentRecipe = await query.FirstAsync();
        }

        public async Task GetIngredients(int id)
        {
            var query = from i in TabsViewModel.tvm.Conn.Table<ListItem>()
                        where i.OwnerID == id
                        select i;
            Ingredients = new ObservableCollection<ListItem>(await query.ToListAsync());
        }

        public async Task SaveRecipe()
        {
            await TabsViewModel.tvm.Conn.UpdateAsync(CurrentRecipe);
            await TabsViewModel.tvm.Conn.UpdateAllAsync(Ingredients);
        }
    }
}
