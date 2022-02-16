using UnityEngine;

public class SavedDataManager : MonoBehaviour
{
	private static SavedDataManager instance;

	public static SavedDataManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		loadPreferences();
	}

	public void loadPreferences()
	{
		SoundManager.Instance.setEffectsVolume(PlayerPrefs.GetFloat("soundVolume", 0.5f));
		SoundManager.Instance.setMusicVolume(PlayerPrefs.GetFloat("musicVolume", 0.5f));
		GameManager.Instance.currentStyle = (ControlStyle)PlayerPrefs.GetInt("controlStyle", 0);
		if (PlayerPrefs.HasKey("graphicsOption"))
		{
			GameManager.Instance.currentGraphicsOption = (GraphicsOption)PlayerPrefs.GetInt("graphicsOption", 0);
		}
		else
		{
			Debug.Log("check to set the graphics option: Default on");
			GameManager.Instance.currentGraphicsOption = GraphicsOption.ON;
		}
		GameManager.Instance.currentInvertOption = (InvertOption)PlayerPrefs.GetInt("invertOption", 0);
		GameManager.Instance.currentSensitivity = PlayerPrefs.GetFloat("sensitivity", 0.1f);
		GameManager.Instance.oliverUnlockedStage = PlayerPrefs.GetInt("oliverUnlockedStage", 0);
		GameManager.Instance.riggsUnlockedStage = PlayerPrefs.GetInt("riggsUnlockedStage", 0);
		GameManager.Instance.wilUnlockedStage = PlayerPrefs.GetInt("wilUnlockedStage", 0);
		GameManager.Instance.startingStage = PlayerPrefs.GetInt("startingStage", 1);
	}

	public void savePreferences()
	{
		PlayerPrefs.SetFloat("soundVolume", SoundManager.Instance.getEffectsVolume());
		PlayerPrefs.SetFloat("musicVolume", SoundManager.Instance.getMusicVolume());
		PlayerPrefs.SetInt("controlStyle", (int)GameManager.Instance.currentStyle);
		PlayerPrefs.SetInt("graphicsOption", (int)GameManager.Instance.currentGraphicsOption);
		PlayerPrefs.SetInt("invertOption", (int)GameManager.Instance.currentInvertOption);
		PlayerPrefs.SetFloat("sensitivity", GameManager.Instance.currentSensitivity);
		PlayerPrefs.SetInt("startingStage", GameManager.Instance.startingStage);
	}

	public void saveState()
	{
		PlayerPrefs.SetInt("character", (int)GameManager.Instance.currentCharacter);
		PlayerPrefs.SetInt("gameMode", (int)GameManager.Instance.currentGameMode);
		PlayerPrefs.SetInt("difficulty", (int)GameManager.Instance.currentDifficulty);
		PlayerPrefs.SetInt("currentStage", GameManager.Instance.currentStage);
		PlayerPrefs.SetInt("hasSpecial", GameManager.Instance.hasAcquiredSpecial ? 1 : 0);
	}

	public void loadState()
	{
		GameManager.Instance.isLoading = true;
		GameManager.Instance.currentCharacter = (Character)PlayerPrefs.GetInt("character", 0);
		GameManager.Instance.currentGameMode = (GameMode)PlayerPrefs.GetInt("gameMode", 0);
		GameManager.Instance.currentDifficulty = (GameDifficulty)PlayerPrefs.GetInt("difficulty", 0);
		GameManager.Instance.currentStage = PlayerPrefs.GetInt("currentStage", 1);
		GameManager.Instance.hasAcquiredSpecial = PlayerPrefs.GetInt("hasSpecial", 0) == 1;
		GameManager.Instance.inTutorialRoom = false;
	}
}
