using Android.App;
using Android.Content.PM;
using Android.OS;
using System.Threading.Tasks;
using System.IO;
using Android.Content;
using Xamarin.Forms;
using Last.Core.Droid.Services;

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
        public static readonly int PickImageRequestCode = 1;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        public static MainActivity Instance;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == PickImageRequestCode)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // Location permission has been granted, okay to retrieve the location of the device.
                    DependencyService.Get<PhotoPickerService>().RaisePhotoPickedSucceeded();
                }
                else
                {
                    DependencyService.Get<PhotoPickerService>().RaisePhotoPickedFailed();
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
    }
}