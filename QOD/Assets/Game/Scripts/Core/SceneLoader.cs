using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LoadingUI _loadingUI;


    [Header("Sounds")]
    [SerializeField] private SoundConfig _sceneLoadSound;

    private bool _isLoading = false;

    public async Task LoadScene(int buildIndex)
    {
        if(_isLoading) return;

        _isLoading = true;

        _loadingUI.SetActive(true);
        SoundManager.Play(_sceneLoadSound, "Scene load");

        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

        operation.allowSceneActivation = false;

        while(operation.progress < 0.9f)
        {
            _loadingUI.SetTargetProgress(operation.progress / 0.9f);
            await Task.Yield();
        }
        _loadingUI.SetTargetProgress(1);

        while (!_loadingUI.IsFinished) await Task.Yield();

        await Task.Delay(250); // delay to show 100% complete


        operation.allowSceneActivation = true;

        while(!operation.isDone) await Task.Yield();

        _loadingUI.SetActive(false);

        _isLoading = false;
    }
}
