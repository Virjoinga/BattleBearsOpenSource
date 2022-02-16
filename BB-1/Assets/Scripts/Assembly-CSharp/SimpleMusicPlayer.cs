using UnityEngine;

public class SimpleMusicPlayer : MonoBehaviour
{
	public string musicToPlay;

	private void Start()
	{
		bool useHighre = GameManager.Instance.useHighres;
	}
}
