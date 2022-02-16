using UnityEngine;

public class TestFacebookCS : MonoBehaviour
{
	public Texture2D text2D;

	private FacebookPlugin facebookPlugin;

	public string FACEBOOK_ID = string.Empty;

	private bool isLoggedIn;

	private string palyerName = "PlayerName";

	private void OnEnable()
	{
		FacebookEventManager.onLoginSuccess += onLoginSuccess;
		FacebookEventManager.onLogoutFinish += onLogoutFinish;
		FacebookEventManager.onGetPlayerNameComplete += onGetPlayerNameComplete;
	}

	private void OnDisable()
	{
		FacebookEventManager.onLoginSuccess -= onLoginSuccess;
		FacebookEventManager.onLogoutFinish -= onLogoutFinish;
		FacebookEventManager.onGetPlayerNameComplete -= onGetPlayerNameComplete;
	}

	private void onLoginSuccess()
	{
		isLoggedIn = true;
	}

	private void onLogoutFinish()
	{
		isLoggedIn = false;
	}

	private void Awake()
	{
	}

	private void Start()
	{
		facebookPlugin = new FacebookPlugin();
		facebookPlugin.Init(FACEBOOK_ID);
		isLoggedIn = facebookPlugin.isLoggedIn();
	}

	private void OnGUI()
	{
		if (FACEBOOK_ID != null && !(FACEBOOK_ID == string.Empty))
		{
			if (GUI.Button(new Rect(0f, 0f, 200f, 50f), "Login"))
			{
				facebookPlugin.login();
			}
			if (!isLoggedIn && GUI.Button(new Rect(225f, 0f, 200f, 50f), "isLoggedIn"))
			{
				isLoggedIn = facebookPlugin.isLoggedIn();
				Debug.Log("Result ..." + facebookPlugin.isLoggedIn());
			}
			if (isLoggedIn && GUI.Button(new Rect(225f, 0f, 200f, 50f), "Logout"))
			{
				facebookPlugin.logout();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 50f), "postMessage 1"))
			{
				facebookPlugin.postMessageOnWall("My Scores :" + Random.Range(0, 10000), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 50f), "postMessage 2"))
			{
				facebookPlugin.postMessageOnWall("Bet you can't beat my score of " + Random.Range(0, 10000) + " in Battle Bears BLAST!", "http://www.battlebears.com/storage/facebook/BB_BLAST_icon.jpg", "https://market.android.com/details?id=com.skyvu.blast", "Get Battle Bears BLAST on the Android Market", "for Android", "★★★★★ \"BLAST is a should buy for only $1. It's the perfect mobile arcade shooter, and truly redefines the genre.\" -AppleNApps.com ★★★★★ \"Battle Bears BLAST is a must play mobile game.\" -AppBattleground.com");
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 50f), "postMessage 3"))
			{
				facebookPlugin.postMessageOnWall("My Scores :" + Random.Range(0, 10000), "http://www.midamobile.com/Mida_Mobile/Home_files/IMG_0109.jpg", string.Empty, string.Empty, string.Empty, string.Empty);
			}
			if (GUI.Button(new Rect(200f, 100f, 150f, 50f), "postMessageDialog 1"))
			{
				facebookPlugin.postMessageOnWallDialog("My Scores :" + Random.Range(0, 10000), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
			}
			if (GUI.Button(new Rect(200f, 150f, 150f, 50f), "postMessageDialog 2"))
			{
				facebookPlugin.postMessageOnWallDialog("My Scores :" + Random.Range(0, 10000), string.Empty, "http://www.MidaMobile.com/", "Name link : Mida Mobile", string.Empty, string.Empty);
			}
			if (GUI.Button(new Rect(200f, 200f, 150f, 50f), "postMessageDialog 3"))
			{
				facebookPlugin.postMessageOnWallDialog("My Scores :" + Random.Range(0, 10000), "http://www.midamobile.com/Mida_Mobile/Home_files/IMG_0109.jpg", "http://www.MidaMobile.com/", "Name link : Mida Mobile", "caption", "a Short description");
			}
			if (GUI.Button(new Rect(200f, 300f, 150f, 50f), "post Image byte[]"))
			{
				byte[] picture = text2D.EncodeToPNG();
				facebookPlugin.postImageOnWall(picture, "caption");
			}
			if (GUI.Button(new Rect(0f, 300f, 150f, 50f), "post Image Path"))
			{
				facebookPlugin.postImageOnWallPath("/mnt/sdcard/Android/data/com.skyvu.blast/files/screentshot1.jpg", "caption");
			}
			if (GUI.Button(new Rect(0f, 350f, 150f, 50f), "get Friend"))
			{
				facebookPlugin.getFriend();
			}
			if (GUI.Button(new Rect(200f, 350f, 150f, 50f), "get Player Name"))
			{
				facebookPlugin.getPlayerNameId();
			}
			GUI.Label(new Rect(350f, 375f, 150f, 50f), palyerName);
		}
	}

	private void onGetPlayerNameComplete(string name)
	{
		palyerName = "PlayerName = " + name;
	}
}
