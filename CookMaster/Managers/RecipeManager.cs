using CookMaster.Models;
using CookMaster.MVVM;
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
    public class RecipeManager : ViewModelBase
    {
        public ObservableCollection<Recipe>? Recipes { get; set; }

        public RecipeManager? recipeManager;

        //Konstruktor
        public RecipeManager(UserManager userManager, User user)
        {
            Recipes = new ObservableCollection<Recipe>();
            SeedDefaultRecipes(user);
            userManager.LoggedIn.MyRecipeList = Recipes;

        }
        
        // Tom konstruktor behövs för att få bort varning i App.xaml
        public RecipeManager()
        {
            Recipes = new ObservableCollection<Recipe>();
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

        public ObservableCollection<Recipe>? GetAllRecipes()
        {
            return Recipes;
        }

        public ObservableCollection<Recipe>? GetByUser(User user)
        {
            return Recipes;
        }

        public void Filter(string criteria)
        {

        }

        public void UpdateRecipe(string title, string ingredients, string instructions, string category, DateOnly date, string createdBy)
        {
            Recipes.Add(new Recipe(title, ingredients, instructions, category, date, createdBy));
        }



        ////Implementera INotifyPropertyChanged
        //public event PropertyChangedEventHandler? PropertyChanged;
        //private void OnPropertyChanged(string v) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));

    }
}
