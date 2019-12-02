using System;
using Last.Core.Services;

namespace Last.Core.iOS.Services
{
    public class FileDeleterService : IFileDeleterService
    {
        public event Action<string, bool> FileDeleted;

        public void DeleteFile(string itemId, string imagePath)
        {
            throw new NotImplementedException();
        }
    }
}