using Last.Core.Message;
using Last.Core.Models;
using Last.Core.Services;
using Last.Core.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<ItemListViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command AddItemCommand { get; set; }
        public INavigation Navigation { get; internal set; }

        private IDataStore<Item> _dataStore;

        public ItemsViewModel(IDataStore<Item> dataStore)
        {
            Title = "Last";
            _dataStore = dataStore;
            Items = new ObservableCollection<ItemListViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(AddItemExecute);

            MessagingCenter.Subscribe<NewItemViewModel, SaveItemMessage>(this, string.Empty, async (obj, message) =>
            {
                // Save
                await _dataStore.AddItemAsync(message.Item);

                // Refresh the list
                LoadItemsCommand.Execute(null);
            });
            MessagingCenter.Subscribe<IItemUpdater, UpdateItemMessage>(this, string.Empty, async (obj, message) =>
            {
                // Update
                await _dataStore.UpdateItemAsync(message.Item);

                // Refresh the list
                LoadItemsCommand.Execute(null);
            });
            MessagingCenter.Subscribe<ItemDetailViewModel, DeleteItemMessage>(this, string.Empty, async (obj, message) =>
            {
                // Delete
                await _dataStore.DeleteItemAsync(message.Id);

                // Refresh the list
                LoadItemsCommand.Execute(null);
            });
        }

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
                Items.Clear();
                var items = await _dataStore.GetItemsAsync(true);
                foreach (var item in items.OrderByDescending(x => x.LastModificationDate))
                {
                    var itemListViewModel = new ItemListViewModel(item, Navigation);
                    itemListViewModel.CountChanged += OnCountChanged;
                    Items.Add(itemListViewModel);
                }
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

        private void OnCountChanged()
        {
            LoadItemsCommand.Execute(null);
        }
    }
}