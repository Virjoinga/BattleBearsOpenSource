using System;
using System.Collections;
using AstarClasses;
using AstarMath;
using UnityEngine;

[AddComponentMenu("Pathfinding/A* Pathfinding")]
public class AstarPath : MonoBehaviour
{
	public class Path
	{
		public float pathStartTime;

		private Node start;

		public Node end;

		private BinaryHeap open;

		private Node current;

		public bool foundEnd;

		private float maxFrameTime = 0.002f;

		private float maxAngle = 20f;

		private float angleCost = 2f;

		private bool stepByStep = true;

		public Node[] path;

		public bool error;

		private float t;

		private int frames = 1;

		private int closedNodes;

		public Path(Vector3 newstart, Vector3 newend, float NmaxAngle, float NangleCost, bool NstepByStep)
		{
			float num = (pathStartTime = Time.realtimeSinceStartup);
			maxFrameTime = active.maxFrameTime;
			maxAngle = NmaxAngle / 90f;
			angleCost = NangleCost;
			stepByStep = NstepByStep;
			Int3 startPos = ToLocal(newstart);
			Int3 endPos = ToLocal(newend);
			PostNew(startPos, endPos);
		}

		public Path(Vector3 newstart, Vector3 newend, float NmaxAngle, float NangleCost, bool NstepByStep, int grid)
		{
			float num = (pathStartTime = Time.realtimeSinceStartup);
			maxFrameTime = active.maxFrameTime;
			maxAngle = NmaxAngle / 90f;
			angleCost = NangleCost;
			stepByStep = NstepByStep;
			Int3 startPos = ToLocal(newstart, grid);
			Int3 endPos = ToLocal(newend, grid);
			t += Time.realtimeSinceStartup - num;
			PostNew(startPos, endPos);
		}

		public void PostNew(Int3 startPos, Int3 endPos)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (startPos == new Int3(-1, -1, -1))
			{
				Debug.Log("Start is not inside any grids");
				error = true;
				return;
			}
			if (endPos == new Int3(-1, -1, -1))
			{
				Debug.Log("Target is not inside any grids");
				error = true;
				return;
			}
			start = GetNode(startPos);
			end = GetNode(endPos);
			if (!start.walkable)
			{
				if (start.neighbours.Length == 0)
				{
					Debug.Log("Starting from non walkable node");
					error = true;
					return;
				}
				start = start.neighbours[0];
				Debug.Log("Start point is not walkable, setting a node close to start as start");
			}
			current = start;
			if (!end.walkable)
			{
				if (end.neighbours.Length == 0)
				{
					error = true;
					return;
				}
				end = end.neighbours[0];
			}
			if (end.area != start.area)
			{
				Debug.Log("We can't walk from start to end, differend areas");
				error = true;
			}
			else
			{
				t += Time.realtimeSinceStartup - realtimeSinceStartup;
			}
		}

		public void Init()
		{
			open = active.binaryHeap;
			open.numberOfItems = 1;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			start.script = this;
			start.parent = null;
			if (active.testStraightLine && CheckLine(start, end, maxAngle))
			{
				foundEnd = true;
				path = new Node[2] { start, end };
			}
			else
			{
				t += Time.realtimeSinceStartup - realtimeSinceStartup;
			}
		}

