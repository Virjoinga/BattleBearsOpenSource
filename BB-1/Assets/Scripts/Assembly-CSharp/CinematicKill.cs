using System.Collections;
using UnityEngine;

public class CinematicKill : MonoBehaviour
{
	public AudioClip enter;

	public AudioClip huggieWat;

	public AudioClip huggieMe;

	public AudioClip huggieDeath;

	public ParticleSystem peSystem;

	public void OnTriggerEnter()
	{
		GetComponent<Animation>().Play();
		StartCoroutine(sounds());
		Object.Destroy(base.gameObject.GetComponent<Collider>());
	}

	public IEnumerator sounds()
	{
		yield return new WaitForSeconds(1.1f);
		SoundManager.Instance.playSound(huggieMe);
		yield return new WaitForSeconds(0.9f);
		SoundManager.Instance.playSound(enter);
		ParticleSystem.EmissionModule emission = peSystem.emission;
		emission.enabled = true;
		yield return new WaitForSeconds(0.4f);
		emission = peSystem.emission;
		emission.enabled = true;
		SoundManager.Instance.playSound(huggieWat);
		yield return new WaitForSeconds(0.5333f);
		SoundManager.Instance.playSound(huggieDeath);
	}
}
