using System.IO;

namespace Last.Core.Services
{
    public interface IPhotoSerializerService
    {
        string SavePicture(string name, MemoryStream data, string location = "temp");
    }
}
