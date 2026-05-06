using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalSaveService", menuName = "Services/LocalSave")]
public class LocalSaveService : BaseSaveService
{
    public override string BaseURL => Application.persistentDataPath;

    public override T Load<T>(string route)
    {
        return JsonConvert.DeserializeObject<T>(IOHelper.GetFileData(BaseURL, route));
    }

    public override void Save<T>(string route, T data)
    {
        IOHelper.SetFileData(BaseURL, route, JsonConvert.SerializeObject(data));
    }
}