using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour{
	
	public enum AUDIOTEMPLATE
	{
		AMBIENT,
		MUSIC,
		SFX,
		FOOTSTEP,
		JUMP,
	}
	
	[SerializeField]
	List<AUDIOTEMPLATE> TemplateKeys;
	[SerializeField]
	List<AudioSource> TemplateValues;
	
	public Dictionary<AUDIOTEMPLATE, AudioSource> AudioTempates = new Dictionary<AUDIOTEMPLATE, AudioSource>();
	
	private static AudioManager s_instance;
	
	public static AudioManager Get()
	{
		return s_instance;
	}
	
	private List<AudioSource> m_allSounds = new List<AudioSource>();
	
	public AudioClip[] m_playerFootsteps;
	
	public AudioClip[] m_playerJumps;
	
	void Awake()
	{
		s_instance = this;
		DontDestroyOnLoad(gameObject);
		
		for(int i = 0; i < TemplateKeys.Count; i++)
		{
			AudioTempates.Add(TemplateKeys[i], TemplateValues[i]);
		}
	}
	
	void Start()
	{
		
	}
	
	void Update()
	{
		for(int i = 0; i < m_allSounds.Count; i++)
		{
			AudioSource addedSound = m_allSounds[i];
			if(!addedSound.isPlaying)
			{
				Destroy(addedSound);
				m_allSounds.RemoveAt(i);
				i--;
			}
		}
	}
	
	public void StopAllSounds()
	{
		for(int i = 0; i < m_allSounds.Count; i++)
		{
			AudioSource addedSound = m_allSounds[i];
			Destroy(addedSound);
			m_allSounds.RemoveAt(i);
			i--;
		}
	}
	
	public AudioSource PlaySound(AudioClip soundClip)
	{		
	 	return PlaySound(AUDIOTEMPLATE.SFX, soundClip);
	}
	
	public AudioSource PlaySound( AUDIOTEMPLATE template, AudioClip soundClip)
	{
		AudioSource newAudio = s_instance.gameObject.AddComponent<AudioSource>();
		newAudio.clip = soundClip;
		
		SetSourceToTemplate(template, newAudio);
		
		newAudio.Play();
		m_allSounds.Add(newAudio);
		return newAudio;
	}
	
	private void SetSourceToTemplate(AUDIOTEMPLATE template, AudioSource source)
	{
		AudioSource templateSource = AudioTempates[template];
		source.dopplerLevel = templateSource.dopplerLevel;
		source.ignoreListenerPause = templateSource.ignoreListenerPause;
		source.ignoreListenerVolume = templateSource.ignoreListenerVolume;
		source.loop = templateSource.loop;
		source.maxDistance = templateSource.maxDistance;
		source.minDistance = templateSource.minDistance;
		source.pan = templateSource.pan;
		source.panLevel = templateSource.panLevel;
		if(template == AUDIOTEMPLATE.FOOTSTEP || template == AUDIOTEMPLATE.JUMP)
		{
			source.pitch = Random.Range(0.5f, 2.0f);
		}
		else
		{
			source.pitch = templateSource.pitch;
		}
		source.playOnAwake = templateSource.playOnAwake;
		source.priority = templateSource.priority;
		source.rolloffMode = templateSource.rolloffMode;
		source.spread = templateSource.spread;
		source.timeSamples = templateSource.timeSamples;
		source.velocityUpdateMode = templateSource.velocityUpdateMode;
		source.volume = templateSource.volume;
	}
	
	public bool StopSound(AudioClip soundClip)
	{
		List<AudioSource> allMatches = m_allSounds.FindAll(x => x.clip == soundClip);

		foreach(AudioSource source in allMatches)
		{
			source.Stop();
		}
		
		return false;
	}
	
	public AudioClip GetRandomFootstep()
	{
		return s_instance.m_playerFootsteps[Random.Range(0, s_instance.m_playerFootsteps.Length)];
	}
	
	
}
