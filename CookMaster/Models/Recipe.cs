using CookMaster.MVVM;

namespace CookMaster.Models
{
    public class Recipe : ViewModelBase
    {
        private string? _title;
        public string? Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string? _ingredients;
        public string? Ingredients
        {
            get => _ingredients;
            set
            {
                if (_ingredients == value) return;
                _ingredients = value;
                OnPropertyChanged(nameof(Ingredients));
            }
        }

        private string? _instructions;
        public string? Instructions
        {
            get => _instructions;
            set
            {
                if (_instructions == value) return;
                _instructions = value;
                OnPropertyChanged(nameof(Instructions));
            }
        }

        private string? _category;
        public string? Category
        {
            get => _category;
            set
            {
                if (_category == value) return;
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        private DateOnly? _date;
        public DateOnly? Date
        {
            get => _date;
            set
            {
                if (_date == value) return;
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string? _createdBy;
        public string? CreatedBy
        {
            get => _createdBy;
            set
            {
                if (_createdBy == value) return;
                _createdBy = value;
                OnPropertyChanged(nameof(CreatedBy));
            }
        }

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
