using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
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
            set
            {
                _selectedRecipe = value;
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

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            private set { _isEditing = value; OnPropertyChanged(); }
        }


        public event EventHandler? OnSaveRecipeSuccess;

        //Konstruktorer
        public RecipeDetailWindowViewModel(UserManager userManager, RecipeManager recipeManager)
        {
            RecipeManager = recipeManager;
            UserManager = userManager;

            EditRecipeCommand = new RelayCommand(_ => BeginEdit(), _ => !IsEditing);
            SaveRecipeCommand = new RelayCommand(_ => SaveRecipe(), _ => IsEditing);
            CancelEditCommand = new RelayCommand(_ => CancelEdit());
        }

        public RecipeDetailWindowViewModel(UserManager userManager, RecipeManager recipeManager, Recipe selectedRecipe)
            : this(userManager, recipeManager)
        {
            SelectedRecipe = selectedRecipe;
            RecipeManager = recipeManager;
            UserManager = userManager;

            EditRecipeCommand = new RelayCommand(_ => BeginEdit(), _ => !IsEditing);
            SaveRecipeCommand = new RelayCommand(_ => SaveRecipe(), _ => IsEditing);
            CancelEditCommand = new RelayCommand(_ => CancelEdit());
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
            IsEditing = false;
        }


        private void BeginEdit()
        {
            IsEditing = true;
            // Kommandon som är beroende av om IsEditing är true
            (EditRecipeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (SaveRecipeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            //(CancelEditCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
        public ICommand EditRecipeCommand { get; }

        public ICommand SaveRecipeCommand { get; }
        public ICommand CancelEditCommand { get; }

        public void SaveRecipe()
        {
            System.Diagnostics.Debug.WriteLine($"SaveRecipe called, SelectedRecipe={(SelectedRecipe?.Title ?? "<null>")}");
            if (SelectedRecipe == null) return;

            SelectedRecipe.Title = Title;
            SelectedRecipe.Ingredients = Ingredients;
            SelectedRecipe.Instructions = Instructions;
            SelectedRecipe.Category = Category;
            SelectedRecipe.Date = Date;
            SelectedRecipe.CreatedBy = CreatedBy;
            //RecipeManager.UpdateRecipe(Title, Ingredients, Instructions, Category, Date, CreatedBy);

            IsEditing = false;
            (EditRecipeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (SaveRecipeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (CancelEditCommand as RelayCommand)?.RaiseCanExecuteChanged();

            System.Diagnostics.Debug.WriteLine("Raising OnSaveRecipeSuccess");

            // Berätta att registreringen var framgångsrik
            OnSaveRecipeSuccess?.Invoke(this, EventArgs.Empty);
        }



        private void CancelEdit()
        {
            // Revert properties from the model
            CopySelectedToProperties();
            (EditRecipeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (SaveRecipeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (CancelEditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            IfClosed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? IfClosed;
    }
}
