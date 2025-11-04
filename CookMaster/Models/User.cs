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

        private string? _country;
        public string? Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }


        //public string? Username { get; set; }
        //public string? Password { get; set; }
        //public string? Country { get; set; }

        public ObservableCollection<Recipe>? MyRecipeList;

        //private readonly UserManager? _userManager;

        //public void ValidateLogin()
        //{
        //    if (_userManager.Login(Username, Password))
        //    {

        //    }
        //}


    }
}
