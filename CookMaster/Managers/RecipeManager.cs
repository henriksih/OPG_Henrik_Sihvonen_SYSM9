using CookMaster.Models;
using CookMaster.MVVM;
using System.Collections.ObjectModel;
using System.Windows;

namespace CookMaster.Managers
{
    public class RecipeManager : ViewModelBase
    {
        public ObservableCollection<Recipe>? Recipes { get; set; }

        public RecipeManager? recipeManager;

        public UserManager? userManager;

        //Konstruktor
        public RecipeManager()
        {
            userManager = (UserManager)Application.Current.Resources["UserManager"];
            Recipes = new ObservableCollection<Recipe>();
            SeedDefaultRecipes();
        }

        private void SeedDefaultRecipes()
        {
            var creator = userManager?.FindUser("user")?.Username ?? "unknown";
            Recipes?.Add(new Recipe(
                "Spaghetti Carbonara",
                "Spaghetti, Bacon, Lök, Äggula",
                "Koka spaghettin och stek upp resten, servera med rå äggula på toppen",
                "Middag",
                new DateOnly(2025, 10, 20),
                creator));
            Recipes?.Add(new Recipe(
               "Pizza Margherita",
               "Mjöl, ägg, vatten, lök, tomat, ost",
               "Rör ihop mjöl ägg och vatten och forma pizzan. Bryn löken lätt och i med tomat," +
               " mixa och sprid på pizzan på med ost och grädda i ugn 15 min",
               "Middag",
               new DateOnly(2025, 10, 20),
               creator));
        }

        public void AddRecipe(string title, string ingredients, string instructions, string category, DateOnly date, string createdBy)
        {
            Recipes?.Add(new Recipe(title, ingredients, instructions, category, date, createdBy));
            if (userManager?.LoggedIn != null)
                userManager.LoggedIn.MyRecipeList = Recipes;
        }

        public bool IsVisibleTo(Recipe recipe, User? user)
        {
            if (recipe == null) return false;
            if (user == null) return true; // visa alla om ingen user finns
            // Admin usern ser allt
            if (user is Admin) return true;
            // annars visa bara CreatedBy userns recept skippa koll på stor/liten bokstav
            return string.Equals(recipe.CreatedBy, user.Username, StringComparison.OrdinalIgnoreCase);
        }
    }
}