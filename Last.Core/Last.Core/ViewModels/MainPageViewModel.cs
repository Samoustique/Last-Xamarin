using Last.Core.Models;
using Last.Core.Services;
using System;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ItemsViewModel ItemsViewModel { get; }
        public AboutViewModel AboutViewModel { get; }

        private Lazy<MockDataStore> Mock = new Lazy<MockDataStore>(() => new MockDataStore());
        private IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>() ?? Mock.Value;

        public MainPageViewModel()
        {
            ItemsViewModel = new ItemsViewModel(DataStore);
            AboutViewModel = new AboutViewModel();
        }
    }
}
