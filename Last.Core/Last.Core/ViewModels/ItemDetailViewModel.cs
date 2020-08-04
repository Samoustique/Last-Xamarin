using System;
using System.IO;
using Last.Core.Message;
using Last.Core.Models;
using Last.Core.Services;
using Plugin.Media;
using Xamarin.Forms;

namespace Last.Core.ViewModels
{
    public abstract class ItemDetailViewModel : BaseViewModel
    {
        private ImageSource _image;
        private bool _isCameraPermissionOn = false;
        private bool _isWriteExternalStoragePermissionOn = false;
        private IRequestPermissionService _requestPermissionService;

        public readonly string PictureChoiceCamera = "Take Photo";
        public readonly string PictureChoiceBrowse = "Choose from Gallery";

        public ItemDetailViewModel()
        {
            PickPhotoButtonCommand = new Command(PickPhotoButtonExecute);
            DeleteItemCommand = new Command<Item>(DeleteItemExecute, DeleteItemCanExecute);

            _requestPermissionService = DependencyService.Get<IRequestPermissionService>();
            _requestPermissionService.WriteExternalStoragePermissionFailed += OnWriteExternalStoragePermissionFailed;
            _requestPermissionService.WriteExternalStoragePermissionSucceded += OnWriteExternalStoragePermissionSucceded;
            _requestPermissionService.CameraPermissionFailed += OnCameraPermissionFailed;
            _requestPermissionService.CameraPermissionSucceded += OnCameraPermissionSucceded;
        }

        private bool HasPermissionToTakePhoto => _isCameraPermissionOn && _isWriteExternalStoragePermissionOn;
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

        protected bool DeleteItemCanExecute(Item item)
        {
            return item != null;
        }

        private void DeleteItemExecute(Item item)
        {
            var fileDeleterService = DependencyService.Get<IFileDeleterService>();
            fileDeleterService.FileDeleted += OnFileDeleted;
            fileDeleterService.DeleteFile(item.Id, item.ImagePath);
        }

        private async void OnFileDeleted(string itemId, bool result)
        {
            await Navigation.PopAsync();
            MessagingCenter.Send(this, string.Empty, new DeleteItemMessage() { Id = itemId });
        }

        private void PickPhotoButtonExecute()
        {
            PictureChoice?.Invoke();
        }

        internal void PictureChoiceDone(string choice)
        {
            if(choice == PictureChoiceCamera)
            {
                TakePictureCheckingPermission();
            }
            else if (choice == PictureChoiceBrowse)
            {
                BrowsePicture();
            }
        }

        private async void TakePictureCheckingPermission()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable ||
                !CrossMedia.Current.IsTakePhotoSupported)
            {
                // TODO Message can't take camera
                return;
            }

            if (!_requestPermissionService.RequestCameraPermission())
            {
                return;
            }

            _isWriteExternalStoragePermissionOn = true;
            _isCameraPermissionOn = true;

            TryTakePicture();
        }

        private void OnWriteExternalStoragePermissionSucceded()
        {
            _isWriteExternalStoragePermissionOn = true;
            TryTakePicture();
        }

        private void OnWriteExternalStoragePermissionFailed()
        {
            // todo display message "no permission"
            _isWriteExternalStoragePermissionOn = false;
        }

        private void OnCameraPermissionSucceded()
        {
            _isCameraPermissionOn = true;
            TryTakePicture();
        }

        private void OnCameraPermissionFailed()
        {
            // todo display message "no permission"
            _isCameraPermissionOn = false;
        }

        private async void TryTakePicture()
        {
            try
            {
                if (!HasPermissionToTakePhoto)
                {
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions());
                //{
                //    Directory = "Last",
                //    SaveToAlbum = true,
                //    CompressionQuality = 75,
                //    CustomPhotoSize = 50,
                //    PhotoSize = PhotoSize.MaxWidthHeight,
                //    MaxWidthHeight = 2000,
                //    DefaultCamera = CameraDevice.Front
                //});

                if (file == null)
                {
                    // TODO Message can't take camera
                    return;
                }

                var stream = file.GetStream();
                if (stream != null)
                {
                    MemoryStream ms = CopyStreamToMemory(stream);
                    ImagePath = DependencyService.Get<IPhotoSerializerService>().SavePicture(Path.GetFileName(file.Path), ms, "imagesFolder");
                    Image = ImageSource.FromStream(() => ms);
                }
            }
            catch (Exception e)
            {
                // TODO display error
                int kk = 0;
                ++kk;
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
