using CookMaster.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CookMaster.Managers
{
    public class RecipeManager : INotifyPropertyChanged
    {
        private List<Recipe>? Recipes { get; set; }

        public RecipeManager? recipeManager;

        //Konstruktor
        public RecipeManager(UserManager userManager, User user)
        {
            Recipes = new List<Recipe>();
            SeedDefaultRecipes(user);

        }

        private void SeedDefaultRecipes(User user)
        {
            Recipes.Add(new Recipe(
                "Spaghetti Carbonara",
                "Spaghetti, Bacon, Lök, Äggula",
                "Koka spaghettin och stek upp resten, servera med rå äggula på toppen",
                "Middag",
                new DateOnly(2025, 10, 20),
                user.Username));
        }

        public void AddRecipe(string title, string ingredients, string instructions, string category, DateOnly date, string createdBy)
        {
            Recipes.Add(new Recipe(title, ingredients, instructions, category, date, createdBy));
        }

        public void RemoveRecipe(Recipe recipe)
        {

        }

        public List<Recipe>? GetAllRecipes()
        {
            return Recipes;
        }

        public List<Recipe>? GetByUser(User user)
        {
            return Recipes;
        }

        public void Filter(string criteria)
        {

        }

        public void UpdateRecipe(Recipe recipe)
        {

        }



        //Implementera INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string v) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));

    }
}
