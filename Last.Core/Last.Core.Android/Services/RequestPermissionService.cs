using System;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Last.Core.Services;

namespace Last.Core.Droid.Services
{
    public class RequestPermissionService : IRequestPermissionService
    {
        public event Action CameraPermissionSucceded;
        public event Action CameraPermissionFailed;
        
        public void RequestCameraPermission()
        {
            if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance, new String[] { Manifest.Permission.Camera }, MainActivity.CameraRequestCode);
            }
        }

        internal void RaiseCameraPermissionSucceded()
        {
            CameraPermissionSucceded?.Invoke();
        }

        internal void RaiseCameraPermissionFailed()
        {
            CameraPermissionFailed?.Invoke();
        }
    }
}