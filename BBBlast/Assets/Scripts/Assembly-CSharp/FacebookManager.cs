using UnityEngine;

public class FacebookManager : MonoBehaviour
{
	private Texture2D text2D;

	private FacebookPlugin facebookPlugin;

	public string FACEBOOK_ID = string.Empty;

	private bool isLoggedIn;

	private string palyerName = "PlayerName";

	private static FacebookManager instance;

	public static FacebookManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		facebookPlugin = new FacebookPlugin();
		facebookPlugin.Init(FACEBOOK_ID);
	}

	public void PostScoreOnWall(string score, string urlToPic, string linkToPage, string nameLink, string caption, string description)
	{
		if (isLoggedIn)
		{
			facebookPlugin.postMessageOnWall(score, urlToPic, linkToPage, nameLink, caption, description);
			return;
		}
		facebookPlugin.login();
		isLoggedIn = facebookPlugin.isLoggedIn();
	}

	public void PostScoreOnWall(string score)
	{
		PostScoreOnWall(score, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
	}

	public void PostScoreOnWall(string score, string urlToPic)
	{
		PostScoreOnWall(score, urlToPic, string.Empty, string.Empty, string.Empty, string.Empty);
	}

	public void PostScoreOnWall(string score, string urlToPic, string linkToPage)
	{
		PostScoreOnWall(score, urlToPic, linkToPage, string.Empty, string.Empty, string.Empty);
	}

	public void PostScoreOnWall(string score, string urlToPic, string linkToPage, string nameLink)
	{
		PostScoreOnWall(score, urlToPic, linkToPage, nameLink, string.Empty, string.Empty);
	}

	public void PostScoreOnWall(string score, string urlToPic, string linkToPage, string nameLink, string caption)
	{
		PostScoreOnWall(score, urlToPic, linkToPage, nameLink, caption, string.Empty);
	}

	public void PostImage(string path)
	{
		if (!isLoggedIn)
		{
			facebookPlugin.login();
			isLoggedIn = facebookPlugin.isLoggedIn();
		}
		if (isLoggedIn)
		{
			facebookPlugin.postImageOnWallPath(path.ToString(), "Bet you can't beat my high score in Battle Bears BLAST!");
		}
	}

	public void PostImage(Texture2D imgTex2D)
	{
		if (isLoggedIn)
		{
			byte[] picture = imgTex2D.EncodeToPNG();
			facebookPlugin.postImageOnWall(picture, "Bet you can't beat my high score in Battle Bears BLAST!");
		}
		else
		{
			facebookPlugin.login();
			isLoggedIn = facebookPlugin.isLoggedIn();
		}
	}

	private void onGetPlayerNameComplete(string name)
	{
		palyerName = "PlayerName = " + name;
	}
}
