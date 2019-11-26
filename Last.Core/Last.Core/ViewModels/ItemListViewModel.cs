using System;
using Last.Core.Models;
using Last.Core.Views;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class ItemListViewModel : BaseViewModel
    {
        public Item Item { get; private set; }
        
        public int Count => Item.Count;
        public ImageSource Image => ImageSource.FromFile(Item.ImagePath);

        public Command OpenItemDetailCommand { get; set; }
        public INavigation Navigation { get; internal set; }

        public event Action CountChanged;

        public ItemListViewModel(Item item, INavigation navigation)
        {
            Item = item;
            Navigation = navigation;
            OpenItemDetailCommand = new Command(OpenItemDetailCommandExecute);
        }

        public void IncrementAsync()
        {
            Item.Count++;
            Item.LastModificationDate = DateTime.Now;
            CountChanged?.Invoke();
        }

        private async void OpenItemDetailCommandExecute()
        {
            var viewModel = new UpdateItemViewModel(Item);
            await Navigation.PushAsync(new ItemDetailPage(viewModel));
        }
    }
}
