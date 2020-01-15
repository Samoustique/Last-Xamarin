using System;

namespace Last.Core.Services
{
    public interface IRequestPermissionService
    {
        event Action CameraPermissionSucceded;
        event Action CameraPermissionFailed;
        void RequestCameraPermission();
    }
}
