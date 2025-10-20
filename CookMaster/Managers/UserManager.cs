using CookMaster.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Managers
{
    internal class UserManager : INotifyPropertyChanged
    //Hantera users och samla i dem en lista skapa default user och Admin usrar.

    {
        private User? _currentUser;
        private readonly List<User> _users = new List<User>();

        //Konstruktor
        public UserManager()
        {
            _users = new List<User>();
            SeedDefaultUsers();
        }


        public User? CurrentUser
        {
            get => _currentUser;

            private set
            {
                _currentUser = value; 
                OnPropertyChanged(nameof(CurrentUser)); 
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public bool IsAuthenticated => CurrentUser != null;

        private void SeedDefaultUsers()
        {
            //_users.Add(new User { Username = "admin", Password = "password", DisplayName = "Administrator", Role = "Admin" });
            _users.Add(new User { Username = "user", Password = "password", Country = "Sweden" });
        }

        public bool Login(string username, string password)
        {
            foreach (var u in _users)
            {
                if (string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)
                    && u.Password == password)
                {
                    CurrentUser = u;
                    return true;
                }
            }
            return false;
        }

        public void Logout()
        {
            CurrentUser = null;
        }


        //Implementera INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string v) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        
    }
}

    
