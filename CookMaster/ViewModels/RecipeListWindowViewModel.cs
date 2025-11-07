using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.Views;
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
            }
        }

        private void OnSelectedRecipeChanged()
        {
            if (_selectedRecipe == null) return;

            GetRecipeDetailsCommand?.Execute(null);

        }

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

            // Ladda om när user ändras
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

            // kolla om receptet tillhör usern
            return RecipeManager?.IsVisibleTo(r, logged) ?? false;
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
            // Säkerställ att ett recept är selekterat
            if (SelectedRecipe == null) return;

            // Öppna ett RecipeDetailWindow för det valda receptet
            RequestOpenRecipeDetail?.Invoke(this, SelectedRecipe);
        }
        public void RemoveRecipe()
        {
            if (SelectedRecipe == null || UserManager == null)
                return;

            if (CurrentUser == null)
                return;

            if (CurrentUser is Admin admin && admin.IsAdmin)
            {
                // Ta bort från den sammanställda listan
                if (RecipeManager?.Recipes != null && RecipeManager.Recipes.Contains(SelectedRecipe))
                {
                    RecipeManager.Recipes.Remove(SelectedRecipe);
                }

                // Uppdatera listan
                _recipesView?.Refresh();
                return;
            }

            //'Vanliga' users kan bara ta bort från sin egen lista
            if (string.Equals(SelectedRecipe.CreatedBy, CurrentUser.Username, StringComparison.OrdinalIgnoreCase))
            {
                // Ta bort från den egna listan om möjligt
                CurrentUser.MyRecipeList?.Remove(SelectedRecipe);
                // Och även från den gemensamma...
                RecipeManager?.Recipes?.Remove(SelectedRecipe);

                // Uppdatera listan
                _recipesView?.Refresh();
                return;
            }
        }
        public void AboutCookMaster()
        {
            var aboutWindow = new AboutWindow();
            var main = Application.Current.MainWindow;

            if (main != null)
            {
                aboutWindow.Owner = main;
            }

            var result = aboutWindow.ShowDialog();
        }
        public void GetUserInfo()
        {
            UserManager?.GetLoggedin();

            var userInfoWindow = new UserInfoWindow();
            var main = Application.Current.MainWindow;
            if (main != null)
            {
                userInfoWindow.Owner = main;
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
                // Säkerställ att det nya MinWindowVMs lösenord nollställs
                if (fallback.DataContext is MainWindowViewModel fallbackVm)
                    fallbackVm.Password = string.Empty;
                Application.Current.MainWindow = fallback;
                fallback.Show();
                return;
            }

            var owner = current.Owner;
            if (owner != null)
            {
                // Töm lösenordet i MainWindowVM
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
                // kolla att nya MainWindowVMs lösen är tomt
                if (newMain.DataContext is MainWindowViewModel newMainVm)
                    newMainVm.Password = string.Empty;

                Application.Current.MainWindow = newMain;
                newMain.Show();
                current.Close();
            }
        }
        public bool CanAdd() => true;
        //kolla att ett recept är valt innan Receptdetaljer eller tabort knapparna kan användas
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
