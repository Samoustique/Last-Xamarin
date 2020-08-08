using System;
using System.Windows.Input;
using Last.Core.Message;
using Last.Core.Models;
using Last.Core.Views;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class ItemListViewModel : BaseViewModel, IItemUpdater
    {
        private UpdateItemViewModel _updateItemViewModel;

        public ItemListViewModel(Item item, INavigation navigation)
        {
            Item = item;
            Navigation = navigation;
            _updateItemViewModel = new UpdateItemViewModel(Item);
            OpenItemDetailCommand = new Command(OpenItemDetailExecute);
            IncrementCommand = new Command(IncrementExecute);
        }

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

        public void Clean()
        {
            _updateItemViewModel.Unsubscribe();
        }

        public void IncrementExecute()
        {
            Item.Count++;
            Item.LastModificationDate = DateTime.Now;
            MessagingCenter.Send(this as IItemUpdater, string.Empty, new UpdateItemMessage() { Item = Item });
            CountChanged?.Invoke();
        }

        private async void OpenItemDetailExecute()
        {
            _updateItemViewModel.Subscribe();
            await Navigation.PushAsync(new ItemDetailPage(_updateItemViewModel));
        }
    }
}
