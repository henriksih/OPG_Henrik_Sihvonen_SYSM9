using CookMaster.Managers;
using CookMaster.ViewModels;
using System;
using System.Windows;

namespace CookMaster.Views
{
    /// <summary>
    /// Interaction logic for RecipeDetailWindow.xaml
    /// </summary>
    public partial class RecipeDetailWindow : Window
    {
        public RecipeDetailWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object? sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is RecipeDetailWindowViewModel oldVm)
                oldVm.OnSaveRecipeSuccess -= Vm_OnSaveRecipeSuccess;

            if (e.NewValue is RecipeDetailWindowViewModel newVm)
                newVm.OnSaveRecipeSuccess += Vm_OnSaveRecipeSuccess;
        }

        private void Vm_OnSaveRecipeSuccess(object? sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Vm_OnSaveRecipeSuccess received - setting DialogResult = true");

            DialogResult = true; // Stänger fönstret
        }
    }
}
