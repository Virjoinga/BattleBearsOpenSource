using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject obj;

	public GameObject obj2;

	private float rnd;

	private void Start()
	{
		rnd = Random.Range(1, 100);
		if (!GameManager.Instance.canHasWil)
		{
			if (rnd % 2f == 0f)
			{
				GameObject gameObject = Object.Instantiate(obj, base.transform.position, base.transform.rotation) as GameObject;
				gameObject.name = obj.name;
				gameObject.transform.parent = base.gameObject.transform.root;
				Object.Destroy(base.gameObject);
			}
			else
			{
				GameObject gameObject2 = Object.Instantiate(obj2, base.transform.position, base.transform.rotation) as GameObject;
				gameObject2.name = obj2.name;
				gameObject2.transform.parent = base.gameObject.transform.root;
				GameManager.Instance.canHasWil = true;
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			GameObject gameObject3 = Object.Instantiate(obj, base.transform.position, base.transform.rotation) as GameObject;
			gameObject3.name = obj.name;
			gameObject3.transform.parent = base.gameObject.transform.root;
			Object.Destroy(base.gameObject);
		}
	}
}
