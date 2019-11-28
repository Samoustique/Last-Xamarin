using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Last.Core.ViewModels;
using System;

namespace Last.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(ItemDetailViewModel itemDetailViewModel)
        {
            InitializeComponent();
            itemDetailViewModel.Navigation = Navigation;
            itemDetailViewModel.PictureChoice += OnPictureChoice;
            BindingContext = itemDetailViewModel;
        }

        private async void OnPictureChoice()
        {
            if (BindingContext is ItemDetailViewModel itemDetailViewModel)
            {
                string choice = await DisplayActionSheet("Picture:", "Cancel", null, itemDetailViewModel.PictureChoiceCamera, itemDetailViewModel.PictureChoiceBrowse);
                itemDetailViewModel.PictureChoiceDone(choice);
            }
        }
    }
}