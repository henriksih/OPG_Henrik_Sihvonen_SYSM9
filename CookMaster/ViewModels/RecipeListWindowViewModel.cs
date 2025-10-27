using CookMaster.Managers;
using CookMaster.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RecipeListWindowViewModel : INotifyPropertyChanged
    {
        public UserManager? UserManager { get; }
        public RecipeManager? RecipeManager { get; }
        //Get access to the recipie list to show in the window
        public List<Recipe>? Recipes
        {
            get => RecipeManager.Recipes;
            }

        private Recipe? _selectedRecipe;
        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set { _selectedRecipe = value; OnPropertyChanged(); }
        }


        private string _title;
        public string Title 
        { 
            get => _title;
            set { _title = value; }
        }
        private string _ingredients;
        public string Ingredients
        {
            get => _ingredients;
            set { _ingredients = value; }
        }

        private string _instructions;
        public string Instructions
        {
            get => _instructions;
            set { _instructions = value; }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set { _category = value; }
        }

        private DateOnly _date;
        public DateOnly Date
        {
            get => _date;
            set { _date = value; }
        }

        private string _createdBy;
        public string CreatedBy
        {
            get => _createdBy;
            set { _createdBy = value; }
        }


        public RecipeListWindowViewModel(UserManager userManager, RecipeManager recipeManager)
        {
            UserManager = userManager;
            RecipeManager = recipeManager;
            AddRecipeCommand = new RelayCommand(execute => AddRecipe(), canExecute => CanAdd());
            GetRecipeDetailsCommand = new RelayCommand(execute => GetRecipe(), canExecute => CanAdd());
            RemoveRecipeCommand = new RelayCommand(execute => RemoveRecipe(), canExecute => CanAdd());
            AboutCookMasterCommand = new RelayCommand(execute => AboutCookMaster(), canExecute => CanAdd());
            GetUserInfoCommand = new RelayCommand(execute => GetUserInfo(), canExecute => CanAdd());

        }



        public ICommand AddRecipeCommand { get; }
        public ICommand GetRecipeDetailsCommand { get; }
        public ICommand RemoveRecipeCommand { get; }
        public ICommand AboutCookMasterCommand { get; }
        public ICommand GetUserInfoCommand { get; }


        public void AddRecipe()
        {
            RecipeManager.AddRecipe(Title, Ingredients, Instructions, Category, Date, CreatedBy);
        }
        public void GetRecipe()
        {
            
        }
        public void RemoveRecipe()
        {

        }
        public void AboutCookMaster()
        {

        }
        public void GetUserInfo()
        {

        }
        public bool CanAdd()
        {
            return true;
        }








        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
