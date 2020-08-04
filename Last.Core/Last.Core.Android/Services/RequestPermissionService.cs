using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Last.Core.Droid.Services;
using Last.Core.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(RequestPermissionService))]
namespace Last.Core.Droid.Services
{
    public class RequestPermissionService : IRequestPermissionService
    {
        public event Action CameraPermissionSucceded;
        public event Action CameraPermissionFailed;
        public event Action WriteExternalStoragePermissionSucceded;
        public event Action WriteExternalStoragePermissionFailed;

        public bool RequestCameraPermission()
        {
            bool result = true;

            if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.Camera) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance, new String[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage }, MainActivity.CameraStorageRequestCode);
                result = false;
            }

            return result;
        }

        internal void RaiseCameraPermissionSucceded()
        {
            CameraPermissionSucceded?.Invoke();
        }

        internal void RaiseCameraPermissionFailed()
        {
            CameraPermissionFailed?.Invoke();
        }

        internal void RaiseWriteExternalStoragePermissionSucceded()
        {
            WriteExternalStoragePermissionSucceded?.Invoke();
        }

        internal void RaiseWriteExternalStoragePermissionFailed()
        {
            WriteExternalStoragePermissionFailed?.Invoke();
        }
    }
}