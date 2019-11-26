using Android.App;
using Android.Content.PM;
using Android.OS;
using System.IO;
using Android.Content;
using Xamarin.Forms;
using Last.Core.Droid.Services;
using Last.Core.Services;
using static Android.Provider.MediaStore.Images;

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
                    var filename = GetFileName(uri);
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    service.RaisePhotoPickedSucceeded(stream, filename);
                }
                else
                {
                    service.RaisePhotoPickedSucceeded();
                }
            }
        }

        private string GetFileName(Android.Net.Uri uri)
        {
            string doc_id = "";
            using (var c1 = ContentResolver.Query(uri, null, null, null, null))
            {
                c1.MoveToFirst();
                string document_id = c1.GetString(0);
                doc_id = document_id.Substring(document_id.LastIndexOf(":") + 1);
            }

            string path = null;

            // The projection contains the columns we want to return in our query.
            string selection = Media.InterfaceConsts.Id + " =? ";
            using (var cursor = ManagedQuery(Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
            {
                if (cursor != null)
                {
                    var columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
            }

            return Path.GetFileName(path);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == PickImageRequestCode)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // Location permission has been granted, okay to retrieve the location of the device.
                    DependencyService.Get<PhotoPickerService>().PickPhoto();
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