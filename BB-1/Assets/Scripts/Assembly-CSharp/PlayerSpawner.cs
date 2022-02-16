using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	public GameObject oliver;

	public GameObject riggs;

	public GameObject wil;

	public int numLives = 1;

	private void Awake()
	{
		GameObject gameObject = null;
		switch (GameManager.Instance.currentCharacter)
		{
		case Character.OLIVER:
			gameObject = oliver;
			break;
		case Character.RIGGS:
			gameObject = riggs;
			break;
		case Character.WIL:
			gameObject = wil;
			break;
		}
		GameObject gameObject2 = null;
		if (gameObject != null)
		{
			gameObject2 = Object.Instantiate(gameObject);
			gameObject2.name = gameObject.name;
			gameObject2.transform.position = base.transform.position;
			gameObject2.transform.rotation = base.transform.rotation;
		}
		else
		{
			Debug.Log("no character selected! spawning oliver by default, this is not desired behaviour!");
			gameObject2 = Object.Instantiate(oliver);
			gameObject2.name = oliver.name;
			gameObject2.transform.position = base.transform.position;
			gameObject2.transform.rotation = base.transform.rotation;
		}
		(gameObject2.GetComponent(typeof(PlayerController)) as PlayerController).numLives = numLives;
		Object.Destroy(base.gameObject);
	}
}
