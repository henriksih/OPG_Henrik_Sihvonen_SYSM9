using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class AboutWindowViewModel
    {
        public string About { get; } = (@"Detta är en informationstext om företaget och appen Cook Master
Det började som ett hobbyprojekt och är nu tillgängligt för alla
Du registrerar dig och kan sedan lägga till dina recept och ordna dem i kategorier");
        public AboutWindowViewModel()
        {
            CloseCommand = new RelayCommand(execute => Close(), canExecute => CanClose());
        }

        public ICommand? CloseCommand { get; }

        public event EventHandler? IfClosed;
        public void Close()
        {
            IfClosed?.Invoke(this, EventArgs.Empty);
        }

        public bool CanClose()
        {
            return true;
        }
    }
}
