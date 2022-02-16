using UnityEngine;

public class retinaChecker : MonoBehaviour
{
	public GUITexture myCrossHairs;

	private void Awake()
	{
		if (Screen.width > 321)
		{
			myCrossHairs.pixelInset = new Rect(base.GetComponent<GUITexture>().pixelInset.x, base.GetComponent<GUITexture>().pixelInset.y, 96f, 96f);
			Object.Destroy(this);
		}
		else
		{
			myCrossHairs.pixelInset = new Rect(base.GetComponent<GUITexture>().pixelInset.x, base.GetComponent<GUITexture>().pixelInset.y, 48f, 48f);
			Object.Destroy(this);
		}
	}
}
