using System;
using System.Windows.Input;
using Last.Core.Models;
using Last.Core.Services;
using Last.Core.Views;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class ItemListViewModel : BaseViewModel
    {
        private Messaging _messaging;

        public Item Item { get; private set; }

        public int Count => Item.Count;
        public ImageSource Image
        {
            get
            {
                return string.IsNullOrEmpty(Item.ImagePath) ? null : ImageSource.FromFile(Item.ImagePath);
            }
        }

        public ICommand OpenItemDetailCommand { get; set; }
        public ICommand IncrementCommand { get; set; }
        public INavigation Navigation { get; internal set; }

        public event Action CountChanged;

        public ItemListViewModel(Item item, INavigation navigation, Services.Messaging messaging)
        {
            _messaging = messaging;
            Item = item;
            Navigation = navigation;
            OpenItemDetailCommand = new Command(OpenItemDetailExecute);
            IncrementCommand = new Command(IncrementExecute);
        }

        public void IncrementExecute()
        {
            Item.Count++;
            Item.LastModificationDate = DateTime.Now;
            CountChanged?.Invoke();
        }

        private async void OpenItemDetailExecute()
        {
            var viewModel = new UpdateItemViewModel(Item, _messaging);
            await Navigation.PushAsync(new ItemDetailPage(viewModel));
        }
    }
}
