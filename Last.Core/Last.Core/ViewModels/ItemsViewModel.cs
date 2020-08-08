using Last.Core.Message;
using Last.Core.Models;
using Last.Core.Services;
using Last.Core.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private List<ItemListViewModel> _items;
        private IDataStore<Item> _dataStore;

        public ItemsViewModel(IDataStore<Item> dataStore)
        {
            Title = "Last";
            _dataStore = dataStore;
            _items = new List<ItemListViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(AddItemExecute);

            MessagingCenter.Subscribe<NewItemViewModel, SaveItemMessage>(this, string.Empty, async (obj, message) =>
            {
                // Save
                await _dataStore.AddItemAsync(message.Item);
            });
            MessagingCenter.Subscribe<IItemUpdater, UpdateItemMessage>(this, string.Empty, async (obj, message) =>
            {
                // Update
                await _dataStore.UpdateItemAsync(message.Item);
            });
            MessagingCenter.Subscribe<ItemDetailViewModel, DeleteItemMessage>(this, string.Empty, async (obj, message) =>
            {
                await _dataStore.DeleteItemAsync(message.Id);
            });
        }

        public List<ItemListViewModel> Items
        {
            get
            {
                return _items;
            }
            set
            {
                SetProperty(ref _items, value);
            }
        }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }
        public INavigation Navigation { get; internal set; }

        private async void AddItemExecute()
        {
            var viewModel = new NewItemViewModel();
            await Navigation.PushAsync(new ItemDetailPage(viewModel));
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.ForEach(x => x.Clean());
                Items.Clear();

                List<ItemListViewModel> myItems = new List<ItemListViewModel>();

                var items = await _dataStore.GetItemsAsync(true);
                foreach (var item in items.OrderByDescending(x => x.LastModificationDate))
                {
                    var itemListViewModel = new ItemListViewModel(item, Navigation);
                    myItems.Add(itemListViewModel);
                }

                Items = myItems;
            }
            catch (Exception ex)
            {
                // TODO error
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}