using UnityEngine;

public class MinimapController : MonoBehaviour
{
	public Vector2 dimensions;

	public float xSpacing = 1.875f;

	public float ySpacing = 1.875f;

	public Vector2 currentPos;

	private Renderer[,] blockers;

	private Transform mapBlockerRoot;

	private Transform myTransform;

	private void Awake()
	{
		myTransform = base.transform;
		blockers = new Renderer[(int)dimensions.x, (int)dimensions.y];
		mapBlockerRoot = myTransform.Find("Minimap/mapBlockers");
		for (int i = 0; i < (int)dimensions.x; i++)
		{
			for (int j = 0; j < (int)dimensions.y; j++)
			{
				blockers[i, j] = mapBlockerRoot.Find(i + "," + j).GetComponent<Renderer>();
				if (i == (int)currentPos.x && j == (int)currentPos.y)
				{
					blockers[i, j].enabled = false;
				}
			}
		}
		if (GameManager.Instance.isLoading)
		{
			loadData();
		}
	}

	public void OnSaveData()
	{
		string text = "";
		for (int i = 0; i < (int)dimensions.x; i++)
		{
			for (int j = 0; j < (int)dimensions.y; j++)
			{
				if (!blockers[i, j].enabled)
				{
					text = text + i + "," + j + " ";
				}
			}
		}
		PlayerPrefs.SetString("minimapData", text);
		PlayerPrefs.SetFloat("minimapPosX", currentPos.x);
		PlayerPrefs.SetFloat("minimapPosY", currentPos.y);
	}

	public void loadData()
	{
		string[] array = PlayerPrefs.GetString("minimapData", "").Split(' ');
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i] == ""))
			{
				string[] array2 = array[i].Split(',');
				blockers[int.Parse(array2[0]), int.Parse(array2[1])].enabled = false;
			}
		}
		currentPos.x = PlayerPrefs.GetFloat("minimapPosX", 0f);
		currentPos.y = PlayerPrefs.GetFloat("minimapPosY", 0f);
	}

	public void OnMove(ExitDirection dir)
	{
		switch (dir)
		{
		case ExitDirection.NORTH:
			currentPos.y -= 1f;
			break;
		case ExitDirection.EAST:
			currentPos.x -= 1f;
			break;
		case ExitDirection.SOUTH:
			currentPos.y += 1f;
			break;
		case ExitDirection.WEST:
			currentPos.x += 1f;
			break;
		}
		blockers[(int)currentPos.x, (int)currentPos.y].enabled = false;
	}
}
