using System;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Last.Core.Droid.Services;
using Last.Core.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]
namespace Last.Core.Droid.Services
{
    public class PhotoPickerService : IPhotoPickerService
    {
        public event Action<Stream> PhotoPickedSucceeded;
        public event Action PhotoPickedFailed;

        public void GetImageStreamAsync()
        {
            if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance, new String[] { Manifest.Permission.ReadExternalStorage }, MainActivity.PickImageRequestCode);
            }

            RaisePhotoPickedSucceeded();
        }

        public void RaisePhotoPickedSucceeded()
        {
            if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted)
            {
                // Define the Intent for getting images
                Intent intent = new Intent();
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionGetContent);

                // Start the picture-picker activity (resumes in MainActivity.cs)
                MainActivity.Instance.StartActivityForResult(
                    Intent.CreateChooser(intent, "Select Picture"),
                    MainActivity.PickImageId);

                // Save the TaskCompletionSource object as a MainActivity property
                MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

                // fire the event
                PhotoPickedSucceeded?.Invoke(MainActivity.Instance.PickImageTaskCompletionSource.Task?.Result);
            }
            else
            {
                PhotoPickedSucceeded?.Invoke(null);
            }
        }

        public void RaisePhotoPickedFailed()
        {
            PhotoPickedFailed?.Invoke();
        }
    }
}