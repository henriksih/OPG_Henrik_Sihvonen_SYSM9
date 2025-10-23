using CookMaster.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Managers
{
    internal class RecipeManager : INotifyPropertyChanged
    {
        public List<Recipe>? Recipes { get; set; }

        public RecipeManager? recipeManager;

        public void AddRecipe(Recipe recipe)
        {

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
