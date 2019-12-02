using System;

namespace Last.Core.Services
{
    public interface IFileDeleterService
    {
        event Action<string, bool> FileDeleted;
        void DeleteFile(string itemId, string imagePath);
    }
}
