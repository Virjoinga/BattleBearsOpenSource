using System;
using UnityEngine;

namespace AstarMath
{
	public class Matrix
	{
		public float[] m;

		public Matrix()
		{
			LoadIdentity();
		}

		public void LoadIdentity()
		{
			m = new float[16];
			for (int i = 0; i < 16; i++)
			{
				m[i] = 0f;
			}
			m[0] = 1f;
			m[5] = 1f;
			m[10] = 1f;
			m[15] = 1f;
		}

		public static Matrix Translate(float _X, float _Y, float _Z)
		{
			Matrix matrix = new Matrix();
			matrix.m[12] = _X;
			matrix.m[13] = _Y;
			matrix.m[14] = _Z;
			return matrix;
		}

		public Matrix translate(float _X, float _Y, float _Z)
		{
			m[12] = _X;
			m[13] = _Y;
			m[14] = _Z;
			return this;
		}

		public static Matrix RotateX(float _Degree)
		{
			Matrix matrix = new Matrix();
			if (_Degree == 0f)
			{
				return matrix;
			}
			float num = Mathf.Cos(_Degree * ((float)Math.PI / 180f));
			float num2 = Mathf.Sin(_Degree * ((float)Math.PI / 180f));
			matrix.m[5] = num;
			matrix.m[6] = num2;
			matrix.m[9] = 0f - num2;
			matrix.m[10] = num;
			return matrix;
		}

		public static Matrix RotateY(float _Degree)
		{
			Matrix matrix = new Matrix();
			if (_Degree == 0f)
			{
				return matrix;
			}
			float num = Mathf.Cos(_Degree * ((float)Math.PI / 180f));
			float num2 = Mathf.Sin(_Degree * ((float)Math.PI / 180f));
			matrix.m[0] = num;
			matrix.m[2] = 0f - num2;
			matrix.m[8] = num2;
			matrix.m[10] = num;
			return matrix;
		}

		public static Matrix RotateZ(float _Degree)
		{
			Matrix matrix = new Matrix();
			if (_Degree == 0f)
			{
				return matrix;
			}
			float num = Mathf.Cos(_Degree * ((float)Math.PI / 180f));
			float num2 = Mathf.Sin(_Degree * ((float)Math.PI / 180f));
			matrix.m[0] = num;
			matrix.m[1] = num2;
			matrix.m[4] = 0f - num2;
			matrix.m[5] = num;
			return matrix;
		}

		public static Matrix Scale(float _In)
		{
			return Scale3D(_In, _In, _In);
		}

		public static Matrix Scale3D(float _X, float _Y, float _Z)
		{
			Matrix matrix = new Matrix();
			matrix.m[0] = _X;
			matrix.m[5] = _Y;
			matrix.m[10] = _Z;
			return matrix;
		}

		public Vector3 TransformVector(Vector3 _V)
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			result.x = _V.x * m[0] + _V.y * m[4] + _V.z * m[8] + m[12];
			result.y = _V.x * m[1] + _V.y * m[5] + _V.z * m[9] + m[13];
			result.z = _V.x * m[2] + _V.y * m[6] + _V.z * m[10] + m[14];
			return result;
		}

		public static Matrix operator *(Matrix _A, Matrix _B)
		{
			Matrix matrix = new Matrix();
			matrix.m[0] = _A.m[0] * _B.m[0] + _A.m[4] * _B.m[1] + _A.m[8] * _B.m[2] + _A.m[12] * _B.m[3];
			matrix.m[4] = _A.m[0] * _B.m[4] + _A.m[4] * _B.m[5] + _A.m[8] * _B.m[6] + _A.m[12] * _B.m[7];
			matrix.m[8] = _A.m[0] * _B.m[8] + _A.m[4] * _B.m[9] + _A.m[8] * _B.m[10] + _A.m[12] * _B.m[11];
			matrix.m[12] = _A.m[0] * _B.m[12] + _A.m[4] * _B.m[13] + _A.m[8] * _B.m[14] + _A.m[12] * _B.m[15];
			matrix.m[1] = _A.m[1] * _B.m[0] + _A.m[5] * _B.m[1] + _A.m[9] * _B.m[2] + _A.m[13] * _B.m[3];
			matrix.m[5] = _A.m[1] * _B.m[4] + _A.m[5] * _B.m[5] + _A.m[9] * _B.m[6] + _A.m[13] * _B.m[7];
			matrix.m[9] = _A.m[1] * _B.m[8] + _A.m[5] * _B.m[9] + _A.m[9] * _B.m[10] + _A.m[13] * _B.m[11];
			matrix.m[13] = _A.m[1] * _B.m[12] + _A.m[5] * _B.m[13] + _A.m[9] * _B.m[14] + _A.m[13] * _B.m[15];
			matrix.m[2] = _A.m[2] * _B.m[0] + _A.m[6] * _B.m[1] + _A.m[10] * _B.m[2] + _A.m[14] * _B.m[3];
			matrix.m[6] = _A.m[2] * _B.m[4] + _A.m[6] * _B.m[5] + _A.m[10] * _B.m[6] + _A.m[14] * _B.m[7];
			matrix.m[10] = _A.m[2] * _B.m[8] + _A.m[6] * _B.m[9] + _A.m[10] * _B.m[10] + _A.m[14] * _B.m[11];
			matrix.m[14] = _A.m[2] * _B.m[12] + _A.m[6] * _B.m[13] + _A.m[10] * _B.m[14] + _A.m[14] * _B.m[15];
			matrix.m[3] = _A.m[3] * _B.m[0] + _A.m[7] * _B.m[1] + _A.m[11] * _B.m[2] + _A.m[15] * _B.m[3];
			matrix.m[7] = _A.m[3] * _B.m[4] + _A.m[7] * _B.m[5] + _A.m[11] * _B.m[6] + _A.m[15] * _B.m[7];
			matrix.m[11] = _A.m[3] * _B.m[8] + _A.m[7] * _B.m[9] + _A.m[11] * _B.m[10] + _A.m[15] * _B.m[11];
			matrix.m[15] = _A.m[3] * _B.m[12] + _A.m[7] * _B.m[13] + _A.m[11] * _B.m[14] + _A.m[15] * _B.m[15];
			return matrix;
		}
	}
}
