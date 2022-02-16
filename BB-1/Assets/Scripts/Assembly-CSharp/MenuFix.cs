using UnityEngine;
using UnityEngine.UI;

public class MenuFix : MonoBehaviour
{
	public Image[] texture;

	private void Awake()
	{
		texture = base.gameObject.GetComponentsInChildren<Image>();
		Initialize();
	}

	private void Initialize()
	{
		float num = (float)Screen.width / 480f;
		float num2 = (float)Screen.height / 320f;
		Image[] array = texture;
		foreach (Image image in array)
		{
			Rect pixelAdjustedRect = image.GetPixelAdjustedRect();
			if ((bool)image)
			{
				image.SetClipRect(new Rect(pixelAdjustedRect.x * num, pixelAdjustedRect.y * num2, pixelAdjustedRect.width * num, pixelAdjustedRect.height * num2), true);
			}
		}
	}
}
