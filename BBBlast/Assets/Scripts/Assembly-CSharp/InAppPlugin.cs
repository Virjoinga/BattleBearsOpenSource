using UnityEngine;

public class InAppPlugin : MonoBehaviour
{
	private enum InAppPurchase
	{
		skyvu_01000_joules,
		skyvu_05000_joules,
		skyvu_15000_joules
	}

	private const int numInAppPurchases = 3;

	private AndroidJavaClass unityPlayerClass;

	private AndroidJavaObject unityActivity;

	private AndroidJavaObject inAppPlugin;

	public string publicLicenseKey;

	public string testingString = string.Empty;

	private string dID;

	private string welcome;

	private bool oldUser;

	private bool needToUpdate;

	private int numMonies;

	private int[] numJoules;

	private int num100Joules;

	private int numrupees5;

	private void OnGUI()
	{
	}

	private void Start()
	{
		PlayerPrefs.SetString("Testing", "Works");
		PlayerPrefs.Save();
		unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
		inAppPlugin = new AndroidJavaObject("com.skyvu.tools.inappplugin.InAppPlugin", unityActivity, publicLicenseKey);
		dID = GetDeviceID();
		numJoules = new int[3];
		numJoules[0] = inAppPlugin.Call<int>("GetNumItem", new object[1] { "skyvu_01000_joules" });
		numJoules[1] = inAppPlugin.Call<int>("GetNumItem", new object[1] { "skyvu_05000_joules" });
		numJoules[2] = inAppPlugin.Call<int>("GetNumItem", new object[1] { "skyvu_15000_joules" });
	}

	public void BuyJoules(string itemID)
	{
		inAppPlugin.Call("BuyItem", itemID);
	}

	private void addItem(string itemID, int num)
	{
		inAppPlugin.Call("SetItemQuantity", itemID, num);
	}

	private void useItem(string itemID, int num)
	{
		inAppPlugin.Call("SetItemQuantity", itemID, num);
	}

	public void PurchasedJoules(string incoming)
	{
		testingString = incoming;
		string s = incoming.Split('_')[1];
		int result;
		if (int.TryParse(s, out result))
		{
			TapJoyManager.Instance.AwardJoules(result);
		}
	}

	private string GetDeviceID()
	{
		return inAppPlugin.Call<string>("GetDeviceID", new object[0]);
	}

	private void OnDestroy()
	{
		inAppPlugin.Call("safeExit");
	}
}
