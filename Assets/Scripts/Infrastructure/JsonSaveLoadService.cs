using System.IO;
using Client.DevTools.MyTools;
using Sirenix.Serialization;

namespace Client.Infrastructure.Services
{
    public class JsonSaveLoadService : ISaveLoadService
    {
        public T Load<T>(string identification = "") where T : class
        {
            var filename = $"{GetType()}{identification}.json";
            string dataPath = Path.Combine(Utility.GetDataPath(), filename);
            if (File.Exists(dataPath))
            {
                byte[] bytes = File.ReadAllBytes(dataPath);
                //Debug.Log(json);
                return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.JSON);
            }

            return null;
        }

        public void Save<T>(T obj, string identification = "")
        {
            var filename = $"{GetType()}{identification}.json";
            string dataPath = Path.Combine(Utility.GetDataPath(), filename);
            if (!Directory.Exists(Utility.GetDataPath()))
                Directory.CreateDirectory(Utility.GetDataPath());

            byte[] bytes = SerializationUtility.SerializeValue(obj, DataFormat.JSON);
            File.WriteAllBytes(dataPath, bytes);
        }
    }
}