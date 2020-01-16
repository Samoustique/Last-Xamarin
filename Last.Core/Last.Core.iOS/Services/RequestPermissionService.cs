using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Last.Core.Services;
using UIKit;

namespace Last.Core.iOS.Services
{
    public class RequestPermissionService : IRequestPermissionService
    {
        public event Action CameraPermissionSucceded;
        public event Action CameraPermissionFailed;

        public bool RequestCameraPermission()
        {
            throw new NotImplementedException();
        }
    }
}