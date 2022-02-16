using System.Collections;
using UnityEngine;

public class LoadtheBar : MonoBehaviour
{
	public GUITexture loadinggraphic;

	public GUITexture loadingbar;

	private AsyncOperation ao;

	private void Start()
	{
		loadinggraphic.pixelInset = new Rect(loadinggraphic.pixelInset.x, loadinggraphic.pixelInset.y, Screen.width, loadinggraphic.pixelInset.height);
		ao = GameManager.Instance.ao;
	}

	private void OnGUI()
	{
	}

	private void Update()
	{
		loadingbar.transform.localScale = new Vector3(ao.progress * (float)Screen.width, 0f, 0f);
	}

	private IEnumerator loadit()
	{
		while (ao.progress < 1f)
		{
			loadingbar.transform.localScale = new Vector3(ao.progress * (float)Screen.width, ao.progress * (float)Screen.width, ao.progress * (float)Screen.width);
		}
		yield return null;
	}
}
