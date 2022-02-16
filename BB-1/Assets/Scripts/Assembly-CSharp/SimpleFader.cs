using UnityEngine;

public class SimpleFader : MonoBehaviour
{
	private Renderer myRenderer;

	private float currentAlpha;

	public float fadeTime = 5f;

	private float startTime;

	public bool pauseTime = true;

	private float pauseTimeScale;

	private void Awake()
	{
		myRenderer = base.transform.Find("plane").GetComponent<Renderer>();
	}

	private void Start()
	{
		if (pauseTime)
		{
			pauseTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}
		startTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		Color color = myRenderer.material.color;
		color.a = (Time.realtimeSinceStartup - startTime) / fadeTime;
		myRenderer.material.color = color;
		if (color.a > 1f)
		{
			if (pauseTime)
			{
				Time.timeScale = pauseTimeScale;
			}
			base.enabled = false;
		}
	}
}
