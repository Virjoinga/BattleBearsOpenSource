using UnityEngine;

namespace AstarClasses
{
	public struct Int3
	{
		public int x;

		public int y;

		public int z;

		public Int3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Int3(float x2, float y2)
		{
			x = Mathf.RoundToInt(x2);
			y = 0;
			z = Mathf.RoundToInt(y2);
		}

		public Int3(float x2, float y2, float z2)
		{
			x = Mathf.RoundToInt(x2);
			y = Mathf.RoundToInt(y2);
			z = Mathf.RoundToInt(z2);
		}

		public static Int3 operator +(Int3 lhs, Int3 rhs)
		{
			return new Int3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

		public static Int3 operator -(Int3 lhs, Int3 rhs)
		{
			return new Int3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
		}

		public static bool operator ==(Int3 lhs, Int3 rhs)
		{
			if (lhs.x == rhs.x && lhs.y == rhs.y)
			{
				return lhs.z == rhs.z;
			}
			return false;
		}

		public static bool operator !=(Int3 lhs, Int3 rhs)
		{
			if (lhs.x == rhs.x && lhs.y == rhs.y)
			{
				return lhs.z != rhs.z;
			}
			return true;
		}

		public static implicit operator Int3(Vector3 i)
		{
			return new Int3(i.x, i.y, i.z);
		}

		public static implicit operator Vector3(Int3 i)
		{
			return new Vector3(i.x, i.y, i.z);
		}

		public static implicit operator Vector2(Int3 i)
		{
			return new Vector2(i.x, i.z);
		}

		public override string ToString()
		{
			return "(" + x + "," + y + "," + z + ")";
		}
	}
}
