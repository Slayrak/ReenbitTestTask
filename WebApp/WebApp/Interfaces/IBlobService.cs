namespace WebApp.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream);
    }
}
