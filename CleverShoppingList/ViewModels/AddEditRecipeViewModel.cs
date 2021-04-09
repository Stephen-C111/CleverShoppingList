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
        ListItem selectedIngredient;
        ObservableCollection<ListItem> ingredients = new ObservableCollection<ListItem>();
        string newItemName;
        decimal newItemPrice;
        bool showItemsList;
        bool noShowItemsList = true;
        ObservableCollection<Item> itemList = new ObservableCollection<Item>();
        string header;
        bool editing;
        bool notEditing;

        public Recipe CurrentRecipe { get => currentRecipe; set { SetProperty(ref currentRecipe, value); } }
        public ListItem SelectedIngredient { get => selectedIngredient; set { SetProperty(ref selectedIngredient, value); } }
        public ObservableCollection<ListItem> Ingredients { get => ingredients; set { SetProperty(ref ingredients, value); } }
        public string NewItemName { get => newItemName; set { SetProperty(ref newItemName, value); } }
        public decimal NewItemPrice { get => newItemPrice; set { SetProperty(ref newItemPrice, value); } }
        public bool ShowItemsList { get => showItemsList; set { SetProperty(ref showItemsList, value); } }
        public bool NoShowItemsList { get => noShowItemsList; set { SetProperty(ref noShowItemsList, value); } }
        public ObservableCollection<Item> ItemList { get => itemList; set { SetProperty(ref itemList, value); } }
        public string Header { get => header; set { SetProperty(ref header, value); } }
        public bool Editing { get => editing; set { SetProperty(ref editing, value); } }
        public bool NotEditing { get => notEditing; set { SetProperty(ref notEditing, value); } }

        public AddEditRecipeViewModel()
        {
            aervm = this;

            UpdateItemList();
        }

        public async void UpdateItemList()
        {
            var list = await TabsViewModel.tvm.Conn.Table<Item>().ToListAsync();

            ItemList = new ObservableCollection<Item>(list);
        }

        public async void SearchItemList(string query)
        {
            List<Item> list = await TabsViewModel.tvm.Conn.QueryAsync<Item>("SELECT * FROM Items WHERE Name LIKE '%" + query + "%'");

            ItemList = new ObservableCollection<Item>(list);
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
