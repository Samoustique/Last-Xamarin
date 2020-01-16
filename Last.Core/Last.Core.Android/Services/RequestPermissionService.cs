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
            if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance, new String[] { Manifest.Permission.WriteExternalStorage }, MainActivity.ExternalStorageRequestCode);
                return false;
            }

            if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance, new String[] { Manifest.Permission.Camera }, MainActivity.CameraRequestCode);
                return false;
            }

            return true;
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