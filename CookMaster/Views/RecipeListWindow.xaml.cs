using CookMaster.Managers;
using CookMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookMaster.Views
{
    /// <summary>
    /// Interaction logic for RecipeListWindow.xaml
    /// </summary>
    public partial class RecipeListWindow : Window
    {
        public RecipeListWindow()
        {
            InitializeComponent();
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];
            var vm = new RecipeListWindowViewModel(userManager, recipeManager);
            DataContext = vm;

            vm.RequestOpenRecipeDetail += (s, recipe) =>
            {
                if (recipe == null) return;
                var detailWindow = new RecipeDetailWindow
                {
                    Owner = this,
                    DataContext = new RecipeDetailWindowViewModel(vm.UserManager, vm.RecipeManager, recipe)
                };
                this.Hide();
                detailWindow.ShowDialog();
                this.Show();
            };

            //if (vm != null)
            //{
            //    vm.PropertyChanged += Vm_PropertyChanged;
            //}
        }


        // New constructor to receive managers explicitly (preferred for runtime)
        public RecipeListWindow(UserManager userManager, RecipeManager recipeManager)
        {
            InitializeComponent();
            DataContext = new RecipeListWindowViewModel(userManager, recipeManager);
        }

        private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(RecipeListWindowViewModel.SelectedRecipe)) return;
            if (DataContext is not RecipeListWindowViewModel vm) return;

            var selected = vm.SelectedRecipe;
            if (selected == null) return;

            // Öppna RecipeDetailWindow
            var detailWindow = new RecipeDetailWindow
            {
                Owner = this,
                DataContext = new RecipeDetailWindowViewModel(vm.UserManager, vm.RecipeManager, selected)

            };

            // Skapa recipeDetailWindowViewModel med rätt indata och sätt DataContext
            //detailWindow.DataContext = new RecipeDetailWindowViewModel(vm.UserManager, vm.RecipeManager, selected);

            // Göm nuvarande RecipeListWindow
            this.Hide();
            detailWindow.ShowDialog();
            this.Show();

            // sätt selectedRecipe till null så fönstret inte återöppnas med en gång
            vm.SelectedRecipe = null;
        }

        private void Receptdetaljer_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RecipeListWindowViewModel)?.GetRecipe();
        }
    }
}
