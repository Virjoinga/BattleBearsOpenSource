using System.Collections;
using UnityEngine;

public class AirlockController : MonoBehaviour
{
	private bool isOpen;

	private bool _startLighting;

	private float _lightingValue;

	private float _lightingSpeed = 1f;

	private Color _openColor;

	private Color _closeColor;

	public Animation animator;

	public float openTime = 20f;

	public GameObject openLight;

	public GameObject closeLight;

	public Light openLighting;

	public Light closeLighting;

	private void Awake()
	{
		openLight.SetActive(false);
		closeLight.SetActive(true);
		openLighting.gameObject.SetActive(false);
		closeLighting.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (_startLighting)
		{
			UpdateLighting();
		}
	}

	private void UpdateLighting()
	{
		_lightingValue += _lightingSpeed * Time.deltaTime;
		if (_lightingValue > 1f)
		{
			_lightingValue = 1f;
			_lightingSpeed = 0f - _lightingSpeed;
		}
		else if (_lightingValue < 0f)
		{
			_lightingValue = 0f;
			_lightingSpeed = 0f - _lightingSpeed;
		}
		openLighting.color = new Color(_openColor.r * _lightingValue, _openColor.g * _lightingValue, _openColor.b * _lightingValue);
		closeLighting.color = new Color(_closeColor.r * _lightingValue, _closeColor.g * _lightingValue, _closeColor.b * _lightingValue);
	}

	public void OnStartLightingAirlock()
	{
		_startLighting = true;
		_openColor = openLighting.color;
		_closeColor = closeLighting.color;
		openLighting.gameObject.SetActive(isOpen);
		closeLighting.gameObject.SetActive(!isOpen);
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!isOpen && c.CompareTag("Player"))
		{
			openAirlockDoors();
		}
	}

	private void OnTriggerStay(Collider c)
	{
		if (!isOpen && c.CompareTag("Player"))
		{
			openAirlockDoors();
		}
	}

	private void openAirlockDoors()
	{
		isOpen = true;
		openLight.SetActive(true);
		closeLight.SetActive(false);
		openLighting.gameObject.SetActive(isOpen);
		closeLighting.gameObject.SetActive(!isOpen);
		animator.CrossFade("open");
		StartCoroutine(delayedCloseAirlockDoors());
	}

	private IEnumerator delayedCloseAirlockDoors()
	{
		yield return new WaitForSeconds(animator["open"].length);
		yield return new WaitForSeconds(openTime);
		animator.CrossFade("close");
		yield return new WaitForSeconds(animator["close"].length);
		isOpen = false;
		openLight.SetActive(false);
		closeLight.SetActive(true);
		openLighting.gameObject.SetActive(isOpen);
		closeLighting.gameObject.SetActive(!isOpen);
	}
}
