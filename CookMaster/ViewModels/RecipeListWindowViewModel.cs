using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RecipeListWindowViewModel : ViewModelBase
    {
        public UserManager? UserManager { get; }
        public RecipeManager? RecipeManager { get; }

        private ICollectionView? _recipesView;
        public ICollectionView? RecipesView => _recipesView;

        private Recipe? _selectedRecipe;
        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set {
                if (_selectedRecipe == value) return;
                _selectedRecipe = value;
                OnPropertyChanged();
                OnSelectedRecipeChanged();
            }
        }

        private void OnSelectedRecipeChanged()
        {
            if (_selectedRecipe == null) return;
                        
            GetRecipeDetailsCommand?.Execute(null);
                        
        }

        // Expose current loggedin user
        public User? CurrentUser => UserManager?.GetLoggedin();

        public RecipeListWindowViewModel(UserManager userManager, RecipeManager recipeManager)
        {
            UserManager = userManager;
            RecipeManager = recipeManager;

            // Create a view over the shared collection (guard null)
            var source = RecipeManager?.Recipes ?? new ObservableCollection<Recipe>();
            _recipesView = CollectionViewSource.GetDefaultView(source);
            _recipesView.Filter = FilterByLoggedInUser;

            // Refresh the view when logged-in user changes
            if (UserManager != null)
            {
                UserManager.PropertyChanged += UserManager_PropertyChanged;
            }

            AddRecipeCommand = new RelayCommand(execute => AddRecipe(), canExecute => CanAdd());
            GetRecipeDetailsCommand = new RelayCommand(execute => GetRecipe(), canExecute => CanAdd());
            RemoveRecipeCommand = new RelayCommand(execute => RemoveRecipe(), canExecute => CanAdd());
            AboutCookMasterCommand = new RelayCommand(execute => AboutCookMaster(), canExecute => CanAdd());
            GetUserInfoCommand = new RelayCommand(execute => GetUserInfo(), canExecute => CanAdd());
        }

        private bool FilterByLoggedInUser(object item)
        {
            if (item is not Recipe r) return false;
            var logged = UserManager?.GetLoggedin();
            if (logged == null) return false; // or return true to show all when no user
            return string.Equals(r.CreatedBy, logged.Username, StringComparison.OrdinalIgnoreCase);
        }

        private void UserManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserManager.LoggedIn))
            {
                _recipesView?.Refresh();
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public ICommand AddRecipeCommand { get; }
        public ICommand GetRecipeDetailsCommand { get; }
        public ICommand RemoveRecipeCommand { get; }
        public ICommand AboutCookMasterCommand { get; }
        public ICommand GetUserInfoCommand { get; }

        public void AddRecipe()
        {
            var addRecipeWindow = new AddRecipeWindow();
            var recipeListWindow = Application.Current.MainWindow;
            if (recipeListWindow != null)
            {
                addRecipeWindow.Owner = recipeListWindow;
                recipeListWindow.Hide();
            }

            var result = addRecipeWindow.ShowDialog();

            // If you expect the dialog to return true when recipe added:
            if (result != true)
            {
                recipeListWindow?.Show();
                return;
            }

            // underlying collection already updated by AddRecipeWindowViewModel
            _recipesView?.Refresh();
            recipeListWindow?.Show();
        }

        public void GetRecipe() 
        {
            // Kontrollera att ett recept är valt
            if (SelectedRecipe == null) return;

            var recipeDetailWindow = new RecipeDetailWindow();
            //Göm RecipeListWindow medan RecipeDetailWindow är öppen
            var main = Application.Current.MainWindow;
            if (main != null)
            {
                recipeDetailWindow.Owner = main;
                main.Hide();
            }

            // Skapa ViewModel med det valda receptet och sätt som DataContext
            var vm = new RecipeDetailWindowViewModel(UserManager, RecipeManager, SelectedRecipe);
            recipeDetailWindow.DataContext = vm;

            var result = recipeDetailWindow.ShowDialog();

            if (result != true)
            {
                Application.Current.Shutdown();
                return;
            }
            //Om registreringen lyckades, visa RecipeListWindow igen
            main?.Show();
        }
        public void RemoveRecipe() { }
        public void AboutCookMaster() { }
        public void GetUserInfo() { UserManager?.GetLoggedin(); }
        public bool CanAdd() => true;
    }
}
