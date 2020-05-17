namespace Last.Core.Services
{
    public interface IAppliPathGetterService
    {
        string GetAppliMainPath();
        bool GetOrCreateFile(string filename, out string path);
        string[] GetFileContent(string path);
    }
}
