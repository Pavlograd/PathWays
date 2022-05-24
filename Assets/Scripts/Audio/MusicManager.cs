using System.Collections;
using System.Collections.Generic;
using StaticClassSettingsGame;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source.volume = source.volume * (PlayerSettings.masterMusicLevel * PlayerSettings.fxMusicLevel);
    }
}
