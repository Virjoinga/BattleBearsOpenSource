using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
	[SerializeField]
	private Slider LoadingSlider;

	private float TimeLoading = 2f;

	private float _curTimeLoading;

	private bool _loadingDone;

	public static LoadingCanvas Instance;

	public bool LoadingDone
	{
		get
		{
			return _loadingDone;
		}
	}

	private void UpdateLoading()
	{
		_curTimeLoading += Time.deltaTime;
		LoadingSlider.value = _curTimeLoading / TimeLoading;
		if (_curTimeLoading >= TimeLoading + 0.3f)
		{
			_loadingDone = true;
			base.gameObject.SetActive(false);
		}
	}

	private void Awake()
	{
		Instance = this;
		LoadingSlider.value = 0f;
		_curTimeLoading = 0f;
		TimeLoading = Random.Range(1.6f, 2f);
	}

	private void Start()
	{
	}

	private void Update()
	{
		UpdateLoading();
	}
}
