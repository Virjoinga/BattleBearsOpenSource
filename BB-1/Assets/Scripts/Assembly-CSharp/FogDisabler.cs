using UnityEngine;

public class FogDisabler : MonoBehaviour
{
	public void Awake()
	{
		RenderSettings.fog = false;
	}

	public void Start()
	{
		RenderSettings.fog = false;
	}

	public void OnPreRender()
	{
		RenderSettings.fog = false;
	}

	public void OnPostRender()
	{
		RenderSettings.fog = false;
	}
}
