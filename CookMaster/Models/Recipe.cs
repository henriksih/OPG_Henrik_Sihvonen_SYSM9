using CookMaster.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    public class Recipe
    {
        public string? Title { get; set; }
        public string? Ingredients { get; set; }
        public string? Instructions { get; set; }

        public string? Category { get; set; }

        public DateOnly? Date { get; set; }

        public string? CreatedBy { get; set; }

        //private readonly UserManager? _userManager;

        //Konstuktor

        public Recipe(string title, string ingredients, string instructions, string category, DateOnly date, string createdBy)
        {
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            Category = category;
            Date = date;
            CreatedBy = createdBy;
        }
    }


}
