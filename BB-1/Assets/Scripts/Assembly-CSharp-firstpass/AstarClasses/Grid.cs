using System;
using UnityEngine;

namespace AstarClasses
{
	[Serializable]
	public class Grid
	{
		public string name = "New Grid";

		public bool showInEditor = true;

		public bool debug = true;

		public bool changed;

		public float _height = 10f;

		public int _width = 10;

		public int _depth = 10;

		public float scale = 1f;

		public Vector3 offset = Vector3.zero;

		public Vector3 globalOffset = Vector3.zero;

		public float nodeSize = 10f;

		public float globalWidth = 100f;

		public float globalDepth = 100f;

		public float globalHeight = 50f;

		public bool showPhysics;

		public int ignoreLayer;

		public LayerMask physicsMask = -1;

		public PhysicsType physicsType = PhysicsType.TouchCapsule;

		public UpDown raycastUpDown = UpDown.Down;

		public float raycastLength = 1000f;

		public float capsuleHeight = 20f;

		public float physicsRadius = 1f;

		public float height
		{
			get
			{
				return _height;
			}
			set
			{
				_height = value;
				globalHeight = value;
			}
		}

		public int width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
				globalWidth = value;
			}
		}

		public int depth
		{
			get
			{
				return _depth;
			}
			set
			{
				_depth = value;
				globalDepth = value;
			}
		}

		public Vector3 realOffset
		{
			get
			{
				return offset + globalOffset;
			}
		}

		public Grid(float h)
		{
			height = h;
			width = 100;
			depth = 100;
			globalWidth = 100f;
			globalDepth = 100f;
			globalHeight = h;
		}

		public Grid()
		{
			height = 10f;
			width = 15;
			depth = 15;
			globalWidth = 15f;
			globalDepth = 15f;
			globalHeight = 10f;
			nodeSize = 1f;
			offset = Vector3.zero;
			globalOffset = offset;
		}

		public Grid(Grid o)
		{
			height = o.height;
			width = o.width;
			depth = o.depth;
			offset = o.offset;
			nodeSize = o.nodeSize;
		}

		public bool Contains(Int3 p)
		{
			if ((float)p.x >= realOffset.x && (float)p.z >= realOffset.z && (float)p.x < realOffset.x + globalWidth * nodeSize && (float)p.z < realOffset.z + globalDepth * nodeSize && (float)p.y >= realOffset.y && (float)p.y < realOffset.y + globalHeight)
			{
				return true;
			}
			return false;
		}

		public bool Contains(Vector3 p)
		{
			if (p.x >= realOffset.x && p.z >= realOffset.z && p.x < realOffset.x + globalWidth * nodeSize && p.z < realOffset.z + globalDepth * nodeSize && p.y >= realOffset.y && p.y < realOffset.y + globalHeight)
			{
				return true;
			}
			return false;
		}

		public void Reset()
		{
			globalOffset = Vector3.zero;
		}
	}
}
