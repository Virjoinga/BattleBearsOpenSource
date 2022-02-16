using UnityEngine;

public class TutorialSkip : MonoBehaviour
{
	public bool isBack;

	public void skip()
	{
		if (!isBack)
		{
			SendMessageUpwards("skipEvent", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			SendMessageUpwards("backEvent", SendMessageOptions.DontRequireReceiver);
		}
		base.GetComponent<Animation>().Play("SkipPress");
	}
}
