using UnityEngine;

public class CallJavaCode : MonoBehaviour
{
	public AndroidJavaClass tapjoyConnect;

	public AndroidJavaObject tapjoyConnectInstance;

	public AndroidJavaObject currentActivity;

	private bool queryFeaturedApp;

	private bool queryDisplayAd;

	private bool queryTapPoints;

	private bool querySpendPoints;

	private bool queryAwardPoints;

	private string tapPointsLabel = string.Empty;

	private string showOffers = "show offers";

	private string featuredApp = "show featured app";

	private string displayAd = "display ad";

	private string getPoints = "get tap points";

	private string spendPoints = "spend points";

	private string awardPoints = "award points";

	private void Start()
	{
		JavaVM.AttachCurrentThread();
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.tapjoy.TapjoyLog");
		androidJavaClass.CallStatic("enableLogging", true);
		AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currentActivity = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
		tapjoyConnect = new AndroidJavaClass("com.tapjoy.TapjoyConnect");
		tapjoyConnect.CallStatic("requestTapjoyConnect", currentActivity, "bba49f11-b87f-4c0f-9632-21aa810dd6f1", "yiQIURFEeKm0zbOggubu");
		tapjoyConnectInstance = tapjoyConnect.CallStatic<AndroidJavaObject>("getTapjoyConnectInstance", new object[0]);
		tapjoyConnectInstance.Call("initVideoAd");
	}

	private void OnGUI()
	{
		int num = 25;
		int num2 = 60;
		int num3 = 25;
		GUI.Label(new Rect(15f, num, 200f, 20f), tapPointsLabel);
		num += num2 + num3;
		if (GUI.Button(new Rect(15f, num, 450f, num2), showOffers))
		{
			Debug.Log("showing offers...");
			tapjoyConnectInstance.Call("showOffers");
		}
		num += num2 + num3;
		if (GUI.Button(new Rect(15f, num, 450f, num2), featuredApp))
		{
			Debug.Log("showing offers...");
			tapjoyConnectInstance.Call("getFeaturedApp");
			queryFeaturedApp = true;
		}
		num += num2 + num3;
		if (GUI.Button(new Rect(15f, num, 450f, num2), getPoints))
		{
			tapjoyConnectInstance.Call("getTapPoints");
			queryTapPoints = true;
		}
		num += num2 + num3;
		if (GUI.Button(new Rect(15f, num, 450f, num2), spendPoints))
		{
			tapjoyConnectInstance.Call("spendTapPoints", 10);
			querySpendPoints = true;
		}
		num += num2 + num3;
		if (GUI.Button(new Rect(15f, num, 450f, num2), awardPoints))
		{
			tapjoyConnectInstance.Call("awardTapPoints", 10);
			queryAwardPoints = true;
		}
		num += num2 + num3;
		if (GUI.Button(new Rect(15f, num, 450f, num2), displayAd))
		{
			num += num2 + num3;
			tapjoyConnectInstance.Call("setBannerAdPosition", 0, num);
			tapjoyConnectInstance.Call("getDisplayAd");
			queryDisplayAd = true;
		}
		num += num2 + num3;
		if (queryFeaturedApp)
		{
			if (tapjoyConnectInstance.Call<bool>("didReceiveFeaturedAppData", new object[0]))
			{
				tapjoyConnectInstance.Call("showFeaturedAppFullScreenAd");
				queryFeaturedApp = false;
			}
			else if (tapjoyConnectInstance.Call<bool>("didReceiveFeaturedAppDataFail", new object[0]))
			{
				tapPointsLabel = "Get Featured App failed";
				queryFeaturedApp = false;
			}
		}
		if (queryDisplayAd)
		{
			if (tapjoyConnectInstance.Call<bool>("didReceiveDisplayAdData", new object[0]))
			{
				tapjoyConnectInstance.Call("showBannerAd");
				queryDisplayAd = false;
			}
			else if (tapjoyConnectInstance.Call<bool>("didReceiveDisplayAdDataFail", new object[0]))
			{
				tapPointsLabel = "Get Display Ad failed";
				queryDisplayAd = false;
			}
		}
		if (queryTapPoints)
		{
			if (tapjoyConnectInstance.Call<bool>("didReceiveGetTapPointsData", new object[0]))
			{
				tapPointsLabel = "Total TapPoints: " + tapjoyConnectInstance.Call<int>("getTapPointsTotal", new object[0]);
				queryTapPoints = false;
			}
			else if (tapjoyConnectInstance.Call<bool>("didReceiveGetTapPointsDataFail", new object[0]))
			{
				tapPointsLabel = "Get Tap Points failed";
				queryTapPoints = false;
			}
		}
		if (querySpendPoints)
		{
			if (tapjoyConnectInstance.Call<bool>("didReceiveSpendTapPointsData", new object[0]))
			{
				tapPointsLabel = "Total TapPoints: " + tapjoyConnectInstance.Call<int>("getTapPointsTotal", new object[0]);
				querySpendPoints = false;
			}
			else if (tapjoyConnectInstance.Call<bool>("didReceiveSpendTapPointsDataFail", new object[0]))
			{
				tapPointsLabel = "Spend Points failed";
				querySpendPoints = false;
			}
		}
		if (queryAwardPoints)
		{
			if (tapjoyConnectInstance.Call<bool>("didReceiveAwardTapPointsData", new object[0]))
			{
				tapPointsLabel = "Total TapPoints: " + tapjoyConnectInstance.Call<int>("getTapPointsTotal", new object[0]);
				queryAwardPoints = false;
			}
			else if (tapjoyConnectInstance.Call<bool>("didReceiveAwardTapPointsDataFail", new object[0]))
			{
				tapPointsLabel = "Award Points failed";
				queryAwardPoints = false;
			}
		}
	}
}
