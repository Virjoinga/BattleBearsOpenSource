using UnityEngine;

public class WhiteOut : MonoBehaviour
{
	private EnemyController myController;

	private MeshEmitter myMeshEmitter;

	public GameObject legToEmit;

	public GameObject headToEmit;

	public GameObject armToEmit;

	public GameObject BOOM;

	private Mesh myMesh;

	private Vector2[] uv;

	private static Vector2[] whiteUVs = new Vector2[273]
	{
		new Vector2(0.814033f, 0.923859f),
		new Vector2(0.814033f, 0.9108204f),
		new Vector2(0.827398f, 0.9174687f),
		new Vector2(0.80249f, 0.9174687f),
		new Vector2(0.787019f, 0.9174687f),
		new Vector2(0.775877f, 0.9111093f),
		new Vector2(0.827398f, 0.9065609f),
		new Vector2(0.814033f, 0.9065609f),
		new Vector2(0.775877f, 0.9065609f),
		new Vector2(0.80249f, 0.9065609f),
		new Vector2(0.814033f, 0.8703276f),
		new Vector2(0.80249f, 0.8703276f),
		new Vector2(0.787019f, 0.9065609f),
		new Vector2(0.787019f, 0.8703276f),
		new Vector2(0.80249f, 0.833241f),
		new Vector2(0.787019f, 0.833241f),
		new Vector2(0.827398f, 0.8703276f),
		new Vector2(0.814033f, 0.833241f),
		new Vector2(0.775877f, 0.8703276f),
		new Vector2(0.775877f, 0.833241f),
		new Vector2(0.762191f, 0.833241f),
		new Vector2(0.814033f, 0.923859f),
		new Vector2(0.814033f, 0.9108204f),
		new Vector2(0.827398f, 0.9174687f),
		new Vector2(0.80249f, 0.9174687f),
		new Vector2(0.787019f, 0.9174687f),
		new Vector2(0.775877f, 0.9111093f),
		new Vector2(0.827398f, 0.9065609f),
		new Vector2(0.814033f, 0.9065609f),
		new Vector2(0.775877f, 0.9065609f),
		new Vector2(0.80249f, 0.9065609f),
		new Vector2(0.814033f, 0.8703276f),
		new Vector2(0.80249f, 0.8703276f),
		new Vector2(0.787019f, 0.9065609f),
		new Vector2(0.787019f, 0.8703276f),
		new Vector2(0.80249f, 0.833241f),
		new Vector2(0.827398f, 0.8703276f),
		new Vector2(0.814033f, 0.833241f),
		new Vector2(0.775877f, 0.8703276f),
		new Vector2(0.762191f, 0.833241f),
		new Vector2(0.787019f, 0.833241f),
		new Vector2(0.775877f, 0.833241f),
		new Vector2(0.756935f, 0.873167f),
		new Vector2(0.740053f, 0.882167f),
		new Vector2(0.740053f, 0.868371f),
		new Vector2(0.684923f, 0.868782f),
		new Vector2(0.70209f, 0.873167f),
		new Vector2(0.722817f, 0.873167f),
		new Vector2(0.70209f, 0.833241f),
		new Vector2(0.722817f, 0.833241f),
		new Vector2(0.684923f, 0.833241f),
		new Vector2(0.667911f, 0.833241f),
		new Vector2(0.740053f, 0.833241f),
		new Vector2(0.667911f, 0.8642731f),
		new Vector2(0.684923f, 0.8608649f),
		new Vector2(0.70209f, 0.8642731f),
		new Vector2(0.722817f, 0.8642731f),
		new Vector2(0.740053f, 0.8605455f),
		new Vector2(0.756935f, 0.873167f),
		new Vector2(0.740053f, 0.882167f),
		new Vector2(0.740053f, 0.868371f),
		new Vector2(0.684923f, 0.868782f),
		new Vector2(0.70209f, 0.873167f),
		new Vector2(0.722817f, 0.873167f),
		new Vector2(0.70209f, 0.8642731f),
		new Vector2(0.722817f, 0.8642731f),
		new Vector2(0.684923f, 0.8608649f),
		new Vector2(0.667911f, 0.8642731f),
		new Vector2(0.740053f, 0.8605455f),
		new Vector2(0.684923f, 0.833241f),
		new Vector2(0.667911f, 0.833241f),
		new Vector2(0.70209f, 0.833241f),
		new Vector2(0.722817f, 0.833241f),
		new Vector2(0.740053f, 0.833241f),
		new Vector2(0.761705f, 0.974285f),
		new Vector2(0.736527f, 0.962396f),
		new Vector2(0.764812f, 0.962164f),
		new Vector2(0.741499f, 0.974668f),
		new Vector2(0.694753f, 0.7916421f),
		new Vector2(0.680429f, 0.7372365f),
		new Vector2(0.692892f, 0.734621f),
		new Vector2(0.667911f, 0.7372365f),
		new Vector2(0.714069f, 0.8278071f),
		new Vector2(0.728843f, 0.987662f),
		new Vector2(0.94242f, 0.7916421f),
		new Vector2(0.94494f, 0.7341781f),
		new Vector2(0.680429f, 0.670522f),
		new Vector2(0.667911f, 0.670522f),
		new Vector2(0.695375f, 0.670522f),
		new Vector2(0.9387324f, 0.670522f),
		new Vector2(0.734366f, 0.959272f),
		new Vector2(0.691681f, 0.9357851f),
		new Vector2(0.819503f, 0.8278071f),
		new Vector2(0.890272f, 0.8178134f),
		new Vector2(0.74596f, 0.8182194f),
		new Vector2(0.855728f, 0.8128605f),
		new Vector2(0.781859f, 0.8130685f),
		new Vector2(0.717564f, 0.93818f),
		new Vector2(0.8453545f, 0.7044405f),
		new Vector2(0.7946045f, 0.7051705f),
		new Vector2(0.7399976f, 0.7086185f),
		new Vector2(0.7243724f, 0.798116f),
		new Vector2(0.9215705f, 0.7064435f),
		new Vector2(0.9017451f, 0.8008807f),
		new Vector2(0.7716783f, 0.798614f),
		new Vector2(0.8600416f, 0.798173f),
		new Vector2(0.8693829f, 0.7655901f),
		new Vector2(0.7718439f, 0.7662531f),
		new Vector2(0.7208874f, 0.7648087f),
		new Vector2(0.6938225f, 0.7631316f),
		new Vector2(0.680429f, 0.7917851f),
		new Vector2(0.667911f, 0.7917851f),
		new Vector2(0.94368f, 0.7629101f),
		new Vector2(0.9167128f, 0.7658984f),
		new Vector2(0.699654f, 0.918057f),
		new Vector2(0.686235f, 0.928898f),
		new Vector2(0.688917f, 0.918057f),
		new Vector2(0.702864f, 0.928898f),
		new Vector2(0.716879f, 0.914772f),
		new Vector2(0.70401f, 0.907462f),
		new Vector2(0.722627f, 0.887642f),
		new Vector2(0.755804f, 0.901626f),
		new Vector2(0.753498f, 0.887642f),
		new Vector2(0.684901f, 0.9080631f),
		new Vector2(0.706622f, 0.897292f),
		new Vector2(0.682353f, 0.897292f),
		new Vector2(0.699654f, 0.918057f),
		new Vector2(0.686235f, 0.928898f),
		new Vector2(0.688917f, 0.918057f),
		new Vector2(0.702864f, 0.928898f),
		new Vector2(0.716879f, 0.914772f),
		new Vector2(0.70401f, 0.907462f),
		new Vector2(0.722627f, 0.887642f),
		new Vector2(0.755804f, 0.901626f),
		new Vector2(0.753498f, 0.887642f),
		new Vector2(0.684901f, 0.9080631f),
		new Vector2(0.706622f, 0.897292f),
		new Vector2(0.682353f, 0.897292f),
		new Vector2(0.900616f, 0.9516921f),
		new Vector2(0.877233f, 0.8575295f),
		new Vector2(0.900616f, 0.8575295f),
		new Vector2(0.877233f, 0.9516921f),
		new Vector2(0.974064f, 0.8575295f),
		new Vector2(0.9542412f, 0.9516921f),
		new Vector2(0.9542412f, 0.8575295f),
		new Vector2(0.974064f, 0.9516921f),
		new Vector2(0.925621f, 0.9516921f),
		new Vector2(0.925621f, 0.8575295f),
		new Vector2(0.854925f, 0.9516921f),
		new Vector2(0.854925f, 0.8575295f),
		new Vector2(0.877233f, 0.994433f),
		new Vector2(0.900616f, 0.994433f),
		new Vector2(0.900616f, 0.8344929f),
		new Vector2(0.877233f, 0.8344929f),
		new Vector2(0.974064f, 0.994433f),
		new Vector2(0.9542412f, 0.994433f),
		new Vector2(0.9542412f, 0.8344929f),
		new Vector2(0.974064f, 0.8344929f),
		new Vector2(0.996399f, 0.9516921f),
		new Vector2(0.996399f, 0.994433f),
		new Vector2(0.996399f, 0.8344929f),
		new Vector2(0.996399f, 0.8575295f),
		new Vector2(0.925621f, 0.994433f),
		new Vector2(0.925621f, 0.8344929f),
		new Vector2(0.780229f, 0.938238f),
		new Vector2(0.751847f, 0.914772f),
		new Vector2(0.925621f, 0.9099194f),
		new Vector2(0.900616f, 0.9099194f),
		new Vector2(0.877233f, 0.9099194f),
		new Vector2(0.974064f, 0.9099194f),
		new Vector2(0.9542412f, 0.9099194f),
		new Vector2(0.794706f, 0.926949f),
		new Vector2(0.775877f, 0.923859f),
		new Vector2(0.762191f, 0.9174687f),
		new Vector2(0.762191f, 0.9065609f),
		new Vector2(0.762191f, 0.8703276f),
		new Vector2(0.827398f, 0.833241f),
		new Vector2(0.794706f, 0.926949f),
		new Vector2(0.775877f, 0.923859f),
		new Vector2(0.762191f, 0.9174687f),
		new Vector2(0.762191f, 0.9065609f),
		new Vector2(0.762191f, 0.8703276f),
		new Vector2(0.827398f, 0.833241f),
		new Vector2(0.667911f, 0.873167f),
		new Vector2(0.684923f, 0.882637f),
		new Vector2(0.712717f, 0.8911101f),
		new Vector2(0.756935f, 0.8642731f),
		new Vector2(0.756935f, 0.833241f),
		new Vector2(0.684923f, 0.882637f),
		new Vector2(0.667911f, 0.873167f),
		new Vector2(0.712717f, 0.8911101f),
		new Vector2(0.756935f, 0.8642731f),
		new Vector2(0.756935f, 0.833241f),
		new Vector2(0.680429f, 0.8278071f),
		new Vector2(0.774668f, 0.987358f),
		new Vector2(0.9226069f, 0.8278071f),
		new Vector2(0.958622f, 0.8278071f),
		new Vector2(0.958622f, 0.792313f),
		new Vector2(0.958622f, 0.756819f),
		new Vector2(0.958622f, 0.670522f),
		new Vector2(0.700563f, 0.997492f),
		new Vector2(0.726293f, 0.986191f),
		new Vector2(0.672036f, 0.957998f),
		new Vector2(0.676851f, 0.984542f),
		new Vector2(0.752099f, 0.997957f),
		new Vector2(0.7946179f, 0.670522f),
		new Vector2(0.739353f, 0.670522f),
		new Vector2(0.8512936f, 0.670522f),
		new Vector2(0.667911f, 0.8278071f),
		new Vector2(0.72032f, 0.901626f),
		new Vector2(0.672525f, 0.914772f),
		new Vector2(0.713996f, 0.897292f),
		new Vector2(0.675407f, 0.897292f),
		new Vector2(0.731289f, 0.912926f),
		new Vector2(0.744592f, 0.912926f),
		new Vector2(0.72032f, 0.901626f),
		new Vector2(0.672525f, 0.914772f),
		new Vector2(0.713996f, 0.897292f),
		new Vector2(0.675407f, 0.897292f),
		new Vector2(0.731289f, 0.912926f),
		new Vector2(0.744592f, 0.912926f),
		new Vector2(0.828197f, 0.951071f),
		new Vector2(0.828216f, 0.978037f),
		new Vector2(0.805141f, 0.994078f),
		new Vector2(0.777969f, 0.986905f),
		new Vector2(0.809432f, 0.933084f),
		new Vector2(0.767474f, 0.962009f),
		new Vector2(0.72921f, 0.943004f),
		new Vector2(0.741253f, 0.958799f),
		new Vector2(0.761114f, 0.958571f),
		new Vector2(0.773591f, 0.942899f),
		new Vector2(0.733859f, 0.923503f),
		new Vector2(0.769436f, 0.923354f),
		new Vector2(0.854925f, 0.994433f),
		new Vector2(0.854925f, 0.8344929f),
		new Vector2(0.832086f, 0.8575295f),
		new Vector2(0.832086f, 0.8344929f),
		new Vector2(0.832086f, 0.9516921f),
		new Vector2(0.832086f, 0.994433f),
		new Vector2(0.80249f, 0.9174687f),
		new Vector2(0.787019f, 0.9174687f),
		new Vector2(0.787019f, 0.9174687f),
		new Vector2(0.80249f, 0.9174687f),
		new Vector2(0.80249f, 0.9174687f),
		new Vector2(0.80249f, 0.9174687f),
		new Vector2(0.787019f, 0.9174687f),
		new Vector2(0.787019f, 0.9174687f),
		new Vector2(0.80249f, 0.9174687f),
		new Vector2(0.80249f, 0.9174687f),
		new Vector2(0.70209f, 0.873167f),
		new Vector2(0.722817f, 0.873167f),
		new Vector2(0.70209f, 0.873167f),
		new Vector2(0.722817f, 0.873167f),
		new Vector2(0.70209f, 0.873167f),
		new Vector2(0.722817f, 0.873167f),
		new Vector2(0.70209f, 0.873167f),
		new Vector2(0.722817f, 0.873167f),
		new Vector2(0.9226069f, 0.8278071f),
		new Vector2(0.714069f, 0.8278071f),
		new Vector2(0.714069f, 0.8278071f),
		new Vector2(0.9226069f, 0.8278071f),
		new Vector2(0.684901f, 0.9080631f),
		new Vector2(0.70401f, 0.907462f),
		new Vector2(0.706622f, 0.897292f),
		new Vector2(0.684901f, 0.9080631f),
		new Vector2(0.684901f, 0.9080631f),
		new Vector2(0.682353f, 0.897292f),
		new Vector2(0.684901f, 0.9080631f),
		new Vector2(0.70401f, 0.907462f),
		new Vector2(0.706622f, 0.897292f),
		new Vector2(0.684901f, 0.9080631f),
		new Vector2(0.684901f, 0.9080631f),
		new Vector2(0.682353f, 0.897292f)
	};

	public void TurnWhite()
	{
		myMesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		myController = GetComponentInChildren<EnemyController>();
		if ((bool)myController)
		{
			myController.myColor = Enemy.WHI;
			myController.BOOM = BOOM;
		}
		else
		{
			GetComponentInChildren<PurpleHugBehavior>().myColor = Enemy.WHI;
			GetComponentInChildren<PurpleHugBehavior>().BOOM = BOOM;
		}
		myMesh.uv = whiteUVs;
		myMeshEmitter = GetComponentInChildren<MeshEmitter>();
		if ((bool)myMeshEmitter)
		{
			myMeshEmitter.armToEmit = armToEmit;
			myMeshEmitter.legToEmit = legToEmit;
			myMeshEmitter.headToEmit = headToEmit;
		}
	}
}