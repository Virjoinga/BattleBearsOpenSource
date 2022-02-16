using UnityEngine;

public class ButtonContType2 : MonoBehaviour
{
	public GameObject myGameObject;

	public void clicked()
	{
		SendMessageUpwards("ButtonPress", base.gameObject.name);
	}

	public void selected()
	{
		myGameObject.GetComponent<Renderer>().enabled = false;
	}

	public void unSelected()
	{
		myGameObject.GetComponent<Renderer>().enabled = true;
	}
}
