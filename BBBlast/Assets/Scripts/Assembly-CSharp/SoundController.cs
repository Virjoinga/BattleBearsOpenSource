using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	public AudioSource effectsAudio;

	public AudioClip[] cache;

	public Dictionary<string, AudioClip> cacheDic;

	[HideInInspector]
	public AudioSource musicAudio;

	private Object[] huggableDeath;

	[HideInInspector]
	public bool musicOff;

	[HideInInspector]
	public bool allOff;

	private int arpeggioIndex = 1;

	private static SoundController instance;

	public bool isiPodMusicPlaying;

	public static SoundController Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		musicAudio = base.GetComponent<AudioSource>();
		cacheDic = new Dictionary<string, AudioClip>();
		AudioClip[] array = cache;
		foreach (AudioClip audioClip in array)
		{
			cacheDic.Add(audioClip.name.ToString(), audioClip);
		}
	}

	public void PlayMySound(string name)
	{
		if (cacheDic.ContainsKey(name))
		{
			playClip(cacheDic[name]);
		}
		else if (name.ToLower().Contains("abbi"))
		{
			playClip(Resources.Load("Abbi_Sounds/" + name) as AudioClip);
		}
	}

	private void Start()
	{
		if (PlayerPrefs.GetInt("music") != 0)
		{
			muteMusic();
		}
		if (PlayerPrefs.GetInt("sound") != 0)
		{
			muteAll();
		}
		huggableDeath = Resources.LoadAll("Huggablesound");
	}

	public void playClip(AudioClip clip)
	{
		effectsAudio.PlayOneShot(clip);
		clip = null;
	}

	public void deathScale(int num)
	{
		int num2 = Random.Range(0, huggableDeath.Length);
		playClip(huggableDeath[num2] as AudioClip);
	}

	public IEnumerator arpeggio(AudioClip c)
	{
		yield return new WaitForSeconds(0.01f * (float)arpeggioIndex);
		effectsAudio.PlayOneShot(c);
	}

	public IEnumerator arpeggioReset()
	{
		yield return new WaitForSeconds(0.02f * (float)arpeggioIndex);
		arpeggioIndex = 1;
	}

	public void muteMusic()
	{
		if (!allOff)
		{
			if (musicOff)
			{
				if (!isiPodMusicPlaying)
				{
					musicAudio.volume = 0.5f;
				}
				musicOff = false;
				PlayerPrefs.SetInt("music", 0);
			}
			else
			{
				musicAudio.volume = 0f;
				musicOff = true;
				PlayerPrefs.SetInt("music", 1);
			}
		}
		else
		{
			musicOff = !musicOff;
		}
	}

	public void muteAll()
	{
		if (allOff)
		{
			if (!musicOff && !isiPodMusicPlaying)
			{
				musicAudio.volume = 0.5f;
			}
			effectsAudio.volume = 0.75f;
			GameManager.Instance.allMute = false;
			allOff = false;
			PlayerPrefs.SetInt("sound", 0);
		}
		else
		{
			musicAudio.volume = 0f;
			effectsAudio.volume = 0f;
			GameManager.Instance.allMute = true;
			allOff = true;
			PlayerPrefs.SetInt("sound", 1);
		}
	}

	public void soundTrack()
	{
		musicAudio.loop = true;
		if (GameManager.Instance.currentGameMode == GameMode.MENU)
		{
			musicAudio.clip = Resources.Load("Music_MainMenu2") as AudioClip;
		}
		else if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL || GameManager.Instance.currentGameMode == GameMode.TIME || GameManager.Instance.currentGameMode == GameMode.SCORE)
		{
			switch (GameManager.Instance.currentLevel)
			{
			case Level.SEWER:
				musicAudio.clip = Resources.Load("music_SewerIndustrial") as AudioClip;
				break;
			case Level.JUNGLE:
				musicAudio.clip = Resources.Load("music_Jungle") as AudioClip;
				break;
			case Level.CLIFF:
				musicAudio.clip = Resources.Load("music_Desert") as AudioClip;
				break;
			case Level.PIRATE:
				musicAudio.clip = Resources.Load("music_PiratesTechno") as AudioClip;
				break;
			}
		}
		else
		{
			musicAudio.clip = Resources.Load("Music_MainMenu2") as AudioClip;
		}
		musicAudio.Play();
	}

	public bool isMusicOn()
	{
		return !musicOff;
	}

	public bool isAllOn()
	{
		return !allOff;
	}

	public void FadeMusic(bool fadeOut)
	{
		StopCoroutine("FadeMusicOut");
		StopCoroutine("FadeMusicIn");
		if (fadeOut)
		{
			StartCoroutine("FadeMusicOut");
		}
		else
		{
			StartCoroutine("FadeMusicIn");
		}
	}

	public IEnumerator FadeMusicOut()
	{
		if (!musicOff && !allOff)
		{
			while (musicAudio.volume > 0f && !musicOff && !allOff && !isiPodMusicPlaying)
			{
				musicAudio.volume -= Time.deltaTime / 2f;
				yield return new WaitForSeconds(0.08f);
			}
		}
	}

	public IEnumerator FadeMusicIn()
	{
		if (musicOff || allOff)
		{
			yield break;
		}
		while (musicAudio.volume < 0.5f)
		{
			if (!musicOff && !allOff && !isiPodMusicPlaying)
			{
				musicAudio.volume += Time.deltaTime / 2f;
				yield return new WaitForSeconds(0.08f);
				continue;
			}
			soundTrack();
			break;
		}
	}
}
