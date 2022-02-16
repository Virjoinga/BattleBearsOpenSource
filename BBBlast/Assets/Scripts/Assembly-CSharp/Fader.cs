using UnityEngine;

public class Fader : MonoBehaviour
{
	private Renderer myRenderer;

	public float fadeTime = 5f;

	private float startTime;

	public bool pauseTime = true;

	private float pauseTimeScale;

	private bool fade = true;

	private float scale;

	private void Awake()
	{
		myRenderer = base.transform.Find("plane").GetComponent<Renderer>();
	}

	private void Start()
	{
		startTime = Time.realtimeSinceStartup;
		scale = 0.1f;
	}

	private void Update()
	{
		if (fade)
		{
			Color color = myRenderer.material.color;
			color.a = (Time.realtimeSinceStartup - startTime) / fadeTime;
			myRenderer.material.color = color;
		}
		else
		{
			Color color2 = myRenderer.material.color;
			color2.a = Mathf.Lerp(1f, 0f, scale);
			myRenderer.material.color = color2;
			scale += 0.01f;
		}
		if (myRenderer.material.color.a >= 1f)
		{
			fade = false;
		}
		if (myRenderer.material.color.a >= 1f && !fade)
		{
			Time.timeScale = 1f;
		}
	}
}
