using System;
using UnityEngine;

[Serializable]
public class TestFacebookJS : MonoBehaviour
{
	public Texture2D text2D;

	private FacebookPlugin facebookPlugin;

	public string FACEBOOK_ID;

	private bool isLoggedIn;

	private string palyerName;

	public TestFacebookJS()
	{
		FACEBOOK_ID = string.Empty;
		palyerName = "PlayerName";
	}

	public virtual void OnEnable()
	{
		FacebookEventManager.onLoginSuccess += onLoginSuccess;
		FacebookEventManager.onLogoutFinish += onLogoutFinish;
		FacebookEventManager.onGetPlayerNameComplete += onGetPlayerNameComplete;
	}

	public virtual void OnDisable()
	{
		FacebookEventManager.onLoginSuccess -= onLoginSuccess;
		FacebookEventManager.onLogoutFinish -= onLogoutFinish;
		FacebookEventManager.onGetPlayerNameComplete -= onGetPlayerNameComplete;
	}

	public virtual void onLoginSuccess()
	{
		isLoggedIn = true;
	}

	public virtual void onLogoutFinish()
	{
		isLoggedIn = false;
	}

	public virtual void Start()
	{
		facebookPlugin = new FacebookPlugin();
		facebookPlugin.Init(FACEBOOK_ID);
		isLoggedIn = facebookPlugin.isLoggedIn();
	}

	public virtual void OnGUI()
	{
		if (!(FACEBOOK_ID == null) && !(FACEBOOK_ID == string.Empty))
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
				facebookPlugin.postMessageOnWall("My Scores :" + UnityEngine.Random.Range(0, 10000), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 50f), "postMessage 2"))
			{
				facebookPlugin.postMessageOnWall("My Scores :" + UnityEngine.Random.Range(0, 10000), "http://www.midamobile.com/Mida_Mobile/Home_files/IMG_0109.jpg", "http://www.MidaMobile.com/", "Name link : Mida Mobile", "caption", "a Short description");
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 50f), "postMessage 3"))
			{
				facebookPlugin.postMessageOnWall("My Scores :" + UnityEngine.Random.Range(0, 10000), "http://www.midamobile.com/Mida_Mobile/Home_files/IMG_0109.jpg", string.Empty, string.Empty, string.Empty, string.Empty);
			}
			if (GUI.Button(new Rect(200f, 100f, 150f, 50f), "postMessageDialog 1"))
			{
				facebookPlugin.postMessageOnWallDialog("My Scores :" + UnityEngine.Random.Range(0, 10000), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
			}
			if (GUI.Button(new Rect(200f, 150f, 150f, 50f), "postMessageDialog 2"))
			{
				facebookPlugin.postMessageOnWallDialog("My Scores :" + UnityEngine.Random.Range(0, 10000), string.Empty, "http://www.MidaMobile.com/", "Name link : Mida Mobile", string.Empty, string.Empty);
			}
			if (GUI.Button(new Rect(200f, 200f, 150f, 50f), "postMessageDialog 3"))
			{
				facebookPlugin.postMessageOnWallDialog("My Scores :" + UnityEngine.Random.Range(0, 10000), "http://www.midamobile.com/Mida_Mobile/Home_files/IMG_0109.jpg", "http://www.MidaMobile.com/", "Name link : Mida Mobile", "caption", "a Short description");
			}
			if (GUI.Button(new Rect(200f, 300f, 150f, 50f), "post Image byte[]"))
			{
				byte[] picture = text2D.EncodeToPNG();
				facebookPlugin.postImageOnWall(picture, "caption");
			}
			if (GUI.Button(new Rect(0f, 300f, 150f, 50f), "post Image Path"))
			{
				facebookPlugin.postImageOnWallPath("/mnt/sdcard/image.jpg", "caption");
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

	public virtual void onGetPlayerNameComplete(string name)
	{
		palyerName = "PlayerName = " + name;
	}

	public virtual void Main()
	{
	}
}
