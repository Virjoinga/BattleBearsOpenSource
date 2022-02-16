using UnityEngine;

namespace AstarClasses
{
	public class Node
	{
		public Vector3 vectorPos;

		public Int3 pos;

		public Node parentx;

		public Node parent;

		public int invParentDirection = -1;

		public bool walkable = true;

		public int basicCost;

		public int extraCost;

		public int _g;

		public int hx = -1;

		public int penalty;

		public float[] angles = new float[9];

		public Node[] neighbours;

		public int[] neighboursKeys;

		public int area;

		public int areaTimeStamp;

		public AstarPath.Path scripty;

		public int[] costs;

		public AstarPath.Path script;

		public int g;

		public int h;

		public int gScore
		{
			get
			{
				if (parent == null)
				{
					return 0;
				}
				return basicCost + extraCost + penalty + parent.gScore;
			}
		}

		public int f
		{
			get
			{
				return h + g;
			}
		}

		public void UpdateG()
		{
			g = parent.g + basicCost + extraCost + penalty;
		}

		public void UpdateAllG()
		{
			g = parent.g + basicCost + extraCost + penalty;
			Node[] array = neighbours;
			foreach (Node node in array)
			{
				if (node.parent == this)
				{
					node.UpdateAllG(g);
				}
			}
		}

		public void UpdateAllG(int parentG)
		{
			g = parentG + basicCost + extraCost + penalty;
			Node[] array = neighbours;
			foreach (Node node in array)
			{
				if (node.parent == this)
				{
					node.UpdateAllG(g);
				}
			}
		}

		public void UpdateH()
		{
			if (AstarPath.active.useWorldPositions)
			{
				h = (int)(Mathf.Abs(script.end.vectorPos.x - vectorPos.x) * 10f + Mathf.Abs(script.end.vectorPos.y - vectorPos.y) * 10f + Mathf.Abs(script.end.vectorPos.z - vectorPos.z) * 10f);
			}
			else
			{
				h = Mathf.Abs(script.end.pos.x - pos.x) * 10 + Mathf.Abs((int)AstarPath.active.grids[script.end.pos.y].offset.y - (int)AstarPath.active.grids[pos.y].offset.y) * AstarPath.active.levelCost + Mathf.Abs(script.end.pos.z - pos.z) * 10;
			}
		}

		public Node()
		{
			walkable = true;
			costs = null;
		}

		public Node(Node o)
		{
			walkable = o.walkable;
			vectorPos = o.vectorPos;
			pos = o.pos;
			angles = o.angles;
			neighbours = o.neighbours;
		}
	}
}
