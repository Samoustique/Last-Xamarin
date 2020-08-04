using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Last.Core.Views;
using Last.Core.ViewModels;
using Last.Core.Services;
using System.Reflection;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Last.Core
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var itemsDataStore = new ItemsDataStore(AssemblyProductVersion);
            
            string bbdPath = string.Empty;
            var alreadyExists = DependencyService.Get<IAppliPathGetterService>()?.GetOrCreateFile(itemsDataStore.BbdFilename, out bbdPath);
            itemsDataStore.BbdPath = bbdPath;
            if (alreadyExists ?? false)
            {
                itemsDataStore.Deserialize();
            }
            else
            {
                itemsDataStore.Serialize();
            }

            ItemsViewModel itemsViewModel = new ItemsViewModel(itemsDataStore);
            MainPage = new NavigationPage(new ItemsPage());
            MainPage.BindingContext = itemsViewModel;
        }

        private Version AssemblyProductVersion
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);

                return attributes.Length == 0
                    ? new Version()
                    : new Version(((AssemblyInformationalVersionAttribute)attributes[0]).InformationalVersion);
            }
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
