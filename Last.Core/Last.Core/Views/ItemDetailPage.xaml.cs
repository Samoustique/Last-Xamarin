using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Last.Core.Models;
using Last.Core.ViewModels;

namespace Last.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}