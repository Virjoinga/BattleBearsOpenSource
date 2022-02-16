using System;
using UnityEngine;

namespace AstarClasses
{
	[Serializable]
	public class NodeLink
	{
		public Vector3 fromVector;

		public Vector3 toVector;

		public LinkType linkType;

		public bool oneWay;
	}
}
