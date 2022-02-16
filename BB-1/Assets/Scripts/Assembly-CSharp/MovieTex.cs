using System.Collections;
using BSCore;
using UnityEngine;
using UnityEngine.Video;

public class MovieTex : MonoBehaviour
{
	public VideoPlayer videoPlayer;

	public AudioSource As;

	private Camera _cam;

	private bool playing;

	private void Awake()
	{
		_cam = GetComponent<Camera>();
		_cam.enabled = false;
	}

	private void Start()
	{
		As.volume = 1f;
		Time.timeScale = 0f;
		Object.DontDestroyOnLoad(base.gameObject);
		videoPlayer.errorReceived += ErrorEventHandler;
	}

	public void ErrorEventHandler(VideoPlayer source, string message)
	{
		GameManager.log("MovieTex.ErrorEventHandler message=" + message);
	}

	private void Update()
	{
		if (BSCoreInput.GetButton(Option.Clear) || BSCoreInput.GetButton(Option.Fire))
		{
			if (videoPlayer != null)
			{
				videoPlayer.Stop();
			}
			StopCoroutine(PlayVideo());
			VideoFinished();
		}
	}

	public void PlayMovie(string videoFile)
	{
		if (videoFile.Equals("r_loss_standard"))
		{
			VideoClip clip = Resources.Load<VideoClip>("Video/r_loss_standard");
			videoPlayer.clip = clip;
			videoPlayer.source = VideoSource.VideoClip;
		}
		else
		{
			videoPlayer.source = VideoSource.Url;
			string url = Application.streamingAssetsPath + "/" + videoFile + ".mp4";
			videoPlayer.url = url;
		}
		videoPlayer.isLooping = false;
		StartCoroutine(PlayVideo());
	}

	private IEnumerator PlayVideo()
	{
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoPlayer.controlledAudioTrackCount = 1;
		videoPlayer.EnableAudioTrack(0, true);
		videoPlayer.SetTargetAudioSource(0, As);
		videoPlayer.Prepare();
		float timePrepare = 3f;
		while (!videoPlayer.isPrepared)
		{
			timePrepare -= Time.unscaledDeltaTime;
			if (!(timePrepare > 0f))
			{
				break;
			}
			yield return null;
		}
		_cam.enabled = true;
		videoPlayer.Play();
		playing = true;
		while (videoPlayer.isPlaying)
		{
			yield return null;
		}
		Debug.LogFormat("frame = {0} - framecount = {1}", videoPlayer.frame, videoPlayer.frameCount);
		Debug.LogFormat("time = {0} - clip.length = {1}", videoPlayer.time, videoPlayer.length);
		VideoFinished();
	}

	private void VideoFinished()
	{
		Time.timeScale = 1f;
		if (!GameManager.Instance.isIntro)
		{
			SoundManager.Instance.OnTempSoundsOn();
			GameManager.Instance.isIntro = true;
		}
		Object.Destroy(base.gameObject);
	}
}
