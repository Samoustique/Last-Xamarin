using Last.Core.Droid.Services;
using Last.Core.Services;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoSerializerService))]
namespace Last.Core.Droid.Services
{
    public class PhotoSerializerService : IPhotoSerializerService
    {
        public string SavePicture(string name, MemoryStream data, string location = "temp")
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "Last", location);
            Directory.CreateDirectory(documentsPath);
            string filePath = Path.Combine(documentsPath, name);

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                data.Read(bArray, 0, (int)data.Length);
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
            data.Position = 0;

            return filePath;
        }
    }
}