using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Last.Core.Views;
using Last.Core.ViewModels;
using Last.Core.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Last.Core
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Messaging messaging = new Messaging();
            ItemsViewModel itemsViewModel = new ItemsViewModel(messaging);
            MainPage = new NavigationPage(new ItemsPage());
            MainPage.BindingContext = itemsViewModel;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
