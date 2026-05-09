using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LoadingUI _loadingUI;

    private bool _isLoading = false;

    public async Task LoadScene(int buildIndex)
    {
        if(_isLoading) return;

        _isLoading = true;
        string sceneName = SceneManager.GetSceneByBuildIndex(buildIndex).name;

        _loadingUI.SetActive(true);
        _loadingUI.UpdateProgress(0, sceneName);
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

        operation.allowSceneActivation = false;

        while(operation.progress < 0.9f)
        {
            _loadingUI.UpdateProgress(operation.progress / 0.9f, sceneName);
            await Task.Yield();
        }
        _loadingUI.UpdateProgress(1, sceneName);
        operation.allowSceneActivation = true;

        while(!operation.isDone) await Task.Yield();

        _loadingUI.SetActive(false);

        _isLoading = false;
    }
}
