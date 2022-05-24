using System.Collections;
using System.Collections.Generic;
using StaticClassSettingsGame;
using UnityEngine;

[System.Serializable]
public struct AudioState
{
    public string name; // Also name of the state to change
    public AudioClip[] clips;
    public float volume;
    public float pitch; // Should be 1 by default
    public bool loop;
}

public class SFXManager : MonoBehaviour
{
    public AudioState _audioState;
    [SerializeField] AudioSource source;
    [SerializeField] SFXData _SFXData;
    bool hasDoneLoop = false;

    // Start is called before the first frame update
    void Start()
    {
        //ChangeState("Idle"); // First state is always Idle but not very useful for a mobile game
    }

    public void StopSong()
    {
        source.Stop();
    }

    void AutomaticChangeClip()
    {
        if (_audioState.clips.Length != 0)
        {
            source.clip = _audioState.clips[Random.Range(0, _audioState.clips.Length)];

            if (_audioState.loop || !hasDoneLoop) // Prevent looping (performance)
            {
                source.Play();
                hasDoneLoop = true;
                Invoke("AutomaticChangeClip", source.clip.length / ((_audioState.pitch == 0) ? 1 : _audioState.pitch));
                return;
            }
        }
        //Invoke("AutomaticChangeClip", 0.1f);
    }

    // (PlayerSettings.masterMusicLevel * PlayerSettings.fxMusicLevel) can be change in game so don't put it inside a value

    public void ChangeState(string name)
    {
        foreach (AudioState item in _SFXData.audioStates)
        {
            if (item.name == name)
            {
                _audioState = item;
                hasDoneLoop = false;
                source.loop = _audioState.loop;
                source.pitch = _audioState.pitch;
                source.volume = _audioState.volume * (PlayerSettings.masterMusicLevel * PlayerSettings.fxMusicLevel);

                CancelInvoke();
                AutomaticChangeClip();
                break;
            }
        }
    }

    public void ChangeVolume()
    {
        source.volume = _audioState.volume * (PlayerSettings.masterMusicLevel * PlayerSettings.fxMusicLevel);
    }

    public float GetVolume()
    {
        return source.volume;
    }

    public void StopSFX()
    {
        source.mute = true;
    }

    public void PlaySFX()
    {
        source.mute = false;
    }

    public string GetState()
    {
        return _audioState.name;
    }
}
