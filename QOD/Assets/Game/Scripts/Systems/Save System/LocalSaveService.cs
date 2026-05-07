using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalSaveService", menuName = "Services/LocalSave")]
public class LocalSaveService : BaseSaveService
{
    public override string BaseURL => Application.persistentDataPath;

    public override Task<T> Load<T>(string route)
    {
        return Task.FromResult(JsonConvert.DeserializeObject<T>(IOHelper.GetFileData(BaseURL, route)));
    }

    public override Task Save<T>(string route, T data)
    {
        IOHelper.SetFileData(BaseURL, route, JsonConvert.SerializeObject(data));
        return Task.CompletedTask;
    }
}