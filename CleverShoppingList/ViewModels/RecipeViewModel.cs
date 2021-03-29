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
    class RecipeViewModel : ViewModelBase
    {
        public static RecipeViewModel rvm;

        ObservableCollection<Recipe> recipeList = new ObservableCollection<Recipe>();


        public ObservableCollection<Recipe> RecipeList { get => recipeList; set { SetProperty(ref recipeList, value); } }

        public RecipeViewModel()
        {
            rvm = this;
            Task.Run(() => UpdateList()).Wait();
        }

        public async Task UpdateList()
        {
            var qlist = from x in TabsViewModel.tvm.Conn.Table<Recipe>()
                        orderby x.Purchased descending
                        select x;
            var list = await qlist.ToListAsync();
            
            RecipeList = new ObservableCollection<Recipe>(list);
        }
    }
}
