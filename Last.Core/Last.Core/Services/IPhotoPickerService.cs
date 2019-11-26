using System;
using System.IO;
using System.Threading.Tasks;

namespace Last.Core.Services
{
    public interface IPhotoPickerService
    {
        event Action<Stream, string> PhotoPickedSucceeded;
        event Action PhotoPickedFailed;
        void GetImageStreamAsync();
    }
}
