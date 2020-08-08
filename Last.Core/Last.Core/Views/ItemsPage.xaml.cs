﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Last.Core.ViewModels;

namespace Last.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as ItemsViewModel).Navigation = Navigation;
            (BindingContext as ItemsViewModel).LoadItemsCommand.Execute(null);
        }
    }
}