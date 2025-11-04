using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookMaster.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        //Implementera INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
