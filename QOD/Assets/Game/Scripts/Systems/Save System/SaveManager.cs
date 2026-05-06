using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    [SerializeField] private BaseSaveService _saveService;

    // implement save and load behaviours here, using _saveService to handle the actual saving and loading of data.
}
