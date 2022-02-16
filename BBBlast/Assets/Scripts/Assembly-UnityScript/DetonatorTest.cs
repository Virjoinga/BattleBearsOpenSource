using System;
using UnityEngine;

[Serializable]
public class DetonatorTest : MonoBehaviour
{
	public GameObject currentDetonator;

	private int _currentExpIdx;

	private bool buttonClicked;

	public GameObject[] detonatorPrefabs;

	public float explosionLife;

	public float timeScale;

	public float detailLevel;

	public GameObject wall;

	private GameObject _currentWall;

	private int _spawnWallTime;

	private object _guiRect;

	private bool toggleBool;

	private Rect checkRect;

	public DetonatorTest()
	{
		_currentExpIdx = -1;
		explosionLife = 10f;
		timeScale = 1f;
		detailLevel = 1f;
		_spawnWallTime = -1000;
		checkRect = new Rect(0f, 0f, 260f, 180f);
	}

	public virtual void Start()
	{
		SpawnWall();
		if (!currentDetonator)
		{
			NextExplosion();
		}
		else
		{
			_currentExpIdx = 0;
		}
	}

	public virtual void OnGUI()
	{
		_guiRect = new Rect(7f, checked(Screen.height - 180), 250f, 200f);
		GUILayout.BeginArea((Rect)_guiRect);
		GUILayout.BeginVertical();
		string lhs = currentDetonator.name;
		if (GUILayout.Button(lhs + " (Click For Next)"))
		{
			NextExplosion();
		}
		if (GUILayout.Button("Rebuild Wall"))
		{
			SpawnWall();
		}
		if (GUILayout.Button("Camera Far"))
		{
			Camera.main.transform.position = new Vector3(0f, 0f, -7f);
			Camera.main.transform.eulerAngles = new Vector3(13.5f, 0f, 0f);
		}
		if (GUILayout.Button("Camera Near"))
		{
			Camera.main.transform.position = new Vector3(0f, -8.664466f, 31.38269f);
			Camera.main.transform.eulerAngles = new Vector3(1.213462f, 0f, 0f);
		}
		GUILayout.Label("Time Scale");
		timeScale = GUILayout.HorizontalSlider(timeScale, 0f, 1f);
		GUILayout.Label("Detail Level (re-explode after change)");
		detailLevel = GUILayout.HorizontalSlider(detailLevel, 0f, 1f);
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	public virtual void NextExplosion()
	{
		checked
		{
			if (_currentExpIdx >= detonatorPrefabs.Length - 1)
			{
				_currentExpIdx = 0;
			}
			else
			{
				_currentExpIdx++;
			}
			currentDetonator = detonatorPrefabs[_currentExpIdx];
		}
	}

	public virtual void SpawnWall()
	{
		if ((bool)_currentWall)
		{
			UnityEngine.Object.Destroy(_currentWall);
		}
		_currentWall = (GameObject)UnityEngine.Object.Instantiate(wall, new Vector3(-7f, -12f, 48f), Quaternion.identity);
		_spawnWallTime = checked((int)Time.time);
	}

	public virtual void Update()
	{
		_guiRect = new Rect(7f, checked(Screen.height - 150), 250f, 200f);
		if (!(Time.time + (float)_spawnWallTime <= 0.5f))
		{
			if (!checkRect.Contains(Input.mousePosition) && Input.GetMouseButtonDown(0))
			{
				SpawnExplosion();
			}
			Time.timeScale = timeScale;
		}
	}

	public virtual void SpawnExplosion()
	{
	}

	public virtual void Main()
	{
	}
}
