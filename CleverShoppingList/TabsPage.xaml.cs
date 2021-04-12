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
    public partial class TabsPage : TabbedPage
    {
        public TabsPage()
        {
            InitializeComponent();
            

            //Create the pages and then set them as children to the main tab page.
            MainPage main = new MainPage();
            main.Title = "Shopping List";

            ItemsPage items = new ItemsPage();
            items.Title = "Items";

            RecipesPage recipes = new RecipesPage();
            recipes.Title = "Recipes";

            ReportsPage reports = new ReportsPage();
            reports.Title = "Reports";

            HelpSettingsPage help = new HelpSettingsPage();
            help.Title = "Help/Settings";

            //Options page should go here with a help section.

            Children.Add(main);
            Children.Add(items);
            Children.Add(recipes);
            Children.Add(reports);
            Children.Add(help);
        }
    }
}