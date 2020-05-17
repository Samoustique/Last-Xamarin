using System;
using System.IO;

namespace Last.Core.Services
{
    public interface IPhotoPickerService
    {
        event Action<Stream, string> PhotoPickedSucceeded;
        event Action PhotoPickedFailed;
        void GetImageStreamAsync();
    }
}
