using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Last.Core.ViewModels;

namespace Last.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage(NewItemViewModel newItemViewModel)
        {
            InitializeComponent();
            newItemViewModel.Navigation = Navigation;
            BindingContext = newItemViewModel;
        }
    }
}