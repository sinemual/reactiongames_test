namespace Client.Infrastructure.Services
{
    public interface ISaveLoadService
    {
        T Load<T>(string identification = "") where T : class;
        void Save<T>(T obj, string identification = "");
    }
}