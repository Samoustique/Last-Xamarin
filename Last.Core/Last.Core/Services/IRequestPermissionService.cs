using System;

namespace Last.Core.Services
{
    public interface IRequestPermissionService
    {
        event Action CameraPermissionSucceded;
        event Action CameraPermissionFailed;
        event Action WriteExternalStoragePermissionSucceded;
        event Action WriteExternalStoragePermissionFailed;
        bool RequestCameraPermission();
    }
}
