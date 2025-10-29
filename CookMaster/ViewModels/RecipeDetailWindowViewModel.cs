using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    internal class RecipeDetailWindowViewModel : ViewModelBase
    {
        public UserManager? UserManager { get; }
        public RecipeManager? RecipeManager { get; }

        private Recipe? _selectedRecipe;
        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set { _selectedRecipe = value; 
                OnPropertyChanged();
                CopySelectedToProperties();
            }
        }


        private string? _title;
        public string? Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }
        private string? _ingredients;
        public string? Ingredients
        {
            get => _ingredients;
            set { _ingredients = value; OnPropertyChanged(); }
        }

        private string? _instructions;
        public string? Instructions
        {
            get => _instructions;
            set { _instructions = value; OnPropertyChanged(); }
        }

        private string? _category;
        public string? Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        private DateOnly _date;
        public DateOnly Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        private string? _createdBy;
        public string? CreatedBy
        {
            get => _createdBy;
            set { _createdBy = value; OnPropertyChanged(); }
        }

        public User? CurrentUser => UserManager?.GetLoggedin();

        public event EventHandler? OnSaveRecipeSuccess;

        //Konstruktorer
        public RecipeDetailWindowViewModel(UserManager userManager, RecipeManager recipeManager)
        {
            RecipeManager = recipeManager;
            UserManager = userManager;
        }

        public RecipeDetailWindowViewModel(UserManager userManager, RecipeManager recipeManager, Recipe selectedRecipe)
            : this(userManager, recipeManager)
        {
            SelectedRecipe = selectedRecipe;
        }


        private void CopySelectedToProperties()
        {
            if (SelectedRecipe == null) return;

            Title = SelectedRecipe.Title;
            Ingredients = SelectedRecipe.Ingredients;
            Instructions = SelectedRecipe.Instructions;
            Category = SelectedRecipe.Category;
            Date = SelectedRecipe.Date ?? default;
            CreatedBy = SelectedRecipe.CreatedBy;
        }

        public ICommand EditRecipeCommand { get; }

        public ICommand SaveRecipeCommand { get; }

        public void SaveRecipe()
        {
            if (SelectedRecipe != null)
            {
                SelectedRecipe.Title = Title;
                SelectedRecipe.Ingredients = Ingredients;
                SelectedRecipe.Instructions = Instructions;
                SelectedRecipe.Category = Category;
                SelectedRecipe.Date = Date;
                SelectedRecipe.CreatedBy = CreatedBy;
                RecipeManager.UpdateRecipe(Title, Ingredients, Instructions, Category, Date, CreatedBy);
            }

            // Berätta att registreringen var framgångsrik
            OnSaveRecipeSuccess?.Invoke(this, EventArgs.Empty);
        }




    }


}
