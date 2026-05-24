using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private Button _playLevelButton;
    [SerializeField] private CanvasGroup _canvasGroup;


    [Header("Sounds")]
    [SerializeField] private SoundConfig _levelLoadSound;


    public bool IsInteractable => _playLevelButton.interactable;

    private Action _onPlayCallback;

    private void Awake()
    {
        _playLevelButton.onClick.AddListener(() => { 
            SoundManager.Play(_levelLoadSound, "Level Select");
            _onPlayCallback?.Invoke();
        });
    }

    public void Set(int level, Action onPlayCallback)
    {
        _level.SetText(level.ToString("D2"));
        _onPlayCallback = onPlayCallback;
    }

    public void SetupNavigation(LevelUI left, LevelUI right, LevelUI up, LevelUI down)
    {
        var nav = _playLevelButton.navigation;


        nav.mode = Navigation.Mode.Explicit;

        nav.selectOnLeft = left ?  left._playLevelButton : null;
        nav.selectOnRight = right ? right._playLevelButton : null;
        nav.selectOnUp = up ? up._playLevelButton : null;
        nav.selectOnDown = down ? down._playLevelButton : null;

        _playLevelButton.navigation = nav;
    }

    public void SetInteractable(bool interactable)
    {
        _playLevelButton.interactable = interactable;
        _canvasGroup.alpha = interactable ? 1 : 0.1f;
    }
}
