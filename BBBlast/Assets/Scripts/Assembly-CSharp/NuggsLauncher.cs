using UnityEngine;

public class NuggsLauncher : MonoBehaviour
{
	public GameObject[] nuggs;

	public Transform[] startPoints;

	public Transform[] endPoints;

	private int num;

	private float pointThreshold = 250f;

	private void Start()
	{
		num = Random.Range(0, startPoints.Length);
	}

	private void Update()
	{
		if (GameManager.Instance.hudController.scoreVal > pointThreshold)
		{
			if (Random.Range(1, 100) > -50 && GameManager.Instance.canHasNuggs)
			{
				Quaternion identity = Quaternion.identity;
				identity.eulerAngles = new Vector3(90f, 90f, 90f);
				int num = Random.Range(0, nuggs.Length);
				GameObject gameObject = Object.Instantiate(nuggs[num], startPoints[this.num].position, identity) as GameObject;
				gameObject.name = nuggs[num].name;
				gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				this.num = Random.Range(0, startPoints.Length);
				GameManager.Instance.canHasNuggs = false;
			}
			pointThreshold += 250f;
		}
	}
}
