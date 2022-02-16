using System.Collections;
using UnityEngine;

public class TapJoyManager : MonoBehaviour
{
	public AndroidJavaClass tapjoyConnect;

	public AndroidJavaObject tapjoyConnectInstance;

	public AndroidJavaObject currentActivity;

	private static TapJoyManager instance;

	public bool hasRcvdRcvData;

	public bool hasRcvdSpendData;

	public static TapJoyManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		hasRcvdRcvData = false;
		hasRcvdSpendData = false;
		JavaVM.AttachCurrentThread();
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		tapjoyConnect = new AndroidJavaClass("com.tapjoy.TapjoyConnect");
		tapjoyConnect.CallStatic("requestTapjoyConnect", currentActivity, "4f98b683-0d7c-499d-8d71-68e5fd4857dc", "fCaVIwV4WkNKNjbEYtrp");
		tapjoyConnectInstance = tapjoyConnect.CallStatic<AndroidJavaObject>("getTapjoyConnectInstance", new object[0]);
		tapjoyConnectInstance.Call("initVideoAd");
		GetFeaturedApp();
	}

	private void Start()
	{
		if (PlayerPrefs.GetInt("isFirstTime") == 1)
		{
			Instance.AwardJoules(250);
			PlayerPrefs.SetInt("isFirstTime", 0);
		}
		StartCoroutine("UpdateTapPoints");
	}

	private IEnumerator UpdateTapPoints()
	{
		int offlineJoules = PlayerPrefs.GetInt("offlineJoules", 0);
		if (offlineJoules > 0)
		{
			tapjoyConnectInstance.Call("awardTapPoints", offlineJoules);
			StartCoroutine("awardReceive");
		}
		else if (offlineJoules < 0)
		{
			tapjoyConnectInstance.Call("spendTapPoints", -offlineJoules);
			StartCoroutine("spendReceive");
		}
		PlayerPrefs.SetInt("offlineJoules", 0);
		yield return new WaitForSeconds(10f);
		while (Application.internetReachability != 0)
		{
			GetTapPointsFromServer();
			yield return new WaitForSeconds(10f);
		}
		StartCoroutine("OfflineConnectionCheck");
	}

	private IEnumerator OfflineConnectionCheck()
	{
		while (Application.internetReachability == NetworkReachability.NotReachable)
		{
			yield return new WaitForSeconds(10f);
		}
		StartCoroutine("UpdateTapPoints");
	}

	public void GetTapPointsFromServer()
	{
		tapjoyConnectInstance.Call("getTapPoints");
		if (Application.internetReachability != 0 && GetTapPoints() != 0)
		{
			PlayerPrefs.SetInt("joules", GetTapPoints());
		}
	}

	public int GetTapPoints()
	{
		return tapjoyConnectInstance.Call<int>("getTapPointsTotal", new object[0]);
	}

	public void ShowOffers()
	{
		tapjoyConnectInstance.Call("showOffers");
	}

	public void AwardJoules(int num)
	{
		int @int = PlayerPrefs.GetInt("joules");
		PlayerPrefs.SetInt("joules", @int + num);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			PlayerPrefs.SetInt("offlineJoules", PlayerPrefs.GetInt("offlineJoules", 0) + num);
			return;
		}
		tapjoyConnectInstance.Call("awardTapPoints", num);
		StartCoroutine("awardReceive");
	}

	public void GetFeaturedApp()
	{
		tapjoyConnectInstance.Call("getFeaturedApp");
		StartCoroutine("receivedFeaturedApp");
	}

	private IEnumerator receivedFeaturedApp()
	{
		while (true)
		{
			if (tapjoyConnectInstance.Call<bool>("didReceiveFeaturedAppData", new object[0]))
			{
				tapjoyConnectInstance.Call("showFeaturedAppFullScreenAd");
				break;
			}
			if (tapjoyConnectInstance.Call<bool>("didReceiveFeaturedAppDataFail", new object[0]))
			{
				break;
			}
			yield return null;
		}
		yield return null;
	}

	public void SpendJoules(int num)
	{
		int @int = PlayerPrefs.GetInt("joules");
		PlayerPrefs.SetInt("joules", @int - num);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			PlayerPrefs.SetInt("offlineJoules", PlayerPrefs.GetInt("offlineJoules", 0) - num);
			return;
		}
		tapjoyConnectInstance.Call("spendTapPoints", num);
		StartCoroutine("spendReceive");
	}

	private IEnumerator awardReceive()
	{
		hasRcvdRcvData = false;
		while (!tapjoyConnectInstance.Call<bool>("didReceiveAwardTapPointsData", new object[0]))
		{
			yield return null;
		}
		hasRcvdRcvData = true;
	}

	private IEnumerator spendReceive()
	{
		hasRcvdSpendData = false;
		while (!tapjoyConnectInstance.Call<bool>("didReceiveSpendTapPointsData", new object[0]))
		{
			yield return null;
		}
		hasRcvdSpendData = true;
	}
}
