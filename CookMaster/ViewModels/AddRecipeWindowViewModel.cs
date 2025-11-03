using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class AddRecipeWindowViewModel : ViewModelBase
    {
        public UserManager? UserManager { get; }
        public RecipeManager? RecipeManager { get; }

        private Recipe? _newRecipe;
        public Recipe? NewRecipe
        {
            get => _newRecipe;
            set { _newRecipe = value; OnPropertyChanged(); }
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

        private DateOnly _date = DateOnly.FromDateTime(DateTime.Today);
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

        // To know when a recipe was successfully added
        private bool _isAdded;
        public bool IsAdded
        {
            get => _isAdded;
            private set { _isAdded = value; OnPropertyChanged(); }
        }

        //Constructor
        public AddRecipeWindowViewModel(RecipeManager recipeManager, UserManager userManager)
        {
            RecipeManager = recipeManager;
            UserManager = userManager;

            if (UserManager != null)
            {
                UserManager.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(UserManager.LoggedIn))
                    {
                        OnPropertyChanged(nameof(CurrentUser));
                        // also update CreatedBy if you want it to follow the login immediately
                        CreatedBy = UserManager.GetLoggedin()?.Username;
                    }
                };
            }
            AddRecipeCommand = new RelayCommand(execute => AddRecipe(), canExecute => CanAdd());
        }

        public ICommand AddRecipeCommand { get; }


        public bool CanAdd() =>
           !string.IsNullOrWhiteSpace(Title)
            && !string.IsNullOrWhiteSpace(Ingredients)
            && !string.IsNullOrWhiteSpace(Instructions)
            && !string.IsNullOrWhiteSpace(Category);
            //&& !string.IsNullOrWhiteSpace(CreatedBy);


        public void AddRecipe()
        {
            if (RecipeManager == null) return;

            // Använd alltid inloggad användare om tillgänglig
            var creator = UserManager?.GetLoggedin()?.Username ?? _createdBy ?? string.Empty;

            RecipeManager.AddRecipe(
                _title ?? string.Empty,
                _ingredients ?? string.Empty,
                _instructions ?? string.Empty,
                _category ?? string.Empty,
                _date,
                creator);

            IsAdded = true;
            OnSaveRecipeSuccess?.Invoke(this, EventArgs.Empty);
        }
    }
}

