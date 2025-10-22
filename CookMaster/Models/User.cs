using CookMaster.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    public class User
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Country { get; set; }

        private readonly UserManager? _userManager;

        //public void ValidateLogin()
        //{
        //    if (_userManager.Login(Username, Password))
        //    {

        //    }
        //}


    }
}
