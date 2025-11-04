using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
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
            set
            {
                if (_selectedRecipe != value)
                {
                    _selectedRecipe = value;
                    OnPropertyChanged(nameof(SelectedRecipe));

                    // Notifiera kommandot att dess CanExecute-status har ändrats
                    if (GetRecipeDetailsCommand is RelayCommand relayCommand)
                    {
                        relayCommand.RaiseCanExecuteChanged();
                    }

                }
                //if (GetRecipeDetailsCommand is RelayCommand detailsCmd)
                //    detailsCmd.RaiseCanExecuteChanged();
                //if (RemoveRecipeCommand is RelayCommand removeCmd)
                //    removeCmd.RaiseCanExecuteChanged();
            }

            //get => _selectedRecipe;
            //set {
            //    if (_selectedRecipe == value) return;
            //    _selectedRecipe = value;
            //    OnPropertyChanged();
            //    OnSelectedRecipeChanged();
            //}
        }

        private void OnSelectedRecipeChanged()
        {
            if (_selectedRecipe == null) return;

            GetRecipeDetailsCommand?.Execute(null);

        }

        //Expose current loggedin user
        public User? CurrentUser => UserManager?.GetLoggedin();

        public RecipeListWindowViewModel()
        {

        }

        public RecipeListWindowViewModel(UserManager userManager, RecipeManager recipeManager)
        {
            UserManager = userManager;
            RecipeManager = recipeManager;

            // Skapa en gemensam lista först
            var source = RecipeManager?.Recipes ?? new ObservableCollection<Recipe>();
            _recipesView = CollectionViewSource.GetDefaultView(source);
            _recipesView.Filter = FilterByLoggedInUser;

            // Se till att receptlistan uppdateras när recept läggs till
            if (RecipeManager?.Recipes != null)
            {
                RecipeManager.Recipes.CollectionChanged += (s, e) =>
                {
                    //Filtret sätts igen och uppdaterar UI
                    _recipesView?.Refresh();
                };
            }

            // Refresh the view when logged-in user changes
            if (UserManager != null)
            {
                UserManager.PropertyChanged += UserManager_PropertyChanged;
            }

            AddRecipeCommand = new RelayCommand(execute => AddRecipe(), canExecute => CanAdd());
            GetRecipeDetailsCommand = new RelayCommand(execute => GetRecipe(), canExecute => CanDo());
            RemoveRecipeCommand = new RelayCommand(execute => RemoveRecipe(), canExecute => CanDo());
            AboutCookMasterCommand = new RelayCommand(execute => AboutCookMaster(), canExecute => CanAdd());
            GetUserInfoCommand = new RelayCommand(execute => GetUserInfo(), canExecute => CanAdd());
            LogOutCommand = new RelayCommand(execute => LogOut(), canExecute => true);
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
        public ICommand LogOutCommand { get; }

        // Event triggas när VM vill 
        //public event EventHandler? RequestLogout;


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

            // Visa recipeListWindow om sparningen inte går bra:
            if (result != true)
            {
                recipeListWindow?.Show();
                return;
            }

            // Annars uppdatera listan och visa recipeListWindow igen
            _recipesView?.Refresh();
            recipeListWindow?.Show();
        }
        public event EventHandler<Recipe?>? RequestOpenRecipeDetail;
        public void GetRecipe() 
        {
            // Kontrollera att ett recept är valt
            //if (SelectedRecipe == null) return;
            //RequestOpenRecipeDetail?.Invoke(this, SelectedRecipe);
            //else
            //{
            //    CanDo();
            //}

            //var recipeDetailWindow = new RecipeDetailWindow();
            ////Göm RecipeListWindow medan RecipeDetailWindow är öppen
            //var main = Application.Current.MainWindow;
            //if (main != null)
            //{
            //    recipeDetailWindow.Owner = main;
            //    main.Hide();
            //}

            //// Skapa ViewModel med det valda receptet och sätt som DataContext
            //var vm = new RecipeDetailWindowViewModel(UserManager, RecipeManager, SelectedRecipe);
            //recipeDetailWindow.DataContext = vm;

            //var result = recipeDetailWindow.ShowDialog();

            //if (result != true)
            //{
            //    Application.Current.Shutdown();
            //    return;
            //}
            ////Om registreringen lyckades, visa RecipeListWindow igen
            //main?.Show();

            //Gamla implementationen nedan

            //// Kontrollera att ett recept är valt
            //if (SelectedRecipe == null) return;


            //var recipeDetailWindow = new RecipeDetailWindow();
            ////Göm RecipeListWindow medan RecipeDetailWindow är öppen
            //var main = Application.Current.MainWindow;
            //if (main != null)
            //{
            //    recipeDetailWindow.Owner = main;
            //    main.Hide();
            //}

            //// Skapa ViewModel med det valda receptet och sätt som DataContext
            //var vm = new RecipeDetailWindowViewModel(UserManager, RecipeManager, SelectedRecipe);
            //recipeDetailWindow.DataContext = vm;

            //var result = recipeDetailWindow.ShowDialog();

            //if (result != true)
            //{
            //    //Application.Current.Shutdown();
            //    main?.Show();
            //    return;
            //}
            ////Om registreringen lyckades, visa RecipeListWindow igen
            //main?.Show();

            //Nya varianten:

            // Säkerställ att ett recept är selekterat
            if (SelectedRecipe == null) return;

            // Öppna ett RecipeDetailWindow för det valda receptet
            RequestOpenRecipeDetail?.Invoke(this, SelectedRecipe);
        }
        public void RemoveRecipe() { }
        public void AboutCookMaster() 
        {
            var aboutWindow = new AboutWindow();
            var main = Application.Current.MainWindow;
            if (main != null)
            {
                aboutWindow.Owner = main;
                //main.Hide();
            }

            var result = aboutWindow.ShowDialog();

            // Visa recipelistwindow oavsett hur det går
            //main?.Show();
        }
        public void GetUserInfo() 
        { 
            UserManager?.GetLoggedin();

            var userInfoWindow = new UserInfoWindow();
            var main = Application.Current.MainWindow;
            if (main != null)
            {
                userInfoWindow.Owner = main;
                //main.Hide();
            }

            var result = userInfoWindow.ShowDialog();
        }

        public void LogOut()
        {
            UserManager?.Logout();
           
            var current = Application.Current.MainWindow;
            if (current == null)
            {
                //Skapa ett mainWindow om det inte finns
                var fallback = new MainWindow();
                // ensure the new main's VM password is cleared
                if (fallback.DataContext is MainWindowViewModel fallbackVm)
                    fallbackVm.Password = string.Empty;
                Application.Current.MainWindow = fallback;
                fallback.Show();
                return;
            }

            var owner = current.Owner;
            if (owner != null)
            {
                // Clear password on the original MainWindow's VM before showing it
                if (owner.DataContext is MainWindowViewModel ownerVm)
                    ownerVm.Password = string.Empty;
                // Gör MainWindow till Owner, visa det och stäng RecipeListWindow
                Application.Current.MainWindow = owner;
                owner.Show();
                current.Close();
            }
            else
            {
                // Om ingen owner finns, skapa och visa ett nytt MainWindow
                var newMain = new MainWindow();
                // ensure the new main's VM password is cleared
                if (newMain.DataContext is MainWindowViewModel newMainVm)
                    newMainVm.Password = string.Empty;

                Application.Current.MainWindow = newMain;
                newMain.Show();
                current.Close();
            }
        }
        public bool CanAdd() => true;
        public bool CanDo()
        {
            if (SelectedRecipe != null)
            {

                return true;
            }
            else return false;
        }
    }
}
