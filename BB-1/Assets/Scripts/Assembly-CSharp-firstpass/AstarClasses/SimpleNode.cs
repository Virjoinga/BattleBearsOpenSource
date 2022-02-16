using UnityEngine;

namespace AstarClasses
{
	public class SimpleNode
	{
		public Vector3 vectorPos;

		public float[] angles;

		public SimpleNode[] neighbours;

		public int[] costs;

		public Int3 pos;

		public SimpleNode()
		{
		}

		public SimpleNode(Vector3 pos)
		{
			vectorPos = pos;
		}

		public SimpleNode(Vector3 pos, float[] an, SimpleNode[] ne, int[] co)
		{
			vectorPos = pos;
			angles = an;
			neighbours = ne;
			costs = co;
			if (neighbours.Length != costs.Length)
			{
				Debug.LogError("Neighbours and Costs arrays length's must be equal");
			}
		}
	}
}
