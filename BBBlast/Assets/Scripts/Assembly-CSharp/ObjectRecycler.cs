using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectRecycler
{
	private List<GameObject> objectList;

	private GameObject objectToRecycle;

	public GameObject nextFree
	{
		get
		{
			GameObject gameObject = objectList.Where((GameObject item) => !item.active).FirstOrDefault();
			if (gameObject == null)
			{
				gameObject = Object.Instantiate(objectToRecycle) as GameObject;
				objectList.Add(gameObject);
			}
			//gameObject.active = true;
			gameObject.SetActive(true);
			return gameObject;
		}
	}

	public ObjectRecycler(GameObject go, int totalObjectAtStart)
	{
		objectList = new List<GameObject>(totalObjectAtStart);
		objectToRecycle = go;
		for (int i = 0; i < totalObjectAtStart; i++)
		{
			GameObject gameObject = Object.Instantiate(go) as GameObject;
			//gameObject.active = false;
			gameObject.SetActive(false);
			//gameObject.SetActiveRecursively(false);
			gameObject.SetActive(false);
			objectList.Add(gameObject);
		}
	}

	private void fireRecycledEvent()
	{
	}

	public void freeObject(GameObject objectToFree)
	{
		//objectToFree.gameObject.active = false;
		objectToFree.gameObject.SetActive(false);
	}
}
