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

        private Lazy<MockDataStore> Mock = new Lazy<MockDataStore>(() => new MockDataStore());
        private IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>() ?? Mock.Value;

        public ItemsViewModel()
        {
            Title = "Last";
            Items = new ObservableCollection<ItemListViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(AddItemExecute);

            MessagingCenter.Subscribe<NewItemViewModel, Item>(this, "Save", async (obj, item) =>
            {
                // Save
                await DataStore.AddItemAsync(item);

                // Refresh the list
                LoadItemsCommand.Execute(null);
            });

            MessagingCenter.Subscribe<UpdateItemViewModel, Item>(this, "Update", async (obj, item) =>
            {
                // Update
                await DataStore.UpdateItemAsync(item);

                // Refresh the list
                LoadItemsCommand.Execute(null);
            });

            MessagingCenter.Subscribe<ItemDetailViewModel, string>(this, "Delete", async (obj, id) =>
            {
                // Delete
                await DataStore.DeleteItemAsync(id);

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
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items.OrderByDescending(x => x.LastModificationDate))
                {
                    var itemListViewModel = new ItemListViewModel(item, Navigation);
                    itemListViewModel.CountChanged += OnCountChanged;
                    Items.Add(itemListViewModel);
                }
            }
            catch (Exception ex)
            {
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