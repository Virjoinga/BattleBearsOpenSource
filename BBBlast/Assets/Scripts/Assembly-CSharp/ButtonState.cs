using UnityEngine;

public class ButtonState : MonoBehaviour
{
	private Vector3 pressedState;

	private Vector3 normalState;

	private void Start()
	{
		normalState = base.gameObject.transform.position;
		pressedState = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z - 0.2f);
	}

	private void Update()
	{
	}

	public void Press()
	{
		base.gameObject.transform.position = pressedState;
	}

	public void Unpressed()
	{
		base.gameObject.transform.position = normalState;
	}
}
