using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
	private Transform scroller;

	private int framesToCheck = 20;

	private float[] changes;

	private Rigidbody scrollRigidbody;

	public string creditsMusic;

	public float scrollSpeed;

	private void Awake()
	{
		changes = new float[framesToCheck];
		scroller = base.transform.Find("Scroll");
		scrollRigidbody = scroller.GetComponent<Rigidbody>();
		if (GameManager.Instance.useHighres)
		{
			SoundManager.Instance.playMusic(Resources.Load("Music/High/" + creditsMusic) as AudioClip, true);
		}
		else
		{
			SoundManager.Instance.playMusic(Resources.Load("Music/Low/" + creditsMusic) as AudioClip, true);
		}
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		switch (b.name)
		{
		case "Facebook_btn":
			b.enable();
			break;
		case "Twitter_btn":
			b.enable();
			break;
		case "SoundTrack_btn":
			b.enable();
			break;
		case "Tshirt_btn":
			b.enable();
			break;
		case "BBv1_btn":
			b.enable();
			break;
		case "mainMenu_btn":
			SoundManager.Instance.playMusic(GameManager.Instance.menuMusic, true);
			break;
		}
	}

	private void Update()
	{
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			changes[Time.frameCount % framesToCheck] = touch.deltaPosition.y;
			if (touch.phase == TouchPhase.Began)
			{
				for (int j = 0; j < framesToCheck; j++)
				{
					changes[j] = 0f;
				}
				scrollRigidbody.velocity = Vector3.zero;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				Vector3 localPosition = scroller.transform.localPosition;
				localPosition.z -= touch.deltaPosition.y;
				scroller.transform.localPosition = localPosition;
				scrollRigidbody.velocity = Vector3.zero;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				float num = 0f;
				for (int k = 0; k < framesToCheck; k++)
				{
					num += changes[k];
				}
				Vector3 velocity = scrollRigidbody.velocity;
				velocity.z = (0f - num) * 2.5f;
				scrollRigidbody.velocity = velocity;
			}
		}
		scrollRigidbody.velocity = Input.GetAxis("Mouse ScrollWheel") * Vector3.forward * scrollSpeed;
		Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
	}

	private void LateUpdate()
	{
		Vector3 localPosition = scroller.transform.localPosition;
		if (localPosition.z > 0f)
		{
			localPosition.z = 0f;
		}
		else if (localPosition.z < -1550f)
		{
			localPosition.z = -1550f;
		}
		scroller.transform.localPosition = localPosition;
	}
}