		public void Calc()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			start.script = this;
			start.parent = null;
			while (!foundEnd && !error)
			{
				if (current == end)
				{
					foundEnd = true;
					break;
				}
				if (current.neighbours.Length == 0)
				{
					error = true;
					Debug.Log("no neihgbours!");
					return;
				}
				for (int i = 0; i < current.neighbours.Length; i++)
				{
					if (current.neighbours[i] != start)
					{
						Open(current.neighbours[i], i, current.angles[current.neighboursKeys[i]]);
					}
				}
				if (open.numberOfItems <= 0)
				{
					Debug.Log("No open points, whole area searched");
					error = true;
					return;
				}
				current = open.Remove();
			}
			if (error)
			{
				return;
			}
			if (end == start)
			{
				path = new Node[1] { start };
				t += Time.realtimeSinceStartup - realtimeSinceStartup;
				return;
			}
			if (active.simplify == Simplify.Simple)
			{
				Node parent = end;
				int num = 0;
				int invParentDirection = parent.invParentDirection;
				ArrayList arrayList = new ArrayList();
				arrayList.Add(parent);
				while (parent.parent != null)
				{
					if (parent.parent.invParentDirection != invParentDirection)
					{
						arrayList.Add(parent.parent);
						invParentDirection = parent.parent.invParentDirection;
					}
					parent = parent.parent;
					num++;
					if (parent == start)
					{
						break;
					}
					if (num > 300)
					{
						Debug.Log("Preventing possible infinity loop");
						break;
					}
				}
				arrayList.Reverse();
				path = arrayList.ToArray(typeof(Node)) as Node[];
				t += Time.realtimeSinceStartup - realtimeSinceStartup;
				return;
			}
			if (active.simplify == Simplify.Full)
			{
				Node parent2 = end;
				ArrayList arrayList2 = new ArrayList();
				arrayList2.Add(parent2);
				int num2 = 0;
				while (parent2.parent != null)
				{
					arrayList2.Add(parent2.parent);
					parent2 = parent2.parent;
					num2++;
					if (parent2 == start)
					{
						break;
					}
				}
				for (int j = 2; j < arrayList2.Count && j < arrayList2.Count; j++)
				{
					if (CheckLine((Node)arrayList2[j], (Node)arrayList2[j - 2], maxAngle))
					{
						arrayList2.RemoveAt(j - 1);
						j--;
					}
				}
				arrayList2.Reverse();
				path = arrayList2.ToArray(typeof(Node)) as Node[];
				t += Time.realtimeSinceStartup - realtimeSinceStartup;
				return;
			}
			Node parent3 = end;
			ArrayList arrayList3 = new ArrayList();
			arrayList3.Add(parent3);
			int num3 = 0;
			while (parent3.parent != null)
			{
				arrayList3.Add(parent3.parent);
				parent3 = parent3.parent;
				num3++;
				if (parent3 == start)
				{
					break;
				}
			}
			arrayList3.Reverse();
			path = arrayList3.ToArray(typeof(Node)) as Node[];
			t += Time.realtimeSinceStartup - realtimeSinceStartup;
		}

		public void Open(Node node, int i, float angle)
		{
			if (node.script != this)
			{
				if (!(angle >= maxAngle))
				{
					node.script = this;
					node.invParentDirection = current.neighboursKeys[i];
					node.parent = current;
					node.basicCost = ((current.costs == null || costs.Length == 0) ? costs[node.invParentDirection] : current.costs[node.invParentDirection]);
					node.extraCost = Mathf.RoundToInt((float)node.basicCost * angle * angleCost);
					node.UpdateH();
					node.UpdateG();
					open.Add(node);
				}
				return;
			}
			int num = ((current.costs == null || current.costs.Length == 0) ? costs[current.neighboursKeys[i]] : current.costs[current.neighboursKeys[i]]);
			int num2 = Mathf.RoundToInt((float)node.basicCost * angle * angleCost);
			if (current.g + num + num2 + node.penalty < node.g)
			{
				node.basicCost = num;
				node.extraCost = num2;
				node.invParentDirection = current.neighboursKeys[i];
				node.parent = current;
				node.UpdateAllG();
				open.Add(node);
			}
			else
			{
				if (node.g + num + num2 + current.penalty >= current.g)
				{
					return;
				}
				bool flag = false;
				for (int j = 0; j < node.neighbours.Length; j++)
				{
					if (node.neighbours[j] == current)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					current.parent = node;
					current.basicCost = num;
					current.extraCost = num2;
					current.invParentDirection = 7 - current.neighboursKeys[i];
					node.UpdateAllG();
					open.Add(current);
				}
			}
		}
	}

	public class BinaryHeap
	{
		private Node[] binaryHeap;

		public int numberOfItems;

		public BinaryHeap(int numberOfElements)
		{
			binaryHeap = new Node[numberOfElements];
			numberOfItems = 1;
		}

		public void Add(Node node)
		{
			if (numberOfItems == binaryHeap.Length)
			{
				numberOfItems--;
			}
			binaryHeap[numberOfItems] = node;
			int num = numberOfItems;
			while (num != 1)
			{
				int num2 = num / 2;
				if (binaryHeap[num].f > binaryHeap[num2].f)
				{
					break;
				}
				Node node2 = binaryHeap[num2];
				binaryHeap[num2] = binaryHeap[num];
				binaryHeap[num] = node2;
				num = num2;
			}
			numberOfItems++;
		}

		public Node Remove()
		{
			numberOfItems--;
			Node result = binaryHeap[1];
			binaryHeap[1] = binaryHeap[numberOfItems];
			int num = 1;
			int num2 = 1;
			do
			{
				num2 = num;
				if (2 * num2 + 1 <= numberOfItems)
				{
					if (binaryHeap[num2].f >= binaryHeap[2 * num2].f)
					{
						num = 2 * num2;
					}
					if (binaryHeap[num].f >= binaryHeap[2 * num2 + 1].f)
					{
						num = 2 * num2 + 1;
					}
				}
				else if (2 * num2 <= numberOfItems && binaryHeap[num2].f >= binaryHeap[2 * num2].f)
				{
					num = 2 * num2;
				}
				if (num2 != num)
				{
					Node node = binaryHeap[num2];
					binaryHeap[num2] = binaryHeap[num];
					binaryHeap[num] = node;
				}
			}
			while (num2 != num);
			return result;
		}
	}

	public static int totalNodeAmount = 0;

	public Node[][,] staticNodes;

	public AstarClasses.Grid[] grids = new AstarClasses.Grid[1]
	{
		new AstarClasses.Grid(20f)
	};

	public AstarClasses.Grid textureGrid = new AstarClasses.Grid(20f);

	public AstarClasses.Grid meshGrid = new AstarClasses.Grid(20f);

	public NodeLink[] links = new NodeLink[0];

	public GridGenerator gridGenerator;

	public BinaryHeap binaryHeap;

	public int levelCost = 40;

	public bool calculateOnStartup = true;

	public static bool activePath = false;

	public static Path prevPath = null;

	[NonSerialized]
	public static Path[] cache;

	public bool cachePaths;

	public int cacheLimit = 3;

	public float cacheTimeLimit = 2f;

	public bool showGrid;

	public bool showGridBounds = true;

	public bool showLinks = true;

	public static AstarPath active;

	public static int[] costs = new int[9] { 14, 10, 14, 10, 10, 14, 10, 14, 20 };

	[NonSerialized]
	public int area = -1;

	public Color[] areaColors;

	public float maxFrameTime = 0.01f;

	public int maxPathsPerFrame = 3;

	[NonSerialized]
	public int pathsThisFrame = 3;

	[NonSerialized]
	public int lastPathFrame = -9999;

	public static ArrayList pathQueue = null;

	public Simplify simplify;

	public float staticMaxAngle = 45f;

	public bool useNormal = true;

	public float heapSize = 1f;

	public DebugMode debugMode;

	public float debugModeRoof = 300f;

	public bool showParent;

	public bool showUnwalkable = true;

	public bool onlyShowLastPath;

	[NonSerialized]
	public Path lastPath;

	[NonSerialized]
	public bool anyPhysicsChanged;

	[NonSerialized]
	public int physicsChangedGrid = -1;

	public Height heightMode;

	public LayerMask groundLayer;

	public bool dontCutCorners;

	public bool testStraightLine;

	public float lineAccuracy = 0.5f;

	public Formula formula;

	public IsNeighbour isNeighbours;

	public float heightRaycast = 100f;

	public bool useWorldPositions;

	public float boundsMargin;

	public Texture2D navTex;

	public float threshold = 0.1f;

	public int penaltyMultiplier = 20;

	public string boundsTag = "";

	public float neighbourDistanceLimit = float.PositiveInfinity;

	public float boundMargin = 1f;

	public LayerMask boundsRayHitMask = 1;

	public float yLimit = 10f;

	public Vector3 navmeshRotation = Vector3.zero;

	public Mesh navmesh;

	public Matrix rotationMatrix = new Matrix();

	public MeshNodePosition meshNodePosition;

	public Transform listRootNode;

	private void OnDisable()
	{
		StopAllCoroutines();
		if (pathQueue != null)
		{
			pathQueue.Clear();
		}
		pathQueue = null;
	}

	public void OnDrawGizmos()
	{
		for (int i = 0; i < grids.Length; i++)
		{
			AstarClasses.Grid grid = grids[i];
			if (showGridBounds)
			{
				float num = grid.globalWidth * grid.nodeSize;
				float num2 = grid.globalDepth * grid.nodeSize;
				Gizmos.color = Color.white;
				Gizmos.DrawLine(new Vector3(grid.realOffset.x, grid.realOffset.y, grid.realOffset.z), new Vector3(grid.realOffset.x + num, grid.realOffset.y, grid.realOffset.z));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x, grid.realOffset.y, grid.realOffset.z), new Vector3(grid.realOffset.x, grid.realOffset.y, grid.realOffset.z + num2));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x + num, grid.realOffset.y, grid.realOffset.z), new Vector3(grid.realOffset.x + num, grid.realOffset.y, grid.realOffset.z + num2));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x, grid.realOffset.y, grid.realOffset.z + num2), new Vector3(grid.realOffset.x + num, grid.realOffset.y, grid.realOffset.z + num2));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x, grid.realOffset.y + grid.globalHeight, grid.realOffset.z), new Vector3(grid.realOffset.x + num, grid.realOffset.y + grid.globalHeight, grid.realOffset.z));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x, grid.realOffset.y + grid.globalHeight, grid.realOffset.z), new Vector3(grid.realOffset.x, grid.realOffset.y + grid.globalHeight, grid.realOffset.z + num2));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x + num, grid.realOffset.y + grid.globalHeight, grid.realOffset.z), new Vector3(grid.realOffset.x + num, grid.realOffset.y + grid.globalHeight, grid.realOffset.z + num2));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x, grid.realOffset.y + grid.globalHeight, grid.realOffset.z + num2), new Vector3(grid.realOffset.x + num, grid.realOffset.y + grid.globalHeight, grid.realOffset.z + num2));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x, grid.realOffset.y, grid.realOffset.z), new Vector3(grid.realOffset.x, grid.realOffset.y + grid.globalHeight, grid.realOffset.z));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x + num, grid.realOffset.y, grid.realOffset.z), new Vector3(grid.realOffset.x + num, grid.realOffset.y + grid.globalHeight, grid.realOffset.z));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x, grid.realOffset.y, grid.realOffset.z + num2), new Vector3(grid.realOffset.x, grid.realOffset.y + grid.globalHeight, grid.realOffset.z + num2));
				Gizmos.DrawLine(new Vector3(grid.realOffset.x + num, grid.realOffset.y, grid.realOffset.z + num2), new Vector3(grid.realOffset.x + num, grid.realOffset.y + grid.globalHeight, grid.realOffset.z + num2));
			}
			if (staticNodes == null || !showGrid || !grid.debug)
			{
				continue;
			}
			Node[,] array = staticNodes[i];
			foreach (Node node in array)
			{
				if ((!showUnwalkable && !node.walkable) || (onlyShowLastPath && node.script != lastPath))
				{
					continue;
				}
				switch (debugMode)
				{
				case DebugMode.Areas:
				{
					Color color2 = Color.white;
					color2 = areaColors[node.area];
					color2.a = 0.5f;
					Gizmos.color = color2;
					break;
				}
				case DebugMode.Angles:
				{
					float num3 = 0f;
					for (int l = 0; l < node.angles.Length; l++)
					{
						num3 = ((node.angles[l] > num3) ? node.angles[l] : num3);
					}
					Color color2 = (Gizmos.color = Color.Lerp(Color.green, Color.red, num3));
					break;
				}
				case DebugMode.H:
				{
					Color color2 = (Gizmos.color = Color.Lerp(Color.green, Color.red, (float)node.h / debugModeRoof));
					break;
				}
				case DebugMode.G:
				{
					Color color2 = (Gizmos.color = Color.Lerp(Color.green, Color.red, (float)node.g / debugModeRoof));
					break;
				}
				case DebugMode.F:
				{
					Color color2 = (Gizmos.color = Color.Lerp(Color.green, Color.red, (float)node.f / debugModeRoof));
					break;
				}
				}
				if (!node.walkable)
				{
					Color color2 = Color.red;
					color2.a = 0.5f;
					Gizmos.color = color2;
					Gizmos.DrawCube(node.vectorPos, Vector3.one * grid.nodeSize * 0.3f);
				}
				else if (node.parent != null && showParent)
				{
					Gizmos.DrawLine(node.vectorPos, node.parent.vectorPos);
				}
				else
				{
					for (int m = 0; m < node.neighbours.Length; m++)
					{
						Gizmos.DrawLine(node.vectorPos, node.neighbours[m].vectorPos);
					}
				}
			}
			if (gridGenerator == GridGenerator.Mesh && navmesh != null)
			{
				Color grey = Color.grey;
				grey.a = 0.7f;
				Gizmos.color = grey;
				Vector3[] vertices = navmesh.vertices;
				int[] triangles = navmesh.triangles;
				for (int n = 0; n < triangles.Length / 3; n++)
				{
					Vector3 from = rotationMatrix.TransformVector(vertices[triangles[n * 3]]);
					Vector3 vector = rotationMatrix.TransformVector(vertices[triangles[n * 3 + 1]]);
					Vector3 to = rotationMatrix.TransformVector(vertices[triangles[n * 3 + 2]]);
					Gizmos.DrawLine(from, vector);
					Gizmos.DrawLine(from, to);
					Gizmos.DrawLine(vector, to);
				}
			}
		}
		if (staticNodes == null || !showLinks)
		{
			return;
		}
		for (int num4 = 0; num4 < links.Length; num4++)
		{
			if (grids.Length == 0)
			{
				break;
			}
			NodeLink nodeLink = links[num4];
			Int3 @int = ToLocal(nodeLink.fromVector);
			Node node2 = null;
			Int3 int2 = ToLocal(nodeLink.toVector);
			Node node3 = null;
			Gizmos.color = Color.green;
			if (@int != new Int3(-1, -1, -1) && !grids[@int.y].changed)
			{
				node2 = GetNode(@int);
			}
			else
			{
				Gizmos.color = Color.red;
			}
			if (int2 != new Int3(-1, -1, -1) && !grids[int2.y].changed)
			{
				node3 = GetNode(int2);
			}
			else
			{
				Gizmos.color = Color.red;
			}
			switch (nodeLink.linkType)
			{
			case LinkType.Link:
				Gizmos.DrawLine((node2 == null) ? nodeLink.fromVector : node2.vectorPos, (node3 == null) ? nodeLink.toVector : node3.vectorPos);
				break;
			case LinkType.NodeDisabler:
				if (node2 != null)
				{
					Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
					Gizmos.DrawSphere(node2.vectorPos, grids[@int.y].nodeSize * 0.4f);
				}
				else
				{
					Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.6f);
					Gizmos.DrawSphere(nodeLink.fromVector, grids[0].nodeSize * 0.4f);
				}
				break;
			case LinkType.NodeEnabler:
				if (node2 != null)
				{
					Gizmos.color = new Color(0f, 1f, 0f, 0.6f);
					Gizmos.DrawSphere(node2.vectorPos, grids[@int.y].nodeSize * 0.4f);
				}
				else
				{
					Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.6f);
					Gizmos.DrawSphere(nodeLink.fromVector, grids[0].nodeSize * 0.4f);
				}
				break;
			}
		}
	}

	public int GetUnwalkableNodeAmount()
	{
		int num = 0;
		for (int i = 0; i < grids.Length; i++)
		{
			Node[,] array = staticNodes[i];
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int j = array.GetLowerBound(0); j <= upperBound; j++)
			{
				for (int k = array.GetLowerBound(1); k <= upperBound2; k++)
				{
					if (!array[j, k].walkable)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public int GetWalkableNodeAmount()
	{
		int num = 0;
		for (int i = 0; i < grids.Length; i++)
		{
			Node[,] array = staticNodes[i];
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int j = array.GetLowerBound(0); j <= upperBound; j++)
			{
				for (int k = array.GetLowerBound(1); k <= upperBound2; k++)
				{
					if (array[j, k].walkable)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	private static IEnumerator pathQueueManager()
	{
		while (pathQueue != null)
		{
			if (pathQueue.Count == 0)
			{
				yield return new WaitForSeconds(0.25f);
				continue;
			}
			Path current = pathQueue[0] as Path;
			if (!current.error)
			{
				active.StartCoroutine(StartPathYield(current));
				while (!current.error && !current.foundEnd)
				{
					yield return 0;
				}
				if (active.pathsThisFrame >= active.maxPathsPerFrame && active.lastPathFrame == Time.frameCount)
				{
					yield return 0;
				}
			}
			pathQueue.RemoveAt(0);
		}
	}

	public static IEnumerator StartPath(Path p)
	{
		if (!p.error)
		{
			if (pathQueue == null)
			{
				pathQueue = new ArrayList();
				active.StartCoroutine(pathQueueManager());
			}
			pathQueue.Add(p);
		}
		yield break;
	}

	public static IEnumerator StartPathYield(Path p)
	{
		if (p.error)
		{
			yield break;
		}
		if (active.lastPathFrame != Time.frameCount)
		{
			active.pathsThisFrame = 0;
			active.lastPathFrame = Time.frameCount;
		}
		active.lastPath = p;
		p.Init();
		if (!p.error && !p.foundEnd)
		{
			while (!p.foundEnd && !p.error)
			{
				yield return 0;
				active.pathsThisFrame = 1;
				p.Calc();
			}
		}
	}

	public void Awake()
	{
		active = this;
		if (cachePaths)
		{
			cache = new Path[cacheLimit];
		}
		else
		{
			cache = new Path[1];
		}
		if (PlayerPrefs.GetInt("isWil") == 1)
		{
			maxFrameTime = 0.35f;
		}
		else
		{
			maxFrameTime = 0.1f;
		}
	}

	public void OnInitialize()
	{
		if (calculateOnStartup)
		{
			active.StopAllCoroutines();
			if (pathQueue != null)
			{
				pathQueue.Clear();
			}
			active = this;
			Scan(true, 0);
		}
	}

	public static Int3 ToLocal(Int3 pos)
	{
		if (active.gridGenerator == GridGenerator.Bounds || active.gridGenerator == GridGenerator.Mesh || active.gridGenerator == GridGenerator.List || active.gridGenerator == GridGenerator.Procedural)
		{
			return ToLocalTest(pos);
		}
		for (int i = 0; i < active.grids.Length; i++)
		{
			AstarClasses.Grid grid = active.grids[i];
			if (grid.Contains(pos))
			{
				pos -= new Int3(grid.offset.x, grid.offset.z);
				pos.x = Mathf.RoundToInt((float)pos.x / grid.nodeSize);
				pos.z = Mathf.RoundToInt((float)pos.z / grid.nodeSize);
				pos.y = i;
				return pos;
			}
		}
		return new Int3(-1, -1, -1);
	}

	public static Int3 ToLocalTest(Vector3 pos)
	{
		Node node = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < active.grids.Length; i++)
		{
			AstarClasses.Grid grid = active.grids[i];
			if (!grid.Contains(pos))
			{
				continue;
			}
			for (int j = 0; j < grid.depth; j++)
			{
				for (int k = 0; k < grid.width; k++)
				{
					Node node2 = GetNode(k, i, j);
					float sqrMagnitude = (pos - node2.vectorPos).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						node = node2;
						num = sqrMagnitude;
					}
				}
			}
		}
		if (node == null)
		{
			return new Int3(-1, -1, -1);
		}
		return node.pos;
	}

	public static Int3 ToLocal(Vector3 Vpos, int gridIndex)
	{
		if (active.gridGenerator == GridGenerator.Bounds || active.gridGenerator == GridGenerator.Mesh || active.gridGenerator == GridGenerator.List || active.gridGenerator == GridGenerator.Procedural)
		{
			return ToLocalTest(Vpos);
		}
		AstarClasses.Grid grid = active.grids[gridIndex];
		if (grid.Contains(Vpos))
		{
			Vpos -= new Vector3(grid.offset.x, 0f, grid.offset.z);
			Int3 result = Vpos;
			result.x = Mathf.RoundToInt(Vpos.x / grid.nodeSize);
			result.z = Mathf.RoundToInt(Vpos.z / grid.nodeSize);
			result.y = gridIndex;
			return result;
		}
		return new Int3(-1, -1, -1);
	}

	public static Int3 ToLocal(Vector3 Vpos)
	{
		if (active.gridGenerator == GridGenerator.Bounds || active.gridGenerator == GridGenerator.Mesh || active.gridGenerator == GridGenerator.List || active.gridGenerator == GridGenerator.Procedural)
		{
			return ToLocalTest(Vpos);
		}
		for (int i = 0; i < active.grids.Length; i++)
		{
			AstarClasses.Grid grid = active.grids[i];
			if (grid.Contains(Vpos))
			{
				Vpos -= new Vector3(grid.offset.x, 0f, grid.offset.z);
				Int3 result = Vpos;
				result.x = Mathf.RoundToInt(Vpos.x / grid.nodeSize);
				result.z = Mathf.RoundToInt(Vpos.z / grid.nodeSize);
				result.y = i;
				return result;
			}
		}
		return new Int3(-1, -1, -1);
	}

	public static int ToLocalX(int pos, int level)
	{
		pos -= (int)active.grids[level].offset.x;
		pos = Mathf.RoundToInt((float)pos / active.grids[level].nodeSize);
		return pos;
	}

	public static int ToLocalZ(int pos, int level)
	{
		pos -= (int)active.grids[level].offset.z;
		pos = Mathf.RoundToInt((float)pos / active.grids[level].nodeSize);
		return pos;
	}

	public static float ToLocalX(float pos, int level)
	{
		pos -= active.grids[level].offset.x;
		pos /= active.grids[level].nodeSize;
		return pos;
	}

	public static float ToLocalZ(float pos, int level)
	{
		pos -= active.grids[level].offset.z;
		pos /= active.grids[level].nodeSize;
		return pos;
	}

	public static Node GetNode(Int3 pos)
	{
		return GetNode(pos.x, pos.y, pos.z);
	}

	public static Node GetNode(int x, int y, int z)
	{
		return active.staticNodes[y][x, z];
	}

	public IEnumerator SetNodes(bool walkable, Bounds bounds, bool fullPhysicsCheck, float t)
	{
		yield return new WaitForSeconds(t);
		SetNodes(walkable, bounds, fullPhysicsCheck, false, -1);
	}

	public IEnumerator SetNodes(bool walkable, Bounds bounds, bool fullPhysicsCheck, bool allGrids, float t)
	{
		yield return new WaitForSeconds(t);
		SetNodes(walkable, bounds, fullPhysicsCheck, allGrids, -1);
	}

	public void SetNodes(bool walkable, Bounds bounds, bool fullPhysicsCheck)
	{
		SetNodes(walkable, bounds, fullPhysicsCheck, false, -1);
	}

	public void SetNodes(bool walkable, Bounds bounds, bool fullPhysicsCheck, bool allGrids)
	{
		SetNodes(walkable, bounds, fullPhysicsCheck, allGrids, -1);
	}

	public void SetNodes(bool walkable, Vector3 point, int gridIndex, bool allGrids)
	{
		if (gridGenerator != 0 && gridGenerator != GridGenerator.Texture)
		{
			Debug.LogError("The SetNodes function can not be used with grid generators other than 'Texture' and 'Grid'");
			return;
		}
		if (allGrids)
		{
			for (int i = 0; i < grids.Length; i++)
			{
				AstarClasses.Grid grid = grids[i];
				Int3 @int = ToLocal(point, i);
				if (!(@int != new Int3(-1, -1, -1)))
				{
					continue;
				}
				Node node = GetNode(@int);
				node.walkable = walkable;
				RecalcNeighbours(node);
				for (int j = Mathf.Clamp(@int.z - 1, 0, grid.depth); j <= @int.z + 1; j++)
				{
					for (int k = Mathf.Clamp(@int.x - 1, 0, grid.width); k <= @int.x + 1; k++)
					{
						if (k < grid.width && j < grid.depth)
						{
							Node node2 = GetNode(new Int3(k, i, j));
							RecalcNeighbours(node2);
						}
					}
				}
			}
			return;
		}
		Int3 int2 = ToLocal(point, gridIndex);
		if (!(int2 != new Int3(-1, -1, -1)))
		{
			return;
		}
		AstarClasses.Grid grid2 = grids[int2.y];
		Node node3 = GetNode(int2);
		node3.walkable = walkable;
		RecalcNeighbours(node3);
		for (int l = Mathf.Clamp(int2.z - 1, 0, grid2.depth); l <= int2.z + 1; l++)
		{
			for (int m = Mathf.Clamp(int2.x - 1, 0, grid2.width); m <= int2.x + 1; m++)
			{
				if (m < grid2.width && l < grid2.depth)
				{
					Node node4 = GetNode(new Int3(m, gridIndex, l));
					RecalcNeighbours(node4);
				}
			}
		}
	}

	public void SetNodes(bool walkable, Bounds bounds, bool fullPhysicsCheck, bool allGrids, LayerMask extraMask)
	{
		if (gridGenerator != 0 && gridGenerator != GridGenerator.Texture)
		{
			Debug.LogError("The SetNodes function can not be used with grid generators other than 'Texture' and 'Grid'");
			return;
		}
		Vector3 min = bounds.min;
		Vector3 vector = bounds.max - min;
		Rect rect = new Rect(min.x, min.z, vector.x, vector.z);
		Debug.Log("Changing the Grid...");
		if (allGrids)
		{
			bool flag = false;
			for (int i = 0; i < grids.Length; i++)
			{
				Int3 @int = ToLocal(bounds.center, i);
				if (@int != new Int3(-1, -1, -1))
				{
					SetNodesWorld(walkable, rect, @int.y, fullPhysicsCheck, extraMask);
					flag = true;
				}
			}
			if (!flag)
			{
				Debug.LogWarning("Can't set nodes, area center is outside all grids");
			}
		}
		else
		{
			Int3 int2 = ToLocal(bounds.center);
			if (int2 != new Int3(-1, -1, -1))
			{
				SetNodesWorld(walkable, rect, int2.y, fullPhysicsCheck, extraMask);
			}
			else
			{
				Debug.LogWarning("Can't set nodes, area center is outside all grids");
			}
		}
	}

	public void SetNodesWorld(bool walkable, Rect rect, int level, bool fullPhysicsCheck, LayerMask extraMask)
	{
		if (gridGenerator != 0 && gridGenerator != GridGenerator.Texture)
		{
			Debug.LogError("The SetNodes function can not be used with grid generators other than 'Texture' and 'Grid'");
			return;
		}
		rect.x = Mathf.Floor(ToLocalX(rect.x, level));
		rect.y = Mathf.Floor(ToLocalZ(rect.y, level));
		rect.width = Mathf.Ceil(rect.width / grids[level].nodeSize);
		rect.height = Mathf.Ceil(rect.height / grids[level].nodeSize);
		if (fullPhysicsCheck)
		{
			RecalculateArea(rect, level, extraMask);
		}
		else
		{
			SetNodesLocal(walkable, rect, level);
		}
	}

	public void SetNodesLocal(bool walkable, Rect rect, int level)
	{
		AstarClasses.Grid grid = grids[level];
		rect = new Rect(Mathf.Clamp(rect.x + 1f, 0f, grid.width - 1), Mathf.Clamp(rect.y + 1f, 0f, grid.depth - 1), Mathf.Clamp(rect.width, 0f, grid.width - 1), Mathf.Clamp(rect.height, 0f, grid.depth - 1));
		int num = (int)rect.x;
		int num2 = (int)rect.y;
		int num3 = (int)rect.xMax;
		int num4 = (int)rect.yMax;
		for (int i = num2; i < num4; i++)
		{
			for (int j = num; j < num3; j++)
			{
				GetNode(j, level, i).walkable = walkable;
			}
		}
		bool flag = false;
		if (walkable)
		{
			for (int k = num2 - 1; k < num4 + 1; k++)
			{
				for (int l = num - 1; l < num3 + 1; l++)
				{
					if (k >= 0 && k < grid.depth && l >= 0 && l < grid.width)
					{
						Node node = GetNode(l, level, k);
						if (node.walkable)
						{
							RecalcNeighbours(node);
						}
						else
						{
							flag = true;
						}
					}
				}
			}
		}
		else
		{
			for (int m = num2 - 1; m < num4 + 1; m++)
			{
				for (int n = num - 1; n < num3 + 1; n++)
				{
					if (m >= 0 && m < grid.depth && n >= 0 && n < grid.width && (m < num2 || m > num4 - 1))
					{
						Node node2 = GetNode(n, level, m);
						if (node2.walkable)
						{
							RecalcNeighbours(node2);
						}
						else
						{
							flag = true;
						}
					}
					else if (n >= 0 && n < grid.width && m >= 0 && m < grid.depth && (n < num || n > num3 - 1))
					{
						Node node3 = GetNode(n, level, m);
						if (node3.walkable)
						{
							RecalcNeighbours(node3);
						}
						else
						{
							flag = true;
						}
					}
				}
			}
		}
		int areaTimeStamp = Mathf.RoundToInt(UnityEngine.Random.Range(0, 10000));
		if (flag && !walkable)
		{
			for (int num5 = num2 - 1; num5 < num4 + 1; num5++)
			{
				for (int num6 = num - 1; num6 < num3 + 1; num6++)
				{
					if (num5 >= 0 && num5 < grid.depth && num6 >= 0 && num6 < grid.width && (num5 < num2 || num5 > num4 - 1 || num6 < num || num6 > num3 - 1))
					{
						Node node4 = GetNode(num6, level, num5);
						if (node4.walkable)
						{
							FloodFill(node4, areaTimeStamp);
						}
					}
				}
			}
		}
		else
		{
			FloodFill(GetNode(num, level, num2), areaTimeStamp);
		}
	}

	public void RecalculateArea(Rect rect, int level, LayerMask extraMask)
	{
		AstarClasses.Grid grid = grids[level];
		rect = new Rect(Mathf.Clamp(rect.x + 1f, 0f, grid.width - 1), Mathf.Clamp(rect.y + 1f, 0f, grid.depth - 1), Mathf.Clamp(rect.width, 0f, grid.width - 1), Mathf.Clamp(rect.height, 0f, grid.depth - 1));
		int num = (int)rect.x;
		int num2 = (int)rect.y;
		int num3 = (int)rect.xMax;
		int num4 = (int)rect.yMax;
		int num5 = Mathf.CeilToInt(grid.physicsRadius);
		for (int i = num2 - num5; i < num4 + num5; i++)
		{
			for (int j = num - num5; j < num3 + num5; j++)
			{
				if (i >= 0 && i < grid.depth && j >= 0 && j < grid.width)
				{
					FullPhysicsCheck(GetNode(j, level, i), grids[level], extraMask);
				}
			}
		}
		for (int k = num2 - num5 - 1; k < num4 + num5 + 1; k++)
		{
			for (int l = num - num5 - 1; l < num3 + num5 + 1; l++)
			{
				if (k >= 0 && k < grid.depth && l >= 0 && l < grid.width)
				{
					Node node = GetNode(l, level, k);
					if (node.walkable)
					{
						RecalcNeighbours(node);
					}
				}
			}
		}
		int areaTimeStamp = Mathf.RoundToInt(UnityEngine.Random.Range(0, 10000));
		for (int m = num2 - num5 - 1; m < num4 + num5 + 1; m++)
		{
			for (int n = num - num5 - 1; n < num3 + num5 + 1; n++)
			{
				if (m >= 0 && m < grid.depth && n >= 0 && n < grid.width)
				{
					Node node2 = GetNode(n, level, m);
					if (node2.walkable)
					{
						FloodFill(node2, areaTimeStamp);
					}
				}
			}
		}
	}

	public void RecalcNeighbours(Node node)
	{
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		int x = node.pos.x;
		int y = node.pos.y;
		int z = node.pos.z;
		AstarClasses.Grid grid = grids[y];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		float num5 = staticMaxAngle / 90f;
		if (x != 0 && GetNode(x - 1, y, z).walkable && (isNeighbours == IsNeighbour.Eight || isNeighbours == IsNeighbour.Four) && node.angles[3] < num5)
		{
			arrayList2.Add(GetNode(x - 1, y, z));
			arrayList.Add(3);
			num++;
			num3++;
		}
		if (x != grid.width - 1 && GetNode(x + 1, y, z).walkable && (isNeighbours == IsNeighbour.Eight || isNeighbours == IsNeighbour.Four) && node.angles[4] < num5)
		{
			arrayList2.Add(GetNode(x + 1, y, z));
			arrayList.Add(4);
			num2++;
			num4++;
		}
		if (z != 0)
		{
			if (GetNode(x, y, z - 1).walkable && (isNeighbours == IsNeighbour.Eight || isNeighbours == IsNeighbour.Four) && node.angles[6] < num5)
			{
				arrayList2.Add(GetNode(x, y, z - 1));
				arrayList.Add(6);
				num3++;
				num4++;
			}
			if (x != 0 && (num3 == 2 || !dontCutCorners) && GetNode(x - 1, y, z - 1).walkable && isNeighbours == IsNeighbour.Eight && node.angles[5] < num5)
			{
				arrayList2.Add(GetNode(x - 1, y, z - 1));
				arrayList.Add(5);
			}
			if (x != grid.width - 1 && (num4 == 2 || !dontCutCorners) && GetNode(x + 1, y, z - 1).walkable && isNeighbours == IsNeighbour.Eight && node.angles[7] < num5)
			{
				arrayList2.Add(GetNode(x + 1, y, z - 1));
				arrayList.Add(7);
			}
		}
		if (z != grid.depth - 1)
		{
			if (GetNode(x, y, z + 1).walkable && (isNeighbours == IsNeighbour.Eight || isNeighbours == IsNeighbour.Four) && node.angles[1] < num5)
			{
				arrayList2.Add(GetNode(x, y, z + 1));
				arrayList.Add(1);
				num++;
				num2++;
			}
			if (x != 0 && (num == 2 || !dontCutCorners) && GetNode(x - 1, y, z + 1).walkable && isNeighbours == IsNeighbour.Eight && node.angles[0] < num5)
			{
				arrayList2.Add(GetNode(x - 1, y, z + 1));
				arrayList.Add(0);
			}
			if (x != grid.width - 1 && (num2 == 2 || !dontCutCorners) && GetNode(x + 1, y, z + 1).walkable && isNeighbours == IsNeighbour.Eight && node.angles[2] < num5)
			{
				arrayList2.Add(GetNode(x + 1, y, z + 1));
				arrayList.Add(2);
			}
		}
		node.neighbours = arrayList2.ToArray(typeof(Node)) as Node[];
		node.neighboursKeys = arrayList.ToArray(typeof(int)) as int[];
	}

	public void FullPhysicsCheck(Node node, AstarClasses.Grid grid, LayerMask extraMask)
	{
		bool flag = false;
		switch (heightMode)
		{
		case Height.Flat:
			node.vectorPos.y = grid.offset.y;
			break;
		case Height.Terrain:
			if ((bool)Terrain.activeTerrain)
			{
				node.vectorPos.y = Terrain.activeTerrain.SampleHeight(node.vectorPos);
			}
			break;
		case Height.Raycast:
		{
			node.vectorPos.y = grid.offset.y;
			RaycastHit hitInfo;
			if (Physics.Raycast(node.vectorPos + new Vector3(0f, grid.height, 0f), -Vector3.up, out hitInfo, grid.height + 0.001f, groundLayer))
			{
				node.vectorPos.y = hitInfo.point.y;
				if (useNormal)
				{
					Vector3 normal = hitInfo.normal;
					if (Vector3.Angle(Vector3.up, normal) > staticMaxAngle)
					{
						node.walkable = false;
						flag = true;
					}
				}
			}
			else
			{
				node.walkable = false;
				flag = true;
			}
			break;
		}
		}
		if (!flag)
		{
			if (Physics.CheckCapsule(node.vectorPos, node.vectorPos + Vector3.up * grid.capsuleHeight, 0.5f * grid.nodeSize * grid.physicsRadius, grid.physicsMask))
			{
				node.walkable = false;
			}
			else
			{
				node.walkable = true;
			}
		}
	}

	public static bool CheckLine(Node from, Node to, float maxAngle)
	{
		if (from.pos.y != to.pos.y)
		{
			return false;
		}
		Vector3 vector = to.pos - from.pos;
		Vector3 normalized = vector.normalized;
		Vector3 vector2 = -Vector3.one;
		for (float num = 0f; num < vector.magnitude; num += active.lineAccuracy)
		{
			Node node = GetNode(from.pos + (Int3)(normalized * num));
			if (num > 0f && vector2 != node.vectorPos)
			{
				Vector3 to2;
				Vector3 from2 = (to2 = node.vectorPos - vector2);
				to2.y = 0f;
				if (Vector3.Angle(from2, to2) >= maxAngle)
				{
					return false;
				}
			}
			vector2 = node.vectorPos;
			if (!node.walkable)
			{
				return false;
			}
		}
		return true;
	}

	public void FloodFill(Node node, int areaTimeStamp)
	{
		if (node.areaTimeStamp == areaTimeStamp || !node.walkable)
		{
			return;
		}
		area++;
		ArrayList arrayList = new ArrayList(areaColors);
		arrayList.Add(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
		areaColors = arrayList.ToArray(typeof(Color)) as Color[];
		int num = 0;
		ArrayList arrayList2 = new ArrayList();
		Node node2 = null;
		arrayList2.Add(node);
		while (arrayList2.Count > 0)
		{
			num++;
			if (num > totalNodeAmount)
			{
				Debug.Log("Infinity Loop");
			}
			node2 = arrayList2[0] as Node;
			node2.area = area;
			arrayList2.Remove(node2);
			for (int i = 0; i < node2.neighbours.Length; i++)
			{
				if (node2.neighbours[i].areaTimeStamp != areaTimeStamp)
				{
					node2.neighbours[i].areaTimeStamp = areaTimeStamp;
					arrayList2.Add(node2.neighbours[i]);
				}
			}
		}
		Debug.Log("Flood Filled " + num + " Nodes, The Grid now contains " + area + " Areas");
	}

	[ContextMenu("Scan Map")]
	public void Scan(bool calcAll, int pass)
	{
		active = this;
		for (int i = 0; i < grids.Length; i++)
		{
			grids[i].Reset();
			grids[i].offset += base.transform.root.position;
		}
		if (gridGenerator == GridGenerator.Texture)
		{
			if (calcAll)
			{
				ScanTexture();
			}
			else
			{
				Debug.LogWarning("The Texture Mode don't use passes, calculate everything once instead");
			}
			binaryHeap = new BinaryHeap(Mathf.CeilToInt((float)totalNodeAmount * heapSize));
		}
		else if (gridGenerator == GridGenerator.Bounds)
		{
			if (calcAll)
			{
				ScanBounds();
			}
			else
			{
				Debug.LogWarning("The Bounds Mode don't use passes, calculate everything once instead");
			}
			binaryHeap = new BinaryHeap(Mathf.CeilToInt((float)totalNodeAmount * heapSize));
		}
		else if (gridGenerator == GridGenerator.List)
		{
			if (calcAll)
			{
				ScanList();
			}
			else
			{
				Debug.LogWarning("The List Mode don't use passes, calculate everything once instead");
			}
			binaryHeap = new BinaryHeap(Mathf.CeilToInt((float)totalNodeAmount * heapSize));
		}
		else if (gridGenerator == GridGenerator.Mesh)
		{
			if (calcAll)
			{
				ScanNavmesh();
			}
			else
			{
				Debug.LogWarning("The Mesh Mode don't use passes, calculate everything once instead");
			}
			binaryHeap = new BinaryHeap(Mathf.CeilToInt((float)totalNodeAmount * heapSize));
		}
		else
		{
			if (gridGenerator == GridGenerator.Procedural)
			{
				return;
			}
			if (pass == 1 || calcAll)
			{
				staticNodes = new Node[grids.Length][,];
				totalNodeAmount = 0;
				for (int j = 0; j < grids.Length; j++)
				{
					staticNodes[j] = new Node[grids[j].width, grids[j].depth];
					totalNodeAmount += grids[j].width * grids[j].depth;
					grids[j].globalWidth = grids[j].width - 1;
					grids[j].globalDepth = grids[j].depth - 1;
					grids[j].changed = false;
				}
			}
			bool flag = false;
			for (int k = 0; k < grids.Length; k++)
			{
				if (!(pass == 1 || calcAll))
				{
					break;
				}
				AstarClasses.Grid grid = grids[k];
				for (int l = 0; l < grid.depth; l++)
				{
					for (int m = 0; m < grid.width; m++)
					{
						Node node = (staticNodes[k][m, l] = new Node());
						node.pos = new Int3(m, k, l);
						node.vectorPos = new Vector3((float)m * grid.nodeSize + grid.offset.x, grid.offset.y, (float)l * grid.nodeSize + grid.offset.z);
						FullPhysicsCheck(node, grid, 0);
						if (node.walkable)
						{
							flag = true;
						}
					}
				}
			}
			for (int n = 0; n < grids.Length; n++)
			{
				if (!(pass == 2 || calcAll))
				{
					break;
				}
				AstarClasses.Grid grid2 = grids[n];
				for (int num = 0; num < grid2.depth; num++)
				{
					for (int num2 = 0; num2 < grid2.width; num2++)
					{
						Node node2 = GetNode(num2, n, num);
						if (num2 != 0)
						{
							Vector3 from = GetNode(num2 - 1, n, num).vectorPos - node2.vectorPos;
							node2.angles[3] = Vector3.Angle(from, -Vector3.right) / 90f;
							GetNode(num2 - 1, n, num).angles[4] = node2.angles[3];
						}
						if (num != 0)
						{
							Vector3 from = GetNode(num2, n, num - 1).vectorPos - node2.vectorPos;
							node2.angles[6] = Vector3.Angle(from, -Vector3.forward) / 90f;
							GetNode(num2, n, num - 1).angles[1] = node2.angles[6];
							if (num2 != 0)
							{
								from = GetNode(num2 - 1, n, num - 1).vectorPos - node2.vectorPos;
								node2.angles[5] = Vector3.Angle(from, -Vector3.right - Vector3.forward) / 90f;
								GetNode(num2 - 1, n, num - 1).angles[2] = node2.angles[5];
							}
							if (num2 != grid2.width - 1)
							{
								from = GetNode(num2 + 1, n, num - 1).vectorPos - node2.vectorPos;
								node2.angles[7] = Vector3.Angle(from, Vector3.right - Vector3.forward) / 90f;
								GetNode(num2 + 1, n, num - 1).angles[0] = node2.angles[7];
							}
						}
					}
				}
			}
			if (pass == 3 || calcAll)
			{
				ApplyEnablerLinks();
			}
			for (int num3 = 0; num3 < grids.Length; num3++)
			{
				if (!(pass == 3 || calcAll))
				{
					break;
				}
				AstarClasses.Grid grid3 = grids[num3];
				for (int num4 = 0; num4 < grid3.depth; num4++)
				{
					for (int num5 = 0; num5 < grid3.width; num5++)
					{
						Node node3 = GetNode(num5, num3, num4);
						RecalcNeighbours(node3);
					}
				}
			}
			if (pass == 3 || calcAll)
			{
				ApplyLinks();
			}
			if (pass == 4 || calcAll)
			{
				FloodFillAll();
			}
			if ((pass == 1 || calcAll) && !flag)
			{
				Debug.LogError("No nodes are walkable (maybe you should change the layer mask)");
			}
			binaryHeap = new BinaryHeap(Mathf.CeilToInt((float)totalNodeAmount * heapSize));
		}
	}

	public void FloodFillAll()
	{
		area = 0;
		int num = Mathf.RoundToInt(UnityEngine.Random.Range(0, 10000));
		int num2 = 0;
		ArrayList arrayList = new ArrayList();
		Node node = null;
		ArrayList arrayList2 = new ArrayList();
		arrayList2.Add(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
		int num3 = 0;
		for (int i = 0; i < grids.Length; i++)
		{
			AstarClasses.Grid grid = grids[i];
			for (int j = 0; j < grid.depth; j++)
			{
				for (int k = 0; k < grid.width; k++)
				{
					if (GetNode(k, i, j).walkable)
					{
						num3++;
					}
				}
			}
		}
		while (num2 < num3)
		{
			area++;
			arrayList2.Add(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value));
			if (area > 400)
			{
				Debug.Log("Preventing possible Infinity Loop (Searched " + num2 + " nodes in the flood fill pass)");
				break;
			}
			for (int l = 0; l < grids.Length; l++)
			{
				AstarClasses.Grid grid2 = grids[l];
				for (int m = 0; m < grid2.depth; m++)
				{
					for (int n = 0; n < grid2.width; n++)
					{
						Node node2 = GetNode(n, l, m);
						if (node2.walkable && node2.areaTimeStamp != num && node2.neighbours.Length != 0)
						{
							node2.areaTimeStamp = num;
							arrayList.Add(node2);
							m = grid2.depth;
							n = grid2.width;
							l = grids.Length;
						}
					}
				}
			}
			if (arrayList.Count == 0)
			{
				num2 = num3;
				area--;
				break;
			}
			while (arrayList.Count > 0)
			{
				num2++;
				if (num2 > num3)
				{
					Debug.LogError("Infinity Loop, can't flood fill more than the total node amount (System Failure)");
					break;
				}
				node = arrayList[0] as Node;
				node.area = area;
				node.areaTimeStamp = num;
				arrayList.Remove(node);
				for (int num4 = 0; num4 < node.neighbours.Length; num4++)
				{
					if (node.neighbours[num4].areaTimeStamp != num)
					{
						node.neighbours[num4].areaTimeStamp = num;
						arrayList.Add(node.neighbours[num4]);
					}
				}
			}
			arrayList.Clear();
		}
		areaColors = arrayList2.ToArray(typeof(Color)) as Color[];
	}

	public void ScanTexture()
	{
		Color[] pixels = navTex.GetPixels(0);
		AstarClasses.Grid grid = textureGrid;
		grids = new AstarClasses.Grid[1] { grid };
		grid.width = navTex.width;
		grid.depth = navTex.height;
		grid.globalWidth = grid.width - 1;
		grid.globalDepth = grid.depth - 1;
		staticNodes = new Node[1][,];
		staticNodes[0] = new Node[grid.width, grid.depth];
		totalNodeAmount = grid.depth * grid.width;
		for (int i = 0; i < grid.depth; i++)
		{
			for (int j = 0; j < grid.width; j++)
			{
				Node node = new Node();
				node.pos = new Int3(j, 0, i);
				node.vectorPos = new Vector3((float)j * grid.nodeSize, 0f, (float)i * grid.nodeSize) + grid.offset;
				if (pixels[i * grid.width + j].grayscale <= threshold)
				{
					node.walkable = false;
				}
				node.penalty = (int)(pixels[i * grid.width + j].r * (float)penaltyMultiplier);
				node.angles = new float[9];
				staticNodes[0][j, i] = node;
			}
		}
		ApplyEnablerLinks();
		for (int k = 0; k < grid.depth; k++)
		{
			for (int l = 0; l < grid.width; l++)
			{
				Node node2 = GetNode(l, 0, k);
				RecalcNeighbours(node2);
			}
		}
		ApplyLinks();
		FloodFillAll();
	}

	public void ScanTilemap(bool[] array, int width, int depth)
	{
		if (width * depth != array.Length)
		{
			Debug.LogError("The array length and width*depth values must match");
			return;
		}
		if (gridGenerator != GridGenerator.Texture)
		{
			Debug.LogError("Only use this grid generator with the Texture mode");
			return;
		}
		if (!calculateOnStartup)
		{
			Debug.LogWarning("To prevent that other grids gets generated at startup just to be replaced by this grid you should switch Calculate Grid On Startup to FALSE");
		}
		AstarClasses.Grid grid = textureGrid;
		grids = new AstarClasses.Grid[1] { grid };
		grid.width = width;
		grid.depth = depth;
		grid.globalWidth = grid.width - 1;
		grid.globalDepth = grid.depth - 1;
		staticNodes = new Node[1][,];
		staticNodes[0] = new Node[grid.width, grid.depth];
		totalNodeAmount = grid.depth * grid.width;
		for (int i = 0; i < grid.depth; i++)
		{
			for (int j = 0; j < grid.width; j++)
			{
				Node node = new Node();
				node.pos = new Int3(j, 0, i);
				node.vectorPos = new Vector3((float)j * grid.nodeSize, 0f, (float)i * grid.nodeSize) + grid.offset;
				if (!array[i * grid.depth + j])
				{
					node.walkable = false;
				}
				node.angles = new float[9];
				staticNodes[0][j, i] = node;
			}
		}
		ApplyEnablerLinks();
		for (int k = 0; k < grid.depth; k++)
		{
			for (int l = 0; l < grid.width; l++)
			{
				Node node2 = GetNode(l, 0, k);
				RecalcNeighbours(node2);
			}
		}
		ApplyLinks();
		FloodFillAll();
	}

	public void ScanBounds()
	{
		Collider[] array = UnityEngine.Object.FindObjectsOfType(typeof(Collider)) as Collider[];
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].isTrigger && array[i].gameObject.tag == boundsTag)
			{
				arrayList.Add(array[i]);
			}
		}
		array = arrayList.ToArray(typeof(Collider)) as Collider[];
		Vector3[] array2 = new Vector3[array.Length * 4];
		for (int j = 0; j < array.Length; j++)
		{
			Bounds bounds = array[j].bounds;
			array2[j * 4] = new Vector3(bounds.extents.x + boundMargin, 0f, bounds.extents.z + boundMargin) + bounds.center;
			array2[j * 4 + 1] = new Vector3(0f - bounds.extents.x - boundMargin, 0f, 0f - bounds.extents.z - boundMargin) + bounds.center;
			array2[j * 4 + 2] = new Vector3(bounds.extents.x + boundMargin, 0f, 0f - bounds.extents.z - boundMargin) + bounds.center;
			array2[j * 4 + 3] = new Vector3(0f - bounds.extents.x - boundMargin, 0f, bounds.extents.z + boundMargin) + bounds.center;
		}
		ArrayList arrayList2 = new ArrayList();
		for (int k = 0; k < array2.Length; k++)
		{
			bool flag = false;
			for (int l = 0; l < array2.Length; l++)
			{
				if (array2[k] == array2[l] && k != l)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				arrayList2.Add(array2[k]);
			}
		}
		array2 = arrayList2.ToArray(typeof(Vector3)) as Vector3[];
		GenerateNavmesh(array2);
	}

	public void ScanNavmesh()
	{
		if (meshNodePosition == MeshNodePosition.Edge)
		{
			ScanNavmeshEdge();
		}
		else
		{
			ScanNavmeshCenter();
		}
	}

	public void ScanNavmeshEdge()
	{
		Vector3[] vertices = navmesh.vertices;
		int[] triangles = navmesh.triangles;
		AstarClasses.Grid grid = meshGrid;
		rotationMatrix = (Matrix.RotateY(navmeshRotation.x) * Matrix.RotateX(navmeshRotation.y) * Matrix.RotateZ(navmeshRotation.z) * Matrix.Scale(0f - grid.nodeSize)).translate(grid.offset.x, grid.offset.y, grid.offset.z);
		if (vertices.Length <= 3)
		{
			Debug.LogError("Mesh Scanner : Make sure the mesh does contains at least three vertices");
			return;
		}
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = rotationMatrix.TransformVector(vertices[i]);
		}
		Vector3[] array = new Vector3[triangles.Length];
		for (int j = 0; j < triangles.Length / 3; j++)
		{
			Vector3 vector = vertices[triangles[j * 3]];
			Vector3 vector2 = vertices[triangles[j * 3 + 1]];
			Vector3 vector3 = vertices[triangles[j * 3 + 2]];
			array[j * 3] = (vector + vector2) * 0.5f;
			array[j * 3 + 1] = (vector + vector3) * 0.5f;
			array[j * 3 + 2] = (vector2 + vector3) * 0.5f;
		}
		int[] array2 = new int[array.Length];
		int[] array3 = new int[array.Length];
		int num = 0;
		Vector3[] array4 = new Vector3[array.Length];
		for (int k = 0; k < array.Length; k++)
		{
			if (array2[k] == -1)
			{
				continue;
			}
			array2[num] = Mathf.FloorToInt((float)k / 3f);
			array3[num] = -k;
			array4[num] = array[k];
			for (int l = k + 1; l < array.Length; l++)
			{
				if (array2[l] != -1 && array[l] == array[k])
				{
					array2[l] = -1;
					array3[num] = Mathf.FloorToInt((float)l / 3f);
					break;
				}
			}
			if (array3[num] != -k)
			{
				num++;
			}
		}
		if (num == 0)
		{
			Debug.LogError("Mesh Scanner : Make sure there is at least one connection");
			return;
		}
		Vector3[] array5 = new Vector3[num + 1];
		for (int m = 0; m <= num; m++)
		{
			array5[m] = array4[m];
		}
		array = array5;
		Bounds bounds = new Bounds(array[0], Vector3.zero);
		for (int n = 0; n < array.Length; n++)
		{
			bounds.Encapsulate(array[n]);
		}
		bounds.extents += Vector3.one * boundsMargin;
		grids = new AstarClasses.Grid[1] { grid };
		grid.width = array.Length;
		grid.depth = 1;
		grid.globalWidth = bounds.size.x / grid.nodeSize;
		grid.globalDepth = bounds.size.z / grid.nodeSize;
		grid.globalHeight = bounds.size.y;
		grid.globalOffset = bounds.center - bounds.extents - grid.offset;
		staticNodes = new Node[1][,];
		staticNodes[0] = new Node[grid.width, grid.depth];
		totalNodeAmount = grid.width * grid.depth;
		for (int num2 = 0; num2 < grid.width; num2++)
		{
			Node node = new Node();
			node.pos = new Int3(num2, 0, 0);
			node.vectorPos = array[num2];
			staticNodes[0][num2, 0] = node;
		}
		ApplyEnablerLinks();
		for (int num3 = 0; num3 < grid.width; num3++)
		{
			Node node2 = staticNodes[0][num3, 0];
			if (!node2.walkable)
			{
				continue;
			}
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			int num4 = array2[num3];
			int num5 = array3[num3];
			for (int num6 = 0; num6 < grid.width; num6++)
			{
				if (num6 == num3)
				{
					continue;
				}
				int num7 = array2[num6];
				int num8 = array3[num6];
				if (num7 == num4 || num7 == num5 || num8 == num4 || num8 == num5)
				{
					Node node3 = staticNodes[0][num6, 0];
					if (node3.walkable)
					{
						float sqrMagnitude = (node3.vectorPos - node2.vectorPos).sqrMagnitude;
						arrayList2.Add(node3);
						arrayList3.Add(Mathf.RoundToInt(Mathf.Sqrt(sqrMagnitude) * 100f));
						arrayList.Add(arrayList.Count);
						Vector3 vector4 = node3.vectorPos - node2.vectorPos;
						Vector3 vector5 = vector4;
						vector5.y = 0f;
						float num9 = Vector3.Angle(vector4.normalized, vector5.normalized) / 90f;
						arrayList4.Add(num9);
					}
				}
			}
			node2.neighbours = arrayList2.ToArray(typeof(Node)) as Node[];
			node2.neighboursKeys = arrayList.ToArray(typeof(int)) as int[];
			node2.costs = arrayList3.ToArray(typeof(int)) as int[];
			node2.angles = arrayList4.ToArray(typeof(float)) as float[];
		}
		ApplyLinks();
		FloodFillAll();
	}

	public void ScanNavmeshCenter()
	{
		Vector3[] vertices = navmesh.vertices;
		int[] triangles = navmesh.triangles;
		AstarClasses.Grid grid = meshGrid;
		rotationMatrix = (Matrix.RotateY(navmeshRotation.x) * Matrix.RotateX(navmeshRotation.y) * Matrix.RotateZ(navmeshRotation.z) * Matrix.Scale(0f - grid.nodeSize)).translate(grid.offset.x, grid.offset.y, grid.offset.z);
		if (vertices.Length <= 2)
		{
			Debug.LogError("Make sure the mash does contains at least two vertices");
			return;
		}
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = rotationMatrix.TransformVector(vertices[i]);
		}
		Vector3[] array = new Vector3[triangles.Length / 3];
		for (int j = 0; j < triangles.Length / 3; j++)
		{
			array[j] = (vertices[triangles[j * 3]] + vertices[triangles[j * 3 + 1]] + vertices[triangles[j * 3 + 2]]) / 3f;
		}
		Bounds bounds = new Bounds(array[0], Vector3.zero);
		for (int k = 0; k < array.Length; k++)
		{
			bounds.Encapsulate(array[k]);
		}
		bounds.extents += Vector3.one * boundsMargin;
		grids = new AstarClasses.Grid[1] { grid };
		grid.width = array.Length;
		grid.depth = 1;
		grid.globalWidth = bounds.size.x / grid.nodeSize;
		grid.globalDepth = bounds.size.z / grid.nodeSize;
		grid.globalHeight = bounds.size.y;
		grid.globalOffset = bounds.center - bounds.extents - grid.offset;
		staticNodes = new Node[1][,];
		staticNodes[0] = new Node[grid.width, grid.depth];
		totalNodeAmount = grid.width * grid.depth;
		for (int l = 0; l < grid.width; l++)
		{
			Node node = new Node();
			node.pos = new Int3(l, 0, 0);
			node.vectorPos = array[l];
			staticNodes[0][l, 0] = node;
		}
		ApplyEnablerLinks();
		for (int m = 0; m < grid.width; m++)
		{
			Node node2 = staticNodes[0][m, 0];
			if (!node2.walkable)
			{
				continue;
			}
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			Vector3[] array2 = new Vector3[3]
			{
				vertices[triangles[m * 3]],
				vertices[triangles[m * 3 + 1]],
				vertices[triangles[m * 3 + 2]]
			};
			for (int n = 0; n < grid.width; n++)
			{
				if (n == m)
				{
					continue;
				}
				int num = 0;
				Vector3[] array3 = new Vector3[3]
				{
					vertices[triangles[n * 3]],
					vertices[triangles[n * 3 + 1]],
					vertices[triangles[n * 3 + 2]]
				};
				Vector3[] array4 = array2;
				foreach (Vector3 vector in array4)
				{
					Vector3[] array5 = array3;
					foreach (Vector3 vector2 in array5)
					{
						if (vector == vector2)
						{
							num++;
						}
					}
				}
				if (num >= 2)
				{
					Node node3 = staticNodes[0][n, 0];
					if (node3.walkable)
					{
						float sqrMagnitude = (node3.vectorPos - node2.vectorPos).sqrMagnitude;
						arrayList2.Add(node3);
						arrayList3.Add(Mathf.RoundToInt(Mathf.Sqrt(sqrMagnitude) * 100f));
						arrayList.Add(arrayList.Count);
						Vector3 vector3 = node3.vectorPos - node2.vectorPos;
						Vector3 vector4 = vector3;
						vector4.y = 0f;
						float num4 = Vector3.Angle(vector3.normalized, vector4.normalized) / 90f;
						arrayList4.Add(num4);
					}
				}
			}
			node2.neighbours = arrayList2.ToArray(typeof(Node)) as Node[];
			node2.neighboursKeys = arrayList.ToArray(typeof(int)) as int[];
			node2.costs = arrayList3.ToArray(typeof(int)) as int[];
			node2.angles = arrayList4.ToArray(typeof(float)) as float[];
		}
		ApplyLinks();
		FloodFillAll();
	}

	public void ScanList()
	{
		if (listRootNode == null)
		{
			Debug.LogError("No Root Node Was Assigned");
			return;
		}
		Transform[] children = GetChildren(listRootNode);
		Vector3[] array = new Vector3[children.Length];
		for (int i = 0; i < children.Length; i++)
		{
			array[i] = children[i].position;
		}
		GenerateNavmesh(array);
	}

	private Transform[] GetChildren(Transform parent)
	{
		Transform[] array = new Transform[parent.childCount];
		int num = 0;
		foreach (Transform item in parent)
		{
			Transform transform = (array[num] = item);
			num++;
		}
		return array;
	}

	public void ApplyEnablerLinks()
	{
		for (int i = 0; i < links.Length; i++)
		{
			NodeLink nodeLink = links[i];
			Int3 @int = ToLocal(nodeLink.fromVector);
			Node node = null;
			if (@int != new Int3(-1, -1, -1))
			{
				node = GetNode(@int);
				if (nodeLink.linkType == LinkType.NodeDisabler)
				{
					node.walkable = false;
				}
				else if (nodeLink.linkType == LinkType.NodeEnabler)
				{
					node.walkable = true;
				}
			}
		}
	}

	public void CreateGrid(SimpleNode[] nodes)
	{
		CreateGrid(new SimpleNode[1][] { nodes });
	}

	public void CreateGrid(SimpleNode[][] nodes)
	{
		if (nodes.Length < 1)
		{
			Debug.LogError("Make sure you use at least one grid");
		}
		AstarClasses.Grid[] array = new AstarClasses.Grid[nodes.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new AstarClasses.Grid();
			array[i].width = nodes[i].Length;
			array[i].depth = 1;
			if (array[i].width < 1)
			{
				Debug.LogError("Make sure you use at least one node for each grid");
				return;
			}
		}
		staticNodes = new Node[array.Length][,];
		totalNodeAmount = 0;
		for (int j = 0; j < array.Length; j++)
		{
			AstarClasses.Grid grid = array[j];
			staticNodes[j] = new Node[grid.width, grid.depth];
			totalNodeAmount += grid.width * grid.depth;
		}
		for (int k = 0; k < array.Length; k++)
		{
			SimpleNode[] array2 = nodes[k];
			for (int l = 0; l < array2.Length; l++)
			{
				Node node = new Node();
				node.pos = new Int3(l, k, 0);
				array2[l].pos = node.pos;
				node.vectorPos = array2[l].vectorPos;
				staticNodes[k][l, 0] = node;
			}
		}
		for (int m = 0; m < array.Length; m++)
		{
			AstarClasses.Grid grid2 = array[m];
			SimpleNode[] array3 = nodes[m];
			for (int n = 0; n < array3.Length; n++)
			{
				Node node2 = staticNodes[m][n, 0];
				node2.neighbours = new Node[array3[n].neighbours.Length];
				node2.costs = new int[array3[n].neighbours.Length];
				node2.angles = new float[array3[n].neighbours.Length];
				node2.neighboursKeys = new int[array3[n].neighbours.Length];
				for (int num = 0; num < node2.neighbours.Length; num++)
				{
					node2.neighbours[num] = GetNode(array3[n].neighbours[num].pos);
					node2.costs[num] = ((array3[n].costs == null) ? ((int)(Vector3.Distance(node2.neighbours[num].vectorPos, node2.vectorPos) * 100f)) : array3[n].costs[num]);
					node2.angles[num] = ((array3[n].angles == null) ? 0f : array3[n].angles[num]);
					node2.neighboursKeys[num] = num;
				}
			}
			Bounds bounds = new Bounds(staticNodes[m][0, 0].vectorPos, Vector3.zero);
			for (int num2 = 0; num2 < array3.Length; num2++)
			{
				bounds.Encapsulate(staticNodes[m][num2, 0].vectorPos);
			}
			bounds.extents += Vector3.one * boundsMargin;
			grid2.globalWidth = bounds.size.x;
			grid2.globalDepth = bounds.size.z;
			grid2.globalHeight = bounds.size.y;
			grid2.globalOffset = bounds.center - bounds.extents;
		}
		grids = array;
		FloodFillAll();
		binaryHeap = new BinaryHeap(Mathf.CeilToInt((float)totalNodeAmount * heapSize));
	}

	public void CreateGridOLD(SimpleNode[] nodes)
	{
		AstarClasses.Grid grid = new AstarClasses.Grid();
		grids = new AstarClasses.Grid[1] { grid };
		grid.width = nodes.Length;
		grid.depth = 1;
		staticNodes = new Node[1][,];
		staticNodes[0] = new Node[grid.width, grid.depth];
		totalNodeAmount = nodes.Length;
		for (int i = 0; i < nodes.Length; i++)
		{
			Node node = new Node();
			node.pos = new Int3(i, 0, 0);
			nodes[i].pos = node.pos;
			node.vectorPos = nodes[i].vectorPos;
			staticNodes[0][i, 0] = node;
		}
		for (int j = 0; j < nodes.Length; j++)
		{
			Node node2 = staticNodes[0][j, 0];
			node2.neighbours = new Node[nodes[j].neighbours.Length];
			node2.costs = new int[nodes[j].neighbours.Length];
			node2.angles = new float[nodes[j].neighbours.Length];
			node2.neighboursKeys = new int[nodes[j].neighbours.Length];
			for (int k = 0; k < node2.neighbours.Length; k++)
			{
				node2.neighbours[k] = GetNode(nodes[j].neighbours[k].pos);
				node2.costs[k] = ((nodes[j].costs == null) ? ((int)(Vector3.Distance(node2.neighbours[k].vectorPos, node2.vectorPos) * 100f)) : nodes[j].costs[k]);
				node2.angles[k] = ((nodes[j].angles == null) ? 0f : nodes[j].angles[k]);
				node2.neighboursKeys[k] = k;
			}
		}
		Bounds bounds = new Bounds(staticNodes[0][0, 0].vectorPos, Vector3.zero);
		for (int l = 0; l < nodes.Length; l++)
		{
			bounds.Encapsulate(staticNodes[0][l, 0].vectorPos);
		}
		bounds.extents += Vector3.one * boundsMargin;
		grid.globalWidth = bounds.size.x;
		grid.globalDepth = bounds.size.z;
		grid.globalHeight = bounds.size.y;
		grid.globalOffset = bounds.center - bounds.extents;
		FloodFillAll();
		binaryHeap = new BinaryHeap(Mathf.CeilToInt((float)totalNodeAmount * heapSize));
	}

	public void ApplyLinks()
	{
		for (int i = 0; i < links.Length; i++)
		{
			NodeLink nodeLink = links[i];
			if (nodeLink.linkType == LinkType.NodeDisabler || nodeLink.linkType == LinkType.NodeEnabler)
			{
				continue;
			}
			Int3 @int = ToLocal(nodeLink.fromVector);
			Node node = null;
			Int3 int2 = ToLocal(nodeLink.toVector);
			Node node2 = null;
			if (!(@int != new Int3(-1, -1, -1)))
			{
				continue;
			}
			node = GetNode(@int);
			if (!(int2 != new Int3(-1, -1, -1)))
			{
				continue;
			}
			node2 = GetNode(int2);
			if (node.walkable && node2.walkable)
			{
				ArrayList arrayList = ((node.costs == null) ? new ArrayList(costs) : new ArrayList(node.costs));
				ArrayList arrayList2 = new ArrayList(node.angles);
				ArrayList arrayList3 = new ArrayList(node.neighbours);
				ArrayList arrayList4 = new ArrayList(node.neighboursKeys);
				if (nodeLink.linkType == LinkType.Link)
				{
					arrayList4.Add(arrayList3.Count);
					arrayList2.Add(0);
					arrayList.Add((int)Vector3.Distance(node.vectorPos, node2.vectorPos));
					arrayList3.Add(node2);
				}
				node.neighbours = arrayList3.ToArray(typeof(Node)) as Node[];
				node.neighboursKeys = arrayList4.ToArray(typeof(int)) as int[];
				node.angles = arrayList2.ToArray(typeof(float)) as float[];
				node.costs = arrayList.ToArray(typeof(int)) as int[];
				if (!nodeLink.oneWay)
				{
					arrayList = ((node2.costs == null) ? new ArrayList(costs) : new ArrayList(node2.costs));
					arrayList2 = new ArrayList(node2.angles);
					arrayList3 = new ArrayList(node2.neighbours);
					arrayList4 = new ArrayList(node2.neighboursKeys);
					arrayList4.Add(arrayList3.Count);
					arrayList2.Add(0);
					arrayList.Add((int)Vector3.Distance(node.vectorPos, node2.vectorPos));
					arrayList3.Add(node);
					node2.neighbours = arrayList3.ToArray(typeof(Node)) as Node[];
					node2.neighboursKeys = arrayList4.ToArray(typeof(int)) as int[];
					node2.angles = arrayList2.ToArray(typeof(float)) as float[];
					node2.costs = arrayList.ToArray(typeof(int)) as int[];
				}
			}
		}
	}

	public void GenerateNavmesh(Vector3[] points)
	{
		Bounds bounds = default(Bounds);
		for (int i = 0; i < points.Length; i++)
		{
			bounds.Encapsulate(points[i]);
		}
		bounds.extents += Vector3.one * boundsMargin;
		AstarClasses.Grid grid = new AstarClasses.Grid(20f);
		grids = new AstarClasses.Grid[1] { grid };
		grid.width = points.Length;
		grid.depth = 1;
		grid.globalWidth = Mathf.CeilToInt(bounds.size.x);
		grid.globalDepth = Mathf.CeilToInt(bounds.size.z);
		grid.globalHeight = Mathf.CeilToInt(bounds.size.y);
		grid.nodeSize = 1f;
		grid.offset = bounds.center - bounds.extents;
		staticNodes = new Node[1][,];
		staticNodes[0] = new Node[grid.width, grid.depth];
		totalNodeAmount = grid.depth * grid.width;
		for (int j = 0; j < grid.width; j++)
		{
			Node node = new Node();
			node.pos = new Int3(j, 0, 0);
			node.vectorPos = points[j];
			node.angles = new float[9];
			staticNodes[0][j, 0] = node;
		}
		for (int k = 0; k < grid.width; k++)
		{
			Node node2 = staticNodes[0][k, 0];
			for (int l = 0; l < grid.width; l++)
			{
				Node node3 = staticNodes[0][l, 0];
				if (node3.vectorPos == node2.vectorPos)
				{
				}
			}
		}
		ApplyEnablerLinks();
		for (int m = 0; m < grid.width; m++)
		{
			Node node4 = staticNodes[0][m, 0];
			if (!node4.walkable)
			{
				continue;
			}
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			ArrayList arrayList4 = new ArrayList();
			for (int n = 0; n < grid.width; n++)
			{
				Node node5 = staticNodes[0][n, 0];
				RaycastHit hitInfo;
				if (!(node5.vectorPos == node4.vectorPos) && !(Mathf.Abs(node4.vectorPos.y - node5.vectorPos.y) > yLimit) && node5.walkable && node5 != node4 && !Physics.Linecast(node4.vectorPos, node5.vectorPos, out hitInfo, boundsRayHitMask) && !Physics.Linecast(node5.vectorPos, node4.vectorPos, out hitInfo, boundsRayHitMask))
				{
					float sqrMagnitude = (node4.vectorPos - node5.vectorPos).sqrMagnitude;
					if (sqrMagnitude <= neighbourDistanceLimit * neighbourDistanceLimit)
					{
						arrayList2.Add(node5);
						arrayList3.Add(Mathf.RoundToInt(Mathf.Sqrt(sqrMagnitude) * 100f));
						arrayList.Add(arrayList.Count);
						arrayList4.Add(0);
					}
				}
			}
			node4.neighbours = arrayList2.ToArray(typeof(Node)) as Node[];
			node4.neighboursKeys = arrayList.ToArray(typeof(int)) as int[];
			node4.costs = arrayList3.ToArray(typeof(int)) as int[];
			node4.angles = arrayList4.ToArray(typeof(float)) as float[];
		}
		ApplyLinks();
		FloodFillAll();
	}

	public static Vector3[] BoundPoints(Bounds b)
	{
		Vector3[] array = new Vector3[4];
		array[0] = new Vector3(b.extents.x, 0f, b.extents.z) + b.center;
		array[1] = new Vector3(0f - b.extents.x, 0f, 0f - b.extents.z) + b.center;
		array[2] = new Vector3(b.extents.x, 0f, 0f - b.extents.z) + b.center;
		array[2] = new Vector3(0f - b.extents.x, 0f, b.extents.z) + b.center;
		return array;
	}

	public void SendBugReport(string email, string message)
	{
		StartCoroutine(SendBugReport2(email, message));
	}

	public IEnumerator SendBugReport2(string email, string message)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("email", email);
		wWWForm.AddField("message", message);
		WWW w = new WWW("http://arongranberg.com/wp-content/uploads/astarpathfinding/bugreport.php", wWWForm);
		yield return w;
		if (w.error != null)
		{
			Debug.LogError("Error: " + w.error);
		}
		else
		{
			Debug.Log("Bug report sent");
		}
	}
}
