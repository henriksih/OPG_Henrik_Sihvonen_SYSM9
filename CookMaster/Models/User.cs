using CookMaster.MVVM;
using System.Collections.ObjectModel;

namespace CookMaster.Models
{
    public class User : ViewModelBase
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Country { get; set; }

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
