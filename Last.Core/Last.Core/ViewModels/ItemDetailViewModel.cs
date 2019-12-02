﻿using System;
using System.IO;
using Last.Core.Models;
using Last.Core.Services;
using Plugin.Media;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public abstract class ItemDetailViewModel : BaseViewModel
    {
        public readonly string PictureChoiceCamera = "Take Photo";
        public readonly string PictureChoiceBrowse = "Choose from Gallery";

        public string ButtonTitle { get; set; }
        public string Text { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ImageSource Image
        {
            get { return this._image; }
            set
            {
                SetProperty(ref _image, value);
            }
        }

        public INavigation Navigation { get; internal set; }
        public Command MainCommand { get; set; }

        public Command PickPhotoButtonCommand { get; set; }
        public Command DeleteItemCommand { get; set; }

        public event Action PictureChoice;

        private ImageSource _image;

        public ItemDetailViewModel()
        {
            PickPhotoButtonCommand = new Command(PickPhotoButtonExecute);
            DeleteItemCommand = new Command<Item>(DeleteItemExecute, DeleteItemCanExecute);
        }

        protected bool DeleteItemCanExecute(Item item)
        {
            return item != null;
        }

        private async void DeleteItemExecute(Item item)
        {
            var fileDeleterService = DependencyService.Get<IFileDeleterService>();
            fileDeleterService.FileDeleted += OnFileDeleted;
            fileDeleterService.DeleteFile(item.Id, item.ImagePath);
        }

        private async void OnFileDeleted(string itemId, bool result)
        {
            await Navigation.PopAsync();
            MessagingCenter.Send(this, "Delete", itemId);
        }

        private void PickPhotoButtonExecute()
        {
            PictureChoice?.Invoke();
        }

        internal void PictureChoiceDone(string choice)
        {
            if(choice == PictureChoiceCamera)
            {
                TakePicture();
            }
            else if (choice == PictureChoiceBrowse)
            {
                BrowsePicture();
            }
        }

        private async void TakePicture()
        {
            //var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //{
            //    Directory = "Test",
            //    SaveToAlbum = true,
            //    CompressionQuality = 75,
            //    CustomPhotoSize = 50,
            //    PhotoSize = PhotoSize.MaxWidthHeight,
            //    MaxWidthHeight = 2000,
            //    DefaultCamera = CameraDevice.Front
            //});
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable ||
                !CrossMedia.Current.IsTakePhotoSupported)
            {
                // TODO Message can't take camera
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            {
                //Directory = "Sample",
                //Name = "test.jpg"
            });

            if(file == null)
            {
                // TODO Message can't take camera
                return;
            }

            var stream = file.GetStream();
            if (stream != null)
            {
                MemoryStream ms = CopyStreamToMemory(stream);
                var name = file.ToString();
                ImagePath = DependencyService.Get<IPhotoSerializerService>().SavePicture(/*filename*/ name, ms, "imagesFolder");
                Image = ImageSource.FromStream(() => ms);
            }
        }

        private void BrowsePicture()
        {
            var photoPickerService = DependencyService.Get<IPhotoPickerService>();
            SubscribePhotoPicker(photoPickerService);
            photoPickerService.GetImageStreamAsync();
        }

        private void OnPhotoPickedSucceeded(Stream stream, string filename)
        {
            UnsubscribePhotoPicker();
            if (stream != null)
            {
                MemoryStream ms = CopyStreamToMemory(stream);
                ImagePath = DependencyService.Get<IPhotoSerializerService>().SavePicture(filename, ms, "imagesFolder");
                Image = ImageSource.FromStream(() => ms);
            }
        }

        private MemoryStream CopyStreamToMemory(Stream inputStream)
        {
            MemoryStream ret = new MemoryStream();
            const int BUFFER_SIZE = 1024;
            byte[] buf = new byte[BUFFER_SIZE];

            int bytesread = 0;
            while ((bytesread = inputStream.Read(buf, 0, BUFFER_SIZE)) > 0)
                ret.Write(buf, 0, bytesread);

            ret.Position = 0;
            return ret;
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
