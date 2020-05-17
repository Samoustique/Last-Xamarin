using Last.Core.Droid.Services;
using Last.Core.Services;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppliPathGetterService))]
namespace Last.Core.Droid.Services
{
    public class AppliPathGetterService : IAppliPathGetterService
    {
        public string GetAppliMainPath()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "Last");
            Directory.CreateDirectory(documentsPath);
            return documentsPath;
        }

        public string[] GetFileContent(string path)
        {
            return File.ReadAllLines(path);
        }

        public bool GetOrCreateFile(string filename, out string path)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "Last");
            var todelete = Directory.CreateDirectory(documentsPath);
            path = Path.Combine(documentsPath, filename);

            bool alreadyExists = File.Exists(path);
            if (!alreadyExists)
            {
                using (File.Create(path)) ;
            }
            return alreadyExists;
        }
    }
}