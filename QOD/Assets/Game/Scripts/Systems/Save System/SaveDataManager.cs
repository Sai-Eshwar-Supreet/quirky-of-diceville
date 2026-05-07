using Newtonsoft.Json;
using System;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [SerializeField] private BaseSaveService _saveService;

    private SaveData _initData;
    private SaveData? _saveData = null;

    public SaveData SaveData => _saveData.Value;

    public void Init()
    {
        var json = IOHelper.GetFileData(Application.streamingAssetsPath, "InitSaveData.json");
        _initData = JsonConvert.DeserializeObject<SaveData>(json);
    }

    // implement save and load behaviours here, using _saveService to handle the actual saving and loading of data.

    public async void Load()
    {
        _saveData = await _saveService.Load<SaveData>("/SaveData.json");

        if( _saveData == null )
        {
            Debug.Log("No save data found, creating new save data.");
            SeedInitData();
        }
    }

    private void SeedInitData()
    {
        _saveData = _initData;
    }
}
