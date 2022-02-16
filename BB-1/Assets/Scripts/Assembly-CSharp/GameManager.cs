using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public struct DelayedActionHandle
	{
		public Coroutine _coroutine;

		public DelayedActionHandle(Coroutine coroutine)
		{
			_coroutine = coroutine;
		}

		public void Kill()
		{
			if (_coroutine != null)
			{
				_instance.StopCoroutine(_coroutine);
			}
		}
	}

	public string version = "";

	public GameObject movieTexture;

	public Character currentCharacter = Character.NONE;

	public GameMode currentGameMode = GameMode.NONE;

	public BossMode currentBossTrial = BossMode.NONE;

	public GameDifficulty currentDifficulty = GameDifficulty.NONE;

	public ControlStyle currentStyle;

	public GraphicsOption currentGraphicsOption = GraphicsOption.ON;

	public InvertOption currentInvertOption;

	public int currentStage = 1;

	public bool isGoingUpElevator;

	public float currentSensitivity = 0.1f;

	public string levelToLoad = "";

	public bool inTutorialRoom = true;

	public bool isLoading;

	public bool inOliverBossRoom;

	public bool inRiggsBossRoom;

	public bool inUdderMode;

	public bool FPSMode;

	public bool isHacked = true;

	public bool isIpad;

	public bool useHighres;

	public bool noBadguys;

	public bool riggsNoBuy = true;

	public bool hasAcquiredSpecial;

	public int maxGuysAlive = 100;

	public int lowEndMaxGuysAlive = 20;

	public float chaseTime = 0.5f;

	public float lowEndChaseTime = 0.75f;

	public int ocoLives;

	public int oliverUnlockedStage;

	public int riggsUnlockedStage;

	public int wilUnlockedStage;

	public int startingStage = 1;

	public bool isIntro;

	private static GameManager _instance;

	public float[] damageMultipliers;

	public float[] bossHPMultipliers;

	public string movieToPlay = "";

	public AudioClip[] musicTracks;

	public AudioClip menuMusic;

	public string menuMusicName = "music_main-menu";

	public AudioClip wilsMusic;

	public GameObject pathfinder;

	public bool vent;

	public TextureChooser textureChooser;

	private static string _onGuiLog;

	public static GameManager Instance
	{
		get
		{
			return _instance;
		}
	}

	public static void log(string text)
	{
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.textColor = Color.green;
		gUIStyle.fontSize = Screen.width / 100;
		GUI.Label(new Rect(20f, 20f, Screen.width - 10, Screen.height - 10), _onGuiLog, gUIStyle);
	}

	private void Awake()
	{
		_instance = this;
		Application.targetFrameRate = 60;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		useHighres = true;
		if (!Application.isEditor)
		{
			Debug.Log("Check for Ipad");
			isIpad = false;
		}
		if (!useHighres)
		{
			maxGuysAlive = lowEndMaxGuysAlive;
			chaseTime = lowEndChaseTime;
		}
	}

	private void Start()
	{
		SceneManager.LoadScene("MainMenu");
	}

	private IEnumerator loadMusic()
	{
		string[] array = new string[3] { "music_rooms_A", "music_rooms_B", "music_rooms_C" };
		string text = ((!useHighres) ? "Low/" : "High/");
		menuMusic = Resources.Load("Music/" + text + menuMusicName) as AudioClip;
		musicTracks = new AudioClip[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			musicTracks[i] = Resources.Load("Music/" + text + array[i]) as AudioClip;
		}
		yield return new WaitForSeconds(0.01f);
	}

	public int getDifficulty()
	{
		return (int)currentDifficulty;
	}

	public float getDamageMultiplier()
	{
		return damageMultipliers[getDifficulty()];
	}

	public void PlayMovie(string Movie)
	{
		StartCoroutine(_PlayMovie(Movie));
	}

	public IEnumerator _PlayMovie(string Movie)
	{
		SoundManager.Instance.pauseMusic();
		yield return null;
		UnityEngine.Object.Instantiate(movieTexture, base.transform.position, Quaternion.identity).GetComponent<MovieTex>().PlayMovie(Movie);
		yield return new WaitForSeconds(1f);
		SoundManager.Instance.resumeMusic();
	}

	private IEnumerator OnLevelWasLoaded()
	{
		StopCoroutine("loadMusic");
		yield return new WaitForSeconds(0.5f);
		StartCoroutine("loadMusic");
	}

	public static DelayedActionHandle RunAfterSeconds(float seconds, Action callback)
	{
		return new DelayedActionHandle(_instance.StartCoroutine(_instance.RunAfterSecondsRoutine(seconds, callback)));
	}

	public static DelayedActionHandle RunWhen(Func<bool> conditional, Action callback)
	{
		return new DelayedActionHandle(_instance.StartCoroutine(_instance.RunWhenRoutine(conditional, callback)));
	}

	public static DelayedActionHandle RunNextFrame(Action callback)
	{
		return new DelayedActionHandle(_instance.StartCoroutine(_instance.RunNextFrameRoutine(callback)));
	}

	public static DelayedActionHandle RunCoroutine(IEnumerator iEnumerator)
	{
		return new DelayedActionHandle(_instance.StartCoroutine(iEnumerator));
	}

	private IEnumerator RunAfterSecondsRoutine(float seconds, Action callback)
	{
		yield return new WaitForSeconds(seconds);
		callback();
	}

	private IEnumerator RunWhenRoutine(Func<bool> conditional, Action callback)
	{
		yield return new WaitUntil(conditional);
		callback();
	}

	private IEnumerator RunNextFrameRoutine(Action callback)
	{
		yield return null;
		callback();
	}
}
