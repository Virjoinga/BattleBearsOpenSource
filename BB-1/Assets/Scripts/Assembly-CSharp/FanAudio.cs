using UnityEngine;

public class FanAudio : MonoBehaviour
{
	public Transform player;

	public Transform[] fanArray;

	private float changeInDistance;

	private float minDistance = float.PositiveInfinity;

	private AudioSource audio;

	private void Start()
	{
		player = GameObject.Find("Wil_FIx").transform;
		audio = base.gameObject.GetComponent<AudioSource>();
	}

	private void Update()
	{
		Transform[] array = fanArray;
		foreach (Transform transform in array)
		{
			changeInDistance = Vector3.Distance(transform.position, player.position);
			if (minDistance > changeInDistance)
			{
				minDistance = changeInDistance;
			}
		}
		audio.volume = SoundManager.Instance.getEffectsVolume() * (5f / minDistance);
		minDistance = float.PositiveInfinity;
	}
}
