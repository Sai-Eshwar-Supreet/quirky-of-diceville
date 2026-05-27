using static SoundManager;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Represents a configuration for a specific sound in the game.
/// </summary>
[CreateAssetMenu(fileName = "AudioConfig", menuName = "Sounds/Audio Config")]
public class SoundConfig : ScriptableObject
{
    [Header("Audio Mixer Parameters")]

    /// <summary>
    /// The volume category to which the sound belongs (e.g., Master, Music, SFX).
    /// </summary>
    [SerializeField] private AudioMixerGroup _mixerGroup;

    [Header("Source Parameters")]
    [SerializeField] private bool _loop = false;
    /// <summary>
    /// The base volume level for the sound.
    /// </summary>
    [SerializeField] private float _baseVolume = 1f;

    /// <summary>
    /// Indicates whether the pitch should be randomized within the specified range.
    /// </summary>
    [SerializeField] private bool _randomizePitch = false;

    /// <summary>
    /// The range of pitch variation for the sound.
    /// </summary>
    [SerializeField] private Vector2 _pitchRange = new(0.9f, 1.1f);

    [Header("Audio Clips")]
    /// <summary>
    /// The audio clips associated with this sound configuration.
    /// </summary>
    [SerializeField] private AudioClip[] _audioClips;

    private string _id = null;
    public string ID
    {
        get
        {
            if (string.IsNullOrEmpty(_id)) _id = $"{System.Guid.NewGuid()}";
            return _id;
        }
    }

    public bool Loop => _loop;

    /// <summary>
    /// Gets the volume category of the sound.
    /// </summary>
    public AudioMixerGroup MixerGroup => _mixerGroup;

    /// <summary>
    /// Gets the base volume level for the sound.
    /// </summary>
    public float BaseVolume => _baseVolume;

    public int ClipsCount => _audioClips.Length;

    /// <summary>
    /// Retrieves a random audio clip from the associated audio clips.
    /// </summary>
    /// <returns>A random AudioClip, or null if no clips are available.</returns>
    public AudioClip GetRandomClip()
    {
        if (_audioClips == null || _audioClips.Length == 0) return null;
        return _audioClips[Random.Range(0, _audioClips.Length)];
    }

    /// <summary>
    /// Calculates the pitch for the sound, optionally randomizing it within the specified range.
    /// </summary>
    /// <returns>The calculated pitch value, clamped between -3 and 3.</returns>
    public float GetPitch()
    {
        return Mathf.Clamp(_randomizePitch ? Random.Range(_pitchRange.x, _pitchRange.y) : 1f, -3, 3);
    }
}