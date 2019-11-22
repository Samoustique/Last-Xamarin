﻿using System;
using System.IO;
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
        public ImageSource Image
        {
            get { return this._image; }
            set
            {
                SetProperty(ref _image, value);
            }
        }

        public INavigation Navigation { get; internal set; }
        public Command SaveCommand { get; set; }
        public Command PickPhotoButtonCommand { get; set; }

        private IDataStore<Item> DataStore;
        private ImageSource _image;

        public NewItemViewModel(IDataStore<Item> dataStore)
        {
            DataStore = dataStore;
            SaveCommand = new Command(SaveExecute);
            PickPhotoButtonCommand = new Command(PickPhotoButtonExecute);
        }

        private async void SaveExecute()
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

        private void PickPhotoButtonExecute()
        {
            var photoPickerService = DependencyService.Get<IPhotoPickerService>();
            SubscribePhotoPicker(photoPickerService);
            photoPickerService.GetImageStreamAsync();
        }

        private void OnPhotoPickedSucceeded(Stream stream)
        {
            UnsubscribePhotoPicker();
            if (stream != null)
            {
                Image = ImageSource.FromStream(() => stream);
            }
        }

        private void OnPhotoPickedFailed()
        {
            UnsubscribePhotoPicker();
            // TODO display message "you need to grant rights"
         }

        private void SubscribePhotoPicker(IPhotoPickerService photoPickerService)
        {
            photoPickerService.PhotoPickedSucceeded += OnPhotoPickedSucceeded;
            photoPickerService.PhotoPickedFailed += OnPhotoPickedFailed;
        }

        private void UnsubscribePhotoPicker()
        {
            var photoPickerService = DependencyService.Get<IPhotoPickerService>();
            photoPickerService.PhotoPickedSucceeded -= OnPhotoPickedSucceeded;
            photoPickerService.PhotoPickedFailed -= OnPhotoPickedFailed;
        }
    }
}
