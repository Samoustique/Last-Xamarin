using Xamarin.Forms;
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

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var itemListViewModel = args.SelectedItem as ItemListViewModel;
            if (itemListViewModel == null)
                return;

            itemListViewModel.IncrementAsync();

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as ItemsViewModel).Navigation = Navigation;

            if ((BindingContext as ItemsViewModel).Items.Count == 0)
                (BindingContext as ItemsViewModel).LoadItemsCommand.Execute(null);
        }
    }
}