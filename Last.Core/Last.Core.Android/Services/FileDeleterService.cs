using System;
using System.IO;
using Last.Core.Services;

namespace Last.Core.Droid.Services
{
    public class FileDeleterService : IFileDeleterService
    {
        public event Action<string, bool> FileDeleted;

        public void DeleteFile(string itemId, string imagePath)
        {
            bool result = true;
            try
            {
                File.Delete(imagePath);
            }
            catch(Exception e)
            {
                result = false;
            }

            FileDeleted?.Invoke(itemId, result);
        }
    }
}