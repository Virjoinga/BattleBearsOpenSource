using System.Collections;
using UnityEngine;

public class SoundTrackController : MonoBehaviour
{
	private int currentTrack;

	private static SoundTrackController instance;

	private bool playingMusicList;

	public static SoundTrackController Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		currentTrack = Random.Range(0, GameManager.Instance.musicTracks.Length);
	}

	public void playNextMusicTrack()
	{
		SoundManager.Instance.playMusic(GameManager.Instance.musicTracks[currentTrack], true);
		currentTrack++;
		if (currentTrack >= GameManager.Instance.musicTracks.Length)
		{
			currentTrack = 0;
		}
	}

	public void startMusicList()
	{
		if (!playingMusicList && currentTrack < GameManager.Instance.musicTracks.Length)
		{
			playingMusicList = true;
			StartCoroutine("playMusicList");
		}
	}

	public void stopMusicList()
	{
		if (playingMusicList)
		{
			playingMusicList = false;
			StopCoroutine("playMusicList");
		}
	}

	private IEnumerator playMusicList()
	{
		while (currentTrack < GameManager.Instance.musicTracks.Length)
		{
			Debug.Log("playing music list song: " + GameManager.Instance.musicTracks[currentTrack].name);
			SoundManager.Instance.playMusic(GameManager.Instance.musicTracks[currentTrack], false);
			currentTrack++;
			if (currentTrack >= GameManager.Instance.musicTracks.Length)
			{
				currentTrack = 0;
			}
			yield return new WaitForSeconds(GameManager.Instance.musicTracks[currentTrack].length);
		}
	}
}
