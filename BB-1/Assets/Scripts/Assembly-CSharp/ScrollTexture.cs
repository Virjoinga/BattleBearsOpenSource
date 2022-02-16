using System;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
	public int matIndex;

	public string[] textureNames;

	public Vector2 scrollRate = new Vector2(3f, 0.2f);

	public Vector2 minOffset = new Vector2(0f, 0f);

	public Vector2 maxOffset = new Vector2(0f, 0f);

	public float period = 1f;

	private float noOscillateOffset;

	private float fakeTime;

	private Renderer myRenderer;

	private Vector2 offset;

	private void Awake()
	{
		myRenderer = GetComponent<Renderer>();
		if (textureNames.Length == 0)
		{
			textureNames = new string[1];
			textureNames[0] = "_MainTex";
		}
	}

	private void Update()
	{
		if (Time.timeScale == 0f)
		{
			fakeTime += 0.05f;
		}
		if (period > 0f)
		{
			float num = ((!(Time.timeScale > 0f)) ? (0.5f * Mathf.Cos(fakeTime / period * 90f * ((float)Math.PI / 180f)) + 0.5f) : (0.5f * Mathf.Cos(Time.time / period * 90f * ((float)Math.PI / 180f)) + 0.5f));
			offset = (1f - num) * minOffset + num * maxOffset;
		}
		if (Time.timeScale > 0f)
		{
			if (period > 0f)
			{
				offset += Time.time * scrollRate;
			}
			else
			{
				offset += Time.deltaTime * scrollRate;
			}
		}
		else
		{
			offset += fakeTime * scrollRate;
		}
		if (offset.x > 1f)
		{
			offset.x -= 1f;
		}
		if (offset.y > 1f)
		{
			offset.y -= 1f;
		}
		if (offset.y < -1f)
		{
			offset.y += 1f;
		}
		if (offset.x < -1f)
		{
			offset.x += 1f;
		}
		string[] array = textureNames;
		foreach (string text in array)
		{
			myRenderer.sharedMaterials[matIndex].SetTextureOffset(text, offset);
		}
	}
}
