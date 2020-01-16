using Android.App;
using Android.Content.PM;
using Android.OS;
using System.IO;
using Android.Content;
using Xamarin.Forms;
using Last.Core.Droid.Services;
using Last.Core.Services;

namespace Last.Core.Droid
{
    [Activity(Label = "Last.Core", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            Instance = this;
        }

        // Field, property, and method for Picture Picker
        public static readonly int PickImageId = 1000;
        public const int PickImageRequestCode = 1;
        public const int CameraRequestCode = 2;
        public const int ExternalStorageRequestCode = 3;

        public static MainActivity Instance;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                var iService = DependencyService.Get<IPhotoPickerService>();
                PhotoPickerService service = iService as PhotoPickerService;

                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    var filename = Path.GetFileName(uri.Path);
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    service.RaisePhotoPickedSucceeded(stream, filename);
                }
                else
                {
                    service.RaisePhotoPickedSucceeded();
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch(requestCode)
            {
                case PickImageRequestCode:
                    if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                    {
                        // Location permission has been granted, okay to retrieve the location of the device.
                        DependencyService.Get<PhotoPickerService>().PickPhoto();
                    }
                    else
                    {
                        DependencyService.Get<PhotoPickerService>().RaisePhotoPickedFailed();
                    }
                    break;
                case CameraRequestCode:
                    if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                    {
                        DependencyService.Get<RequestPermissionService>().RaiseCameraPermissionSucceded();
                    }
                    else
                    {
                        DependencyService.Get<RequestPermissionService>().RaiseCameraPermissionFailed();
                    }
                    break;
                case ExternalStorageRequestCode:
                    if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                    {
                        DependencyService.Get<RequestPermissionService>().RaiseWriteExternalStoragePermissionSucceded();
                    }
                    else
                    {
                        DependencyService.Get<RequestPermissionService>().RaiseWriteExternalStoragePermissionFailed();
                    }
                    break;
                default:
                    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                    break;
            }
        }
    }
}