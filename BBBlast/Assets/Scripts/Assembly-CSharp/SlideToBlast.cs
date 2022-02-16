using System.Collections;
using UnityEngine;

public class SlideToBlast : MonoBehaviour
{
	private void Start()
	{
		GameManager.Instance.isLevel = false;
		SoundController.Instance.playClip(Resources.Load("Abbi Sounds/abbi_slide") as AudioClip);
		StartCoroutine("slide");
	}

	public IEnumerator slide()
	{
		base.GetComponent<Animation>().Play("SlideToBlast");
		yield return new WaitForSeconds(base.GetComponent<Animation>()["SlideToBlast"].length);
		Object.Destroy(base.gameObject);
	}
}
