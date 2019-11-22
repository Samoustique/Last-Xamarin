using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Last.Core.ViewModels;

namespace Last.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(ItemDetailViewModel newItemViewModel)
        {
            InitializeComponent();
            newItemViewModel.Navigation = Navigation;
            BindingContext = newItemViewModel;
        }
    }
}