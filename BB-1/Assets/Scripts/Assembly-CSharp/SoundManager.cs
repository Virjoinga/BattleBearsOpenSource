using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private float effectsVolume = 0.4f;

	private float musicVolume;

	private static SoundManager instance;

	private float preTempEffectsVolume = 0.4f;

	private float preTempMusicVolume = 0.4f;

	private bool isDisabled;

	private bool _lastLoopMode;

	private AudioClip _lastClip;

	private AudioSource effectsAudio;

	public AudioSource musicAudio;

	private AudioSource ambienceAudio;

	public static SoundManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		musicAudio = GetComponent<AudioSource>();
		effectsAudio = base.transform.Find("effects").GetComponent<AudioSource>();
		ambienceAudio = base.transform.Find("ambience").GetComponent<AudioSource>();
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void setTempEffectsVolume(float v)
	{
		preTempEffectsVolume = v;
	}

	public void setTempMusicVolume(float v)
	{
		preTempMusicVolume = v;
		Debug.Log(v);
	}

	public void OnTempMusicOff()
	{
		preTempEffectsVolume = effectsVolume;
		preTempMusicVolume = musicVolume;
		Debug.Log(preTempMusicVolume);
		setEffectsVolume(0f);
		setMusicVolume(0f);
		Debug.Log(preTempMusicVolume);
	}

	public void OnTempMusicOn()
	{
		Debug.Log(preTempMusicVolume);
		setEffectsVolume(preTempEffectsVolume);
		setMusicVolume(preTempMusicVolume);
	}

	public void OnTempSoundsOff()
	{
		isDisabled = true;
		preTempEffectsVolume = effectsVolume;
		preTempMusicVolume = musicVolume;
		setEffectsVolume(0f);
		setMusicVolume(0f);
	}

	public void OnTempSoundsOn()
	{
		isDisabled = false;
		setEffectsVolume(preTempEffectsVolume);
		setMusicVolume(preTempMusicVolume);
	}

	public void setEffectsVolume(float v)
	{
		if (v < 0f)
		{
			v = 0f;
		}
		effectsVolume = v;
		Component[] array = Object.FindObjectsOfType(typeof(AudioSource)) as Component[];
		for (int i = 0; i < array.Length; i++)
		{
			AudioSource audioSource = array[i] as AudioSource;
			if (audioSource != musicAudio)
			{
				audioSource.volume = v;
			}
		}
		ambienceAudio.volume = v;
		effectsAudio.volume = v;
	}

	public void setMusicVolume(float v)
	{
		if (v < 0f)
		{
			v = 0f;
		}
		musicVolume = v;
		musicAudio.volume = v;
	}

	public float getEffectsVolume()
	{
		return effectsVolume;
	}

	public float getMusicVolume()
	{
		return musicVolume;
	}

	public float getTempMusicVolume()
	{
		return preTempMusicVolume;
	}

	public void playAmbienceSound(AudioClip c)
	{
		if (effectsVolume > 0f && c != null && !isDisabled)
		{
			ambienceAudio.loop = true;
			ambienceAudio.clip = c;
			ambienceAudio.Play();
		}
	}

	public void playSound(AudioClip c)
	{
		if (effectsVolume > 0f && c != null && !isDisabled)
		{
			if (c.name == "huggables_explode_splat")
			{
				effectsAudio.PlayOneShot(c, effectsVolume / 2f);
			}
			else
			{
				effectsAudio.PlayOneShot(c, effectsVolume);
			}
		}
	}

	public void pauseMusic()
	{
		musicAudio.Pause();
	}

	public void resumeMusic()
	{
		musicAudio.Play();
	}

	public void playMusic(AudioClip c, bool loopMode)
	{
		if (musicAudio != null)
		{
			musicAudio.Stop();
		}
		StartCoroutine(delayedMusicStart(c, loopMode));
	}

	private IEnumerator delayedMusicStart(AudioClip c, bool loopMode)
	{
		yield return new WaitForSeconds(0.1f);
		musicAudio.loop = loopMode;
		musicAudio.clip = c;
		musicAudio.Play();
	}

	public void stopAll()
	{
		musicAudio.Stop();
		musicAudio.clip = null;
		effectsAudio.Stop();
		effectsAudio.clip = null;
		ambienceAudio.Stop();
		ambienceAudio.clip = null;
	}

	private void OnLevelWasLoaded()
	{
		stopAll();
	}
}
