using System.IO;

namespace Last.Core.Services
{
    public interface IPhotoSerializerService
    {
        void SavePicture(string name, MemoryStream data, string location = "temp");
    }
}
