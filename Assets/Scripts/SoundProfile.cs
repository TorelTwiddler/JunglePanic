using UnityEngine;
using System.Collections;

[System.Serializable]
public class SoundProfile
{
	[SerializeField]
	private AudioClip[] Jump;
	[SerializeField]
	private AudioClip[] Land;
	[SerializeField]
	private AudioClip[] Footstep;
	[SerializeField]
	private AudioClip[] Hit;
	[SerializeField]
	private AudioClip[] Catch;
	[SerializeField]
	private AudioClip[] Celebrate;
	[SerializeField]
	private AudioClip[] Death;
	
	public AudioClip GetRandomClip(AudioClip[] source)
	{
		if(source != null && source.Length > 0)
		{
			return source[Random.Range(0, source.Length)];
		}
		
		return null;
	}
	
	public AudioClip GetJump()
	{
		return GetRandomClip(Jump);
	}
	
	public AudioClip GetLand()
	{
		return GetRandomClip(Land);
	}
	
	public AudioClip GetFootstep()
	{
		return GetRandomClip(Footstep);
	}
	
	public AudioClip GetHit()
	{
		return GetRandomClip(Hit);
	}
	
	public AudioClip GetCatch()
	{
		return GetRandomClip(Catch);
	}
	
	public AudioClip GetCelebrate()
	{
		return GetRandomClip(Celebrate);
	}
	
	public AudioClip GetDeath()
	{
		return GetRandomClip(Death);
	}
}
