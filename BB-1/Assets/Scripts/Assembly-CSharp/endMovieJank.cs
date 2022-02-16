using UnityEngine;

public class endMovieJank : MonoBehaviour
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
		myRenderer.sharedMaterial.color = new Color(0f, 0f, 0f, 0f);
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
		color.a -= (Time.realtimeSinceStartup - startTime) / fadeTime;
		myRenderer.material.color = color;
		if (color.a < 0f)
		{
			if (pauseTime)
			{
				Time.timeScale = pauseTimeScale;
			}
			base.enabled = false;
		}
	}
}
