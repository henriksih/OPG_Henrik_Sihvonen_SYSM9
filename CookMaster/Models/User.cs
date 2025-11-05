using CookMaster.MVVM;
using System.Collections.ObjectModel;

namespace CookMaster.Models
{
    public class User : ViewModelBase
    {
        private string? _username;
        public string? Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string? _securityQuestion;
        public string? SecurityQuestion
        {
            get => _securityQuestion;
            set { _securityQuestion = value; OnPropertyChanged(); }
        }

        private string? _securityAnswer;
        public string? SecurityAnswer
        {
            get => _securityAnswer;
            set { _securityAnswer = value; OnPropertyChanged(); }
        }


        private string? _country;
        public string? Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Recipe>? MyRecipeList;
    }
}
