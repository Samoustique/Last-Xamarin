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

            var itemsDataStore = new ItemsDataStore();

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
