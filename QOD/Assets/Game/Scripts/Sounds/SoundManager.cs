using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using System.Linq;


/// <summary>
/// Represents different sound groups in the audio mixer (e.g., Master, Music, SFX).
/// </summary>
public enum SoundGroup
{
    Master,
    Music,
    SFX
}


public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioMixer _audioMixer;
    /// <summary>
    /// Parent transform for dynamically created audio sources.
    /// </summary>
    [SerializeField] private Transform _dynamicSourcesParent;

    /// <summary>
    /// Represents the interval in seconds for cleaning up unused audio sources.
    /// </summary>
    [SerializeField] private float _soundCleanupInterval = 2f;

    /// <summary>
    /// Object pool for managing reusable audio sources.
    /// </summary>
    private ObjectPool<AudioSource> _soundPool;

    /// <summary>
    /// Dictionary for tracking active sounds by their unique IDs.
    /// </summary>
    private readonly Dictionary<string, AudioSource> _activeDynamicSounds = new();
    private readonly Dictionary<string, Coroutine> _activeCompletionCoroutines = new();

    /// <summary>
    /// Cached WaitForSeconds instance for the sound cleanup interval.
    /// </summary>
    private WaitForSeconds _soundCleanupDelay;

    protected override void Awake()
    {
        base.Awake();
        InitializeSoundSystems();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Initializes the sound system.
    /// </summary>
    private void InitializeSoundSystems()
    {
        _soundCleanupDelay = new WaitForSeconds(_soundCleanupInterval);
        InitializePool();
        _activeDynamicSounds.Clear();
        _activeCompletionCoroutines.Clear();
        StopAllCoroutines();
        StartCoroutine(CleanupUnusedSources());
    }

    #region Audio Pool
    /// <summary>
    /// Initializes the object pool for managing dynamic audio sources.
    /// </summary>
    private void InitializePool()
    {
        _soundPool = new(
            createFunc: CreateAudioSource,
            actionOnGet: GetAudioSource,
            actionOnRelease: ReleasePooledAudioSource,
            actionOnDestroy: DestroyPooledAudioSource
        );
    }

    /// <summary>
    /// Creates a new audio source for the object pool.
    /// </summary>
    /// <returns>A new AudioSource instance.</returns>
    private AudioSource CreateAudioSource()
    {
        AudioSource audioSource = new GameObject($"PooledAudioSource_{Guid.NewGuid()}").AddComponent<AudioSource>();
        audioSource.transform.SetParent(_dynamicSourcesParent);
        audioSource.playOnAwake = false;
        return audioSource;
    }

    /// <summary>
    /// Activates an audio source when retrieved from the pool.
    /// </summary>
    /// <param name="source">The audio source to activate.</param>
    private void GetAudioSource(AudioSource source)
    {
        source.gameObject.SetActive(true);
        source.gameObject.hideFlags = HideFlags.None;
    }

    /// <summary>
    /// Resets and deactivates an audio source when released back to the pool.
    /// </summary>
    /// <param name="source">The audio source to reset and deactivate.</param>
    private void ReleasePooledAudioSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.loop = false;
        source.gameObject.SetActive(false);
        source.gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    /// <summary>
    /// Destroys an audio source when removed from the pool.
    /// </summary>
    /// <param name="source">The audio source to destroy.</param>
    private void DestroyPooledAudioSource(AudioSource source)
    {
        if (source.gameObject == null) return;
        source.Stop();
        Destroy(source.gameObject);
    }
    #endregion

    #region Audio API

    public static void PauseSounds()
    {
        if (Instance == null) return;
        foreach (var source in Instance._activeDynamicSounds.Values)
        {
            if (source != null && source.isPlaying) source.Pause();
        }
    }

    public static void ResumeSounds()
    {
        if (Instance == null) return;
        foreach (var source in Instance._activeDynamicSounds.Values)
        {
            if (source != null && !source.isPlaying) source.UnPause();
        }
    }

    public static void PlayDedicated(SoundConfig config, AudioSource source)
    {
        source.Stop();
        source.clip = config.GetRandomClip();
        source.volume = config.BaseVolume;
        source.pitch = config.GetPitch();
        source.loop = config.Loop;
        if (config.MixerGroup != null) source.outputAudioMixerGroup = config.MixerGroup;
        source.Play();
    }

    /// <summary>
    /// Plays a sound effect using a dynamic audio source by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sound.</param>
    /// <param name="volumeMuliplier">Multiplier for the sound's base volume.</param>
    /// <param name="loop">Indicates whether the sound should _loop.</param>
    public static void Play(SoundConfig config, string idOverride = null, Action onComplete = null)
    {
        if (Instance == null) return;
        Instance.PlaySFXDynamic(config, idOverride, onComplete);
    }

    /// <summary>
    /// Pauses a sound effect by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sound.</param>
    public static void PauseSFX(string id)
    {
        if (Instance == null) return;
        if (Instance._activeDynamicSounds.TryGetValue(id, out AudioSource source)) source.Pause();
    }

    /// <summary>
    /// Resumes a paused sound effect by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sound.</param>
    public static void ResumeSFX(string id)
    {
        if (Instance == null) return;
        if (Instance._activeDynamicSounds.TryGetValue(id, out AudioSource source)) source.UnPause();
    }

    /// <summary>
    /// Stops a sound effect by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sound.</param>
    public static void Stop(string id)
    {
        if (Instance == null) return;
        Instance.StopSoundInternal(id);
    }

    public static void StopAllSounds()
    {
        if (Instance == null) return;
        while (Instance._activeDynamicSounds.Count > 0)
        {
            var firstKey = Instance._activeDynamicSounds.Keys.First();
            Instance.StopSoundInternal(firstKey);
        }
    }

    /// <summary>
    /// Converts a percentage value to decibels.
    /// </summary>
    /// <param name="percent">The percentage value.</param>
    /// <returns>The equivalent decibel value.</returns>
    public static float PercentToDecibel(float percent) => 20 * Mathf.Log10(percent);

    /// <summary>
    /// Converts a decibel value to a percentage.
    /// </summary>
    /// <param name="decibel">The decibel value.</param>
    /// <returns>The equivalent percentage value.</returns>
    public static float DecibelToPercent(float decibel) => Mathf.Pow(10, (decibel / 20f));

    /// <summary>
    /// Gets the volume of a sound group.
    /// </summary>
    /// <param name="soundGroup">The sound group to query.</param>
    /// <param name="inDB">Indicates whether the volume should be returned in decibels.</param>
    /// <returns>The volume of the sound group.</returns>
    public static float GetVolume(SoundGroup soundGroup, bool inDB = true)
    {
        if (Instance == null) return 0;
        Instance._audioMixer.GetFloat(soundGroup.ToString(), out float volume);
        return inDB ? volume : DecibelToPercent(volume);
    }

    /// <summary>
    /// Sets the volume of a sound group.
    /// </summary>
    /// <param name="soundGroup">The sound group to modify.</param>
    /// <param name="volume">The new volume value.</param>
    /// <param name="inDB">Indicates whether the volume is specified in decibels.</param>
    public static void SetVolume(SoundGroup soundGroup, float volume, bool inDB = true)
    {
        if (Instance == null) return;
        if (!inDB) volume = PercentToDecibel(volume);
        volume = Mathf.Clamp(volume, -80, 0);
        Instance._audioMixer.SetFloat(soundGroup.ToString(), volume);
    }
    #endregion

    #region Internal Methods
    private void PlaySFXDynamic(SoundConfig config, string idOverride = null, Action onComplete = null)
    {
        if (config == null)
        {
            Debug.LogWarning("SoundConfig is null. Cannot play sound.");
            onComplete?.Invoke();
            return;
        }
        string id = idOverride ?? config.ID;
        StopSoundInternal(id);

        if (config.ClipsCount == 0)
        {
            onComplete?.Invoke();
            return;
        }

        AudioSource source = _soundPool.Get();
        if (source == null)
        {
            onComplete?.Invoke();
            return;
        }

        ConfigureAudioSource(source, config, config.BaseVolume, config.Loop);
        source.Play();

        if (_activeDynamicSounds.ContainsKey(id)) _activeDynamicSounds[id] = source;
        else _activeDynamicSounds.Add(id, source);

        if (onComplete != null && !config.Loop)
        {
            Coroutine completionCoroutine = StartCoroutine(WaitForSoundCompletion(id, source, onComplete));

            if (_activeCompletionCoroutines.ContainsKey(id)) _activeCompletionCoroutines[id] = completionCoroutine;
            else _activeCompletionCoroutines.Add(id, completionCoroutine);
        }
    }

    private IEnumerator WaitForSoundCompletion(string soundId, AudioSource source, Action onComplete)
    {
        yield return new WaitUntil(() => !source.isPlaying);

        _activeCompletionCoroutines.Remove(soundId);
        if (_activeDynamicSounds.ContainsKey(soundId)) onComplete?.Invoke();
    }

    /// <summary>
    /// Stops a sound effect by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sound.</param>
    private void StopSoundInternal(string id)
    {
        if (_activeCompletionCoroutines.TryGetValue(id, out Coroutine completionCoroutine))
        {
            if (completionCoroutine != null)
            {
                StopCoroutine(completionCoroutine);
            }
            _activeCompletionCoroutines.Remove(id);
        }

        if (_activeDynamicSounds.TryGetValue(id, out AudioSource source))
        {
            _soundPool.Release(source);
            _activeDynamicSounds.Remove(id);
        }
    }

    /// <summary>
    /// Coroutine responsible for cleaning up unused audio sources.
    /// This method runs indefinitely, checking for audio sources that are either null or not playing.
    /// If such sources are found, they are removed from the active sounds dictionary and released back to the object pool.
    /// </summary>
    /// <returns>
    /// A WaitForSeconds instance representing the interval between cleanup operations.
    /// </returns>
    private IEnumerator CleanupUnusedSources()
    {
        while (true)
        {
            yield return _soundCleanupDelay;

            List<string> inactiveList = new();
            foreach (var kvp in _activeDynamicSounds)
            {
                if (kvp.Value == null || !kvp.Value.isPlaying) inactiveList.Add(kvp.Key);
            }

            while (inactiveList.Count > 0)
            {
                string id = inactiveList[0];
                inactiveList.RemoveAt(0);
                StopSoundInternal(id);
            }
        }
    }

    /// <summary>
    /// Configures an audio source with the specified sound configuration.
    /// </summary>
    /// <param name="source">The audio source to configure.</param>
    /// <param name="config">The sound configuration to apply.</param>
    /// <param name="volumeMultiplier">Multiplier for the sound's volume.</param>
    /// <param name="loop">Indicates whether the sound should _loop.</param>
    private void ConfigureAudioSource(AudioSource source, SoundConfig config, float volumeMultiplier, bool loop)
    {
        source.clip = config.GetRandomClip();
        source.volume = config.BaseVolume * volumeMultiplier;
        source.pitch = config.GetPitch();
        source.loop = loop;
        SetSourceMixerGroup(source, config.MixerGroup);
    }

    /// <summary>
    /// Sets the audio mixer group for an audio source.
    /// </summary>
    /// <param name="source">The audio source to configure.</param>
    /// <param name="soundGroup">The sound group to assign.</param>
    private void SetSourceMixerGroup(AudioSource source, AudioMixerGroup soundGroup)
    {
        if (soundGroup != null) source.outputAudioMixerGroup = soundGroup;
    }
    #endregion
}