using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour{

	private static AudioManager s_instance;
	
	public static AudioManager Get()
	{
		if(s_instance == null)
		{
			s_instance = (new GameObject("AudioManager")).AddComponent<AudioManager>();
		}
		return s_instance ;
	}
	
	private Dictionary<string, AudioSource> m_allSounds = new Dictionary<string, AudioSource>();
	
	void Start()
	{
	}
	
	void Update()
	{
		foreach(string key in m_allSounds.Keys)
		{
			AudioSource addedSound = m_allSounds[key];
			if(!addedSound.isPlaying)
			{
				//Destroy(addedSound);
			}
		}
	}
	
	public void StopAllSounds()
	{
		foreach(string key in m_allSounds.Keys)
		{
			AudioSource addedSound = m_allSounds[key];
			if(!addedSound.isPlaying)
			{
				Destroy(addedSound);
			}
		}
	}
	
	public string GenerateUniqueKey()
	{
		int intOffset = 0;
		System.Diagnostics.StackFrame returnStack = new System.Diagnostics.StackFrame(2);
		string returnString = string.Format("Line({0}):{1}-{2}",returnStack.GetFileLineNumber(), returnStack.GetMethod().Name, intOffset);
		while(m_allSounds.ContainsKey(returnString))
		{
			returnString = string.Format("Line({0}):{1}-{2}",returnStack.GetFileLineNumber(), returnStack.GetMethod().Name,++intOffset);
		}
		
		return returnString;
	}

	public AudioSource PlaySound(AudioClip soundClip)
	{
		return PlaySound(GenerateUniqueKey(), soundClip);
	}
	
	public AudioSource PlaySound(string key, AudioClip soundClip)
	{
		if(m_allSounds.ContainsKey(key))
		{
			m_allSounds[key].Play();
			return m_allSounds[key];
		}
		
	 	AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = soundClip;
		
		newAudio.Play();
		m_allSounds.Add(key, newAudio);
		return newAudio;
	}
	
	public bool StopSound(string key, AudioClip soundClip)
	{
		if(m_allSounds.ContainsKey(key))
		{
			m_allSounds[key].Stop();
			return true;
		}
		
		return false;
	}
	
	
}
