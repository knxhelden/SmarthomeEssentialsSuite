namespace KnxHelden.SHES.Services.Files
{
    public interface IFileService : IService
    {
        T Read<T>(string folderPath, string fileName);

        void Save<T>(string folderPath, string fileName, T content);

        void Delete(string folderPath, string fileName);
    }
}
