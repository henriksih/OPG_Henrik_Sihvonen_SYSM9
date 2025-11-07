using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
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

        // För att veta om ett recept är tillagt
        private bool _isAdded;
        public bool IsAdded
        {
            get => _isAdded;
            private set { _isAdded = value; OnPropertyChanged(); }
        }

        //Konstruktor
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
        //User koll behövs inte för det sätts av applikationen och kan inte ändras av användaren
        //Date är också satt och kan bar ändras via datepickern.



        public void AddRecipe()
        {
            // Stoppa om RecipeManager saknas
            if (RecipeManager == null)
                return;

            // Börja med en tom creator
            string creator = string.Empty;

            // Försök använd den inloggade användarens användarnamn
            if (UserManager != null)
            {
                var logged = UserManager.GetLoggedin();
                if (logged != null && !string.IsNullOrEmpty(logged.Username))
                {
                    creator = logged.Username;
                }
            }

            // Om ingen är inloggad, använd värdet i _createdBy (om det finns)
            if (string.IsNullOrEmpty(creator) && !string.IsNullOrEmpty(_createdBy))
            {
                creator = _createdBy;
            }

            // Som sista utväg, se till att creator inte blir null (använd tom sträng)
            if (string.IsNullOrEmpty(creator))
            {
                creator = string.Empty;
            }

            // Säkerställ att vi inte skickar null till RecipeManager (ersätt med tomma strängar)
            string title = _title ?? string.Empty;
            string ingredients = _ingredients ?? string.Empty;
            string instructions = _instructions ?? string.Empty;
            string category = _category ?? string.Empty;
            DateOnly date = _date;

            // Lägg till receptet
            RecipeManager.AddRecipe(title, ingredients, instructions, category, date, creator);

            IsAdded = true;
            OnSaveRecipeSuccess?.Invoke(this, EventArgs.Empty);
        }
    }
}

