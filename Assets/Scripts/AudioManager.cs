using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class AudioManager : NetworkBehaviour {

    public Sound[] sounds;
    public static AudioManager Instance { get; private set; }

	void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.playOnAwake = false;
        }		
	}
    [Command]
    public void CmdPlay(string name)
    {
        RpcPlay(name);
    }
    [ClientRpc]
    public void RpcPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    } 
}
