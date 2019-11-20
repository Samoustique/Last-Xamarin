using System;
using Last.Core.Models;
using Last.Core.Services;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        public string Text { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }

        public INavigation Navigation { get; internal set; }
        public Command SaveCommand { get; set; }

        private IDataStore<Item> DataStore;

        public NewItemViewModel(IDataStore<Item> dataStore)
        {
            DataStore = dataStore;
            SaveCommand = new Command(SaveExecuteAsync);
        }

        private async void SaveExecuteAsync()
        {
            await DataStore.AddItemAsync(
                new Item()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = this.Text,
                    Count = this.Count,
                    Description = this.Description,
                    LastModificationDate = DateTime.Now
                });
            var items = await DataStore.GetItemsAsync(true);
            await Navigation.PopModalAsync();
            MessagingCenter.Send(this, "Refresh");
        }
    }
}
