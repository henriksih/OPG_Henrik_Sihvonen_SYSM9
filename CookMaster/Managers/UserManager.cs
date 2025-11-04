using CookMaster.Models;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Managers
{
    public class UserManager : ViewModelBase
    //Hantera users och samla i dem en lista skapa default och Admin usrar.

    {
        
        private readonly List<User> _users = new List<User>();

        private User? _loggedIn;
        public User? LoggedIn
        {
            get => _loggedIn;

            private set
            {
                _loggedIn = value; 
                OnPropertyChanged(nameof(LoggedIn)); 
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        //Konstruktor
        public UserManager()
        {
            _users = new List<User>();
            SeedDefaultUsers();
        }


        public bool IsAuthenticated => LoggedIn != null;

        private void SeedDefaultUsers()
        {
            _users.Add(new User
            {
                Username = "user",
                Password = "a", //"password",
                Country = "Sweden",
                MyRecipeList = new System.Collections.ObjectModel.ObservableCollection<Recipe>()
            });
            _users.Add(new Admin
            {
                Username = "admin",
                Password = "a", //"password",
                Country = "Norge",
                MyRecipeList = new System.Collections.ObjectModel.ObservableCollection<Recipe>()
            });
        }

        public bool Login(string username, string password)
        {
            foreach (var u in _users)
            {
                if (string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)
                    && u.Password == password)
                {
                    LoggedIn = u;
                    return true;
                }
            }
            return false;
        }

        public void Logout()
        {
            LoggedIn = null;
        }

        public void Register(string user, string password, string country)
        {
            _users.Add(new User { Username = user, Password = password, Country = country });
        }

        public User FindUser(string name)
        {
            foreach(User u in _users)
            {
                if(u.Username == name)
                {
                    return u;
                }
            }
            return null;
        }

        public void ChangePassword(string user, string password)
        {

        }

        public User? GetLoggedin()
        {
            return _loggedIn;
        }
    }
}

    
