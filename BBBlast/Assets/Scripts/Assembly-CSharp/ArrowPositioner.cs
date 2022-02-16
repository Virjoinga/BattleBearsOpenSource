using UnityEngine;

public class ArrowPositioner : MonoBehaviour
{
	public float xPos;

	private float initXPos;

	private float initYPos;

	private float initZPos;

	private float yPos;

	private float zPos;

	private void Start()
	{
		initXPos = base.gameObject.transform.localPosition.x;
		initYPos = base.gameObject.transform.localPosition.y;
		initZPos = base.gameObject.transform.localPosition.z;
		xPos = initXPos;
		zPos = initZPos;
	}

	private void Update()
	{
		yPos = 0.025f * Mathf.Pow(xPos, 2f);
		base.gameObject.transform.localPosition = new Vector3(initXPos - xPos, initYPos - yPos, zPos);
	}
}
