using System;
using System.Collections;
using UnityEngine;

public class BlackRoomController : MonoBehaviour
{
	public GameObject tronRoom;

	public GameObject slideToBlast;

	private int state;

	private GameObject stbInstance;

	private bool cleared;

	public GameObject jungle;

	public GameObject sewer;

	public GameObject cliff;

	public GameObject pirate;

	private void Start()
	{
		SoundController.Instance.FadeMusic(true);
		GameManager.Instance.tronActive = true;
		cleared = false;
		StartCoroutine("loadLevel");
		if (!GameManager.Instance.clearedEntranceExam)
		{
			stbInstance = UnityEngine.Object.Instantiate(slideToBlast) as GameObject;
			state = 1;
		}
		else
		{
			state = 0;
		}
		StartCoroutine("DelayedLoadLevel");
	}

	public IEnumerator DelayedLoadLevel()
	{
		yield return new WaitForSeconds(1.01f);
		if (GameManager.Instance.currentLevel == Level.RANDOM)
		{
			GameManager.Instance.generateLevel();
		}
		if (GameManager.Instance.currentLevel == Level.JUNGLE)
		{
			UnityEngine.Object.Instantiate(jungle);
		}
		if (GameManager.Instance.currentLevel == Level.SEWER)
		{
			UnityEngine.Object.Instantiate(sewer);
		}
		if (GameManager.Instance.currentLevel == Level.CLIFF)
		{
			UnityEngine.Object.Instantiate(cliff);
		}
		if (GameManager.Instance.currentLevel == Level.PIRATE)
		{
			UnityEngine.Object.Instantiate(pirate);
		}
	}

	private void Update()
	{
		switch (state)
		{
		case 0:
			if (cleared)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			break;
		case 1:
			if (stbInstance == null)
			{
				state = 2;
			}
			break;
		case 2:
			PlayerPrefs.SetString("clearedEntry", "true");
			GameManager.Instance.clearedEntranceExam = true;
			state = 0;
			break;
		}
	}

	public IEnumerator loadLevel()
	{
		GC.Collect();
		Resources.UnloadUnusedAssets();
		yield return new WaitForSeconds(0.1f);
		Material myMat = tronRoom.GetComponent<Renderer>().material;
		Application.LoadLevelAdditive("Game");
		while (tronRoom.GetComponent<Renderer>().material.color.a > 0.01f)
		{
			myMat.SetColor("_Color", new Color(1f, 1f, 1f, tronRoom.GetComponent<Renderer>().material.color.a - 0.05f));
			yield return new WaitForSeconds(0.08f);
		}
		yield return new WaitForSeconds(0.01f);
		SoundController.Instance.soundTrack();
		SoundController.Instance.FadeMusic(false);
		UnityEngine.Object.Destroy(tronRoom);
		cleared = true;
	}
}
