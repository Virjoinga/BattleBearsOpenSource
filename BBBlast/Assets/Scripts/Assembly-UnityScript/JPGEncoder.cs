using System;
using System.Threading;
using UnityEngine;

[Serializable]
public class JPGEncoder
{
	private int[] ZigZag;

	private int[] YTable;

	private int[] UVTable;

	private float[] fdtbl_Y;

	private float[] fdtbl_UV;

	private BitString[] YDC_HT;

	private BitString[] UVDC_HT;

	private BitString[] YAC_HT;

	private BitString[] UVAC_HT;

	private int[] std_dc_luminance_nrcodes;

	private int[] std_dc_luminance_values;

	private int[] std_ac_luminance_nrcodes;

	private int[] std_ac_luminance_values;

	private int[] std_dc_chrominance_nrcodes;

	private int[] std_dc_chrominance_values;

	private int[] std_ac_chrominance_nrcodes;

	private int[] std_ac_chrominance_values;

	private BitString[] bitcode;

	private int[] category;

	private int bytenew;

	private int bytepos;

	private ByteArray byteout;

	private int[] DU;

	private float[] YDU;

	private float[] UDU;

	private float[] VDU;

	public bool isDone;

	private BitmapData image;

	private int sf;

	public JPGEncoder(Texture2D texture, float quality)
	{
		ZigZag = new int[64]
		{
			0, 1, 5, 6, 14, 15, 27, 28, 2, 4,
			7, 13, 16, 26, 29, 42, 3, 8, 12, 17,
			25, 30, 41, 43, 9, 11, 18, 24, 31, 40,
			44, 53, 10, 19, 23, 32, 39, 45, 52, 54,
			20, 22, 33, 38, 46, 51, 55, 60, 21, 34,
			37, 47, 50, 56, 59, 61, 35, 36, 48, 49,
			57, 58, 62, 63
		};
		YTable = new int[64];
		UVTable = new int[64];
		fdtbl_Y = new float[64];
		fdtbl_UV = new float[64];
		std_dc_luminance_nrcodes = new int[17]
		{
			0, 0, 1, 5, 1, 1, 1, 1, 1, 1,
			0, 0, 0, 0, 0, 0, 0
		};
		std_dc_luminance_values = new int[12]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11
		};
		std_ac_luminance_nrcodes = new int[17]
		{
			0, 0, 2, 1, 3, 3, 2, 4, 3, 5,
			5, 4, 4, 0, 0, 1, 125
		};
		std_ac_luminance_values = new int[162]
		{
			1, 2, 3, 0, 4, 17, 5, 18, 33, 49,
			65, 6, 19, 81, 97, 7, 34, 113, 20, 50,
			129, 145, 161, 8, 35, 66, 177, 193, 21, 82,
			209, 240, 36, 51, 98, 114, 130, 9, 10, 22,
			23, 24, 25, 26, 37, 38, 39, 40, 41, 42,
			52, 53, 54, 55, 56, 57, 58, 67, 68, 69,
			70, 71, 72, 73, 74, 83, 84, 85, 86, 87,
			88, 89, 90, 99, 100, 101, 102, 103, 104, 105,
			106, 115, 116, 117, 118, 119, 120, 121, 122, 131,
			132, 133, 134, 135, 136, 137, 138, 146, 147, 148,
			149, 150, 151, 152, 153, 154, 162, 163, 164, 165,
			166, 167, 168, 169, 170, 178, 179, 180, 181, 182,
			183, 184, 185, 186, 194, 195, 196, 197, 198, 199,
			200, 201, 202, 210, 211, 212, 213, 214, 215, 216,
			217, 218, 225, 226, 227, 228, 229, 230, 231, 232,
			233, 234, 241, 242, 243, 244, 245, 246, 247, 248,
			249, 250
		};
		std_dc_chrominance_nrcodes = new int[17]
		{
			0, 0, 3, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 0, 0, 0, 0, 0
		};
		std_dc_chrominance_values = new int[12]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11
		};
		std_ac_chrominance_nrcodes = new int[17]
		{
			0, 0, 2, 1, 2, 4, 4, 3, 4, 7,
			5, 4, 4, 0, 1, 2, 119
		};
		std_ac_chrominance_values = new int[162]
		{
			0, 1, 2, 3, 17, 4, 5, 33, 49, 6,
			18, 65, 81, 7, 97, 113, 19, 34, 50, 129,
			8, 20, 66, 145, 161, 177, 193, 9, 35, 51,
			82, 240, 21, 98, 114, 209, 10, 22, 36, 52,
			225, 37, 241, 23, 24, 25, 26, 38, 39, 40,
			41, 42, 53, 54, 55, 56, 57, 58, 67, 68,
			69, 70, 71, 72, 73, 74, 83, 84, 85, 86,
			87, 88, 89, 90, 99, 100, 101, 102, 103, 104,
			105, 106, 115, 116, 117, 118, 119, 120, 121, 122,
			130, 131, 132, 133, 134, 135, 136, 137, 138, 146,
			147, 148, 149, 150, 151, 152, 153, 154, 162, 163,
			164, 165, 166, 167, 168, 169, 170, 178, 179, 180,
			181, 182, 183, 184, 185, 186, 194, 195, 196, 197,
			198, 199, 200, 201, 202, 210, 211, 212, 213, 214,
			215, 216, 217, 218, 226, 227, 228, 229, 230, 231,
			232, 233, 234, 242, 243, 244, 245, 246, 247, 248,
			249, 250
		};
		bitcode = new BitString[65535];
		category = new int[65535];
		bytepos = 7;
		byteout = new ByteArray();
		DU = new int[64];
		YDU = new float[64];
		UDU = new float[64];
		VDU = new float[64];
		image = new BitmapData(texture);
		if (!(quality > 0f))
		{
			quality = 1f;
		}
		if (!(quality <= 100f))
		{
			quality = 100f;
		}
		checked
		{
			if (!(quality >= 50f))
			{
				sf = (int)(5000f / quality);
			}
			else
			{
				sf = (int)(200f - quality * 2f);
			}
			Thread thread = new Thread(doEncoding);
			thread.Start();
		}
	}

	private void initQuantTables(int sf)
	{
		int num = default(int);
		float num2 = default(float);
		int[] array = new int[64]
		{
			16, 11, 10, 16, 24, 40, 51, 61, 12, 12,
			14, 19, 26, 58, 60, 55, 14, 13, 16, 24,
			40, 57, 69, 56, 14, 17, 22, 29, 51, 87,
			80, 62, 18, 22, 37, 56, 68, 109, 103, 77,
			24, 35, 55, 64, 81, 104, 113, 92, 49, 64,
			78, 87, 103, 121, 120, 101, 72, 92, 95, 98,
			112, 100, 103, 99
		};
		checked
		{
			for (num = 0; num < 64; num++)
			{
				unchecked
				{
					num2 = Mathf.Floor(checked(array[num] * sf + 50) / 100);
					if (!(num2 >= 1f))
					{
						num2 = 1f;
					}
					else if (!(num2 <= 255f))
					{
						num2 = 255f;
					}
				}
				YTable[ZigZag[num]] = (int)num2;
			}
			int[] array2 = new int[64]
			{
				17, 18, 24, 47, 99, 99, 99, 99, 18, 21,
				26, 66, 99, 99, 99, 99, 24, 26, 56, 99,
				99, 99, 99, 99, 47, 66, 99, 99, 99, 99,
				99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
				99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
				99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
				99, 99, 99, 99
			};
			for (num = 0; num < 64; num++)
			{
				unchecked
				{
					num2 = Mathf.Floor(checked(array2[num] * sf + 50) / 100);
					if (!(num2 >= 1f))
					{
						num2 = 1f;
					}
					else if (!(num2 <= 255f))
					{
						num2 = 255f;
					}
				}
				UVTable[ZigZag[num]] = (int)num2;
			}
			float[] array3 = new float[8] { 1f, 1.3870399f, 1.306563f, 1.1758755f, 1f, 0.78569496f, 0.5411961f, 0.27589938f };
			num = 0;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					fdtbl_Y[num] = 1f / ((float)YTable[ZigZag[num]] * array3[i] * array3[j] * 8f);
					fdtbl_UV[num] = 1f / ((float)UVTable[ZigZag[num]] * array3[i] * array3[j] * 8f);
					num++;
				}
			}
		}
	}

	private BitString[] computeHuffmanTbl(int[] nrcodes, int[] std_table)
	{
		int num = 0;
		int num2 = 0;
		BitString[] array = new BitString[256];
		checked
		{
			for (int i = 1; i <= 16; i++)
			{
				for (int j = 1; j <= nrcodes[i]; j++)
				{
					array[std_table[num2]] = new BitString();
					array[std_table[num2]].val = num;
					array[std_table[num2]].len = i;
					num2++;
					num++;
				}
				num *= 2;
			}
			return array;
		}
	}

	private void initHuffmanTbl()
	{
		YDC_HT = computeHuffmanTbl(std_dc_luminance_nrcodes, std_dc_luminance_values);
		UVDC_HT = computeHuffmanTbl(std_dc_chrominance_nrcodes, std_dc_chrominance_values);
		YAC_HT = computeHuffmanTbl(std_ac_luminance_nrcodes, std_ac_luminance_values);
		UVAC_HT = computeHuffmanTbl(std_ac_chrominance_nrcodes, std_ac_chrominance_values);
	}

	private void initCategoryfloat()
	{
		int num = 1;
		int num2 = 2;
		int num3 = default(int);
		BitString bitString = null;
		checked
		{
			for (int i = 1; i <= 15; i++)
			{
				for (num3 = num; num3 < num2; num3++)
				{
					category[32767 + num3] = i;
					bitString = new BitString();
					bitString.len = i;
					bitString.val = num3;
					bitcode[32767 + num3] = bitString;
				}
				for (num3 = -(num2 - 1); num3 <= -num; num3++)
				{
					category[32767 + num3] = i;
					bitString = new BitString();
					bitString.len = i;
					bitString.val = num2 - 1 + num3;
					bitcode[32767 + num3] = bitString;
				}
				num <<= 1;
				num2 <<= 1;
			}
		}
	}

	public virtual byte[] GetBytes()
	{
		return (!isDone) ? null : byteout.GetAllBytes();
	}

	private void writeBits(BitString bs)
	{
		int val = bs.val;
		checked
		{
			int num = bs.len - 1;
			while (num >= 0)
			{
				if (unchecked((int)checked((long)val & (long)unchecked((int)Convert.ToUInt32(1 << num)))) != 0)
				{
					bytenew = (int)((long)bytenew | (long)unchecked((int)Convert.ToUInt32(1 << bytepos)));
				}
				num--;
				bytepos--;
				if (bytepos < 0)
				{
					if (bytenew == 255)
					{
						writeByte(byte.MaxValue);
						writeByte(0);
					}
					else
					{
						writeByte((byte)bytenew);
					}
					bytepos = 7;
					bytenew = 0;
				}
			}
		}
	}

	private void writeByte(byte value)
	{
		byteout.writeByte(value);
	}

	private void writeWord(int value)
	{
		checked
		{
			writeByte((byte)((value >> 8) & 0xFF));
			writeByte((byte)(value & 0xFF));
		}
	}

	private float[] fDCTQuant(float[] data, float[] fdtbl)
	{
		float num = default(float);
		float num2 = default(float);
		float num3 = default(float);
		float num4 = default(float);
		float num5 = default(float);
		float num6 = default(float);
		float num7 = default(float);
		float num8 = default(float);
		float num9 = default(float);
		float num10 = default(float);
		float num11 = default(float);
		float num12 = default(float);
		float num13 = default(float);
		float num14 = default(float);
		float num15 = default(float);
		float num16 = default(float);
		float num17 = default(float);
		float num18 = default(float);
		float num19 = default(float);
		int num20 = default(int);
		int num21 = 0;
		checked
		{
			for (num20 = 0; num20 < 8; num20++)
			{
				num = data[num21 + 0] + data[num21 + 7];
				num8 = data[num21 + 0] - data[num21 + 7];
				num2 = data[num21 + 1] + data[num21 + 6];
				num7 = data[num21 + 1] - data[num21 + 6];
				num3 = data[num21 + 2] + data[num21 + 5];
				num6 = data[num21 + 2] - data[num21 + 5];
				num4 = data[num21 + 3] + data[num21 + 4];
				num5 = data[num21 + 3] - data[num21 + 4];
				num9 = num + num4;
				num12 = num - num4;
				num10 = num2 + num3;
				num11 = num2 - num3;
				data[num21 + 0] = num9 + num10;
				data[num21 + 4] = num9 - num10;
				num13 = (num11 + num12) * 0.70710677f;
				data[num21 + 2] = num12 + num13;
				data[num21 + 6] = num12 - num13;
				num9 = num5 + num6;
				num10 = num6 + num7;
				num11 = num7 + num8;
				num17 = (num9 - num11) * 0.38268343f;
				num14 = 0.5411961f * num9 + num17;
				num16 = 1.306563f * num11 + num17;
				num15 = num10 * 0.70710677f;
				num18 = num8 + num15;
				num19 = num8 - num15;
				data[num21 + 5] = num19 + num14;
				data[num21 + 3] = num19 - num14;
				data[num21 + 1] = num18 + num16;
				data[num21 + 7] = num18 - num16;
				num21 += 8;
			}
			num21 = 0;
			for (num20 = 0; num20 < 8; num20++)
			{
				num = data[num21 + 0] + data[num21 + 56];
				num8 = data[num21 + 0] - data[num21 + 56];
				num2 = data[num21 + 8] + data[num21 + 48];
				num7 = data[num21 + 8] - data[num21 + 48];
				num3 = data[num21 + 16] + data[num21 + 40];
				num6 = data[num21 + 16] - data[num21 + 40];
				num4 = data[num21 + 24] + data[num21 + 32];
				num5 = data[num21 + 24] - data[num21 + 32];
				num9 = num + num4;
				num12 = num - num4;
				num10 = num2 + num3;
				num11 = num2 - num3;
				data[num21 + 0] = num9 + num10;
				data[num21 + 32] = num9 - num10;
				num13 = (num11 + num12) * 0.70710677f;
				data[num21 + 16] = num12 + num13;
				data[num21 + 48] = num12 - num13;
				num9 = num5 + num6;
				num10 = num6 + num7;
				num11 = num7 + num8;
				num17 = (num9 - num11) * 0.38268343f;
				num14 = 0.5411961f * num9 + num17;
				num16 = 1.306563f * num11 + num17;
				num15 = num10 * 0.70710677f;
				num18 = num8 + num15;
				num19 = num8 - num15;
				data[num21 + 40] = num19 + num14;
				data[num21 + 24] = num19 - num14;
				data[num21 + 8] = num18 + num16;
				data[num21 + 56] = num18 - num16;
				num21++;
			}
			for (num20 = 0; num20 < 64; num20++)
			{
				data[num20] = Mathf.Round(data[num20] * fdtbl[num20]);
			}
			return data;
		}
	}

	private void writeAPP0()
	{
		writeWord(65504);
		writeWord(16);
		writeByte(74);
		writeByte(70);
		writeByte(73);
		writeByte(70);
		writeByte(0);
		writeByte(1);
		writeByte(1);
		writeByte(0);
		writeWord(1);
		writeWord(1);
		writeByte(0);
		writeByte(0);
	}

	private void writeSOF0(int width, int height)
	{
		writeWord(65472);
		writeWord(17);
		writeByte(8);
		writeWord(height);
		writeWord(width);
		writeByte(3);
		writeByte(1);
		writeByte(17);
		writeByte(0);
		writeByte(2);
		writeByte(17);
		writeByte(1);
		writeByte(3);
		writeByte(17);
		writeByte(1);
	}

	private void writeDQT()
	{
		writeWord(65499);
		writeWord(132);
		writeByte(0);
		int num = default(int);
		checked
		{
			for (num = 0; num < 64; num++)
			{
				writeByte((byte)YTable[num]);
			}
			writeByte(1);
			for (num = 0; num < 64; num++)
			{
				writeByte((byte)UVTable[num]);
			}
		}
	}

	private void writeDHT()
	{
		writeWord(65476);
		writeWord(418);
		int num = default(int);
		writeByte(0);
		checked
		{
			for (num = 0; num < 16; num++)
			{
				writeByte((byte)std_dc_luminance_nrcodes[num + 1]);
			}
			for (num = 0; num <= 11; num++)
			{
				writeByte((byte)std_dc_luminance_values[num]);
			}
			writeByte(16);
			for (num = 0; num < 16; num++)
			{
				writeByte((byte)std_ac_luminance_nrcodes[num + 1]);
			}
			for (num = 0; num <= 161; num++)
			{
				writeByte((byte)std_ac_luminance_values[num]);
			}
			writeByte(1);
			for (num = 0; num < 16; num++)
			{
				writeByte((byte)std_dc_chrominance_nrcodes[num + 1]);
			}
			for (num = 0; num <= 11; num++)
			{
				writeByte((byte)std_dc_chrominance_values[num]);
			}
			writeByte(17);
			for (num = 0; num < 16; num++)
			{
				writeByte((byte)std_ac_chrominance_nrcodes[num + 1]);
			}
			for (num = 0; num <= 161; num++)
			{
				writeByte((byte)std_ac_chrominance_values[num]);
			}
		}
	}

	private void writeSOS()
	{
		writeWord(65498);
		writeWord(12);
		writeByte(3);
		writeByte(1);
		writeByte(0);
		writeByte(2);
		writeByte(17);
		writeByte(3);
		writeByte(17);
		writeByte(0);
		writeByte(63);
		writeByte(0);
	}

	private float processDU(float[] CDU, float[] fdtbl, float DC, BitString[] HTDC, BitString[] HTAC)
	{
		BitString bs = HTAC[0];
		BitString bs2 = HTAC[240];
		int num = default(int);
		float[] array = fDCTQuant(CDU, fdtbl);
		checked
		{
			for (num = 0; num < 64; num++)
			{
				DU[ZigZag[num]] = (int)array[num];
			}
			int num2 = (int)((float)DU[0] - DC);
			DC = DU[0];
			if (num2 == 0)
			{
				writeBits(HTDC[0]);
			}
			else
			{
				writeBits(HTDC[category[32767 + num2]]);
				writeBits(bitcode[32767 + num2]);
			}
			int num3 = 63;
			while (num3 > 0 && DU[num3] == 0)
			{
				num3--;
			}
			float result;
			if (num3 == 0)
			{
				writeBits(bs);
				result = DC;
			}
			else
			{
				for (num = 1; num <= num3; num++)
				{
					int num4 = num;
					for (; DU[num] == 0 && num <= num3; num++)
					{
					}
					int num5 = num - num4;
					if (num5 >= 16)
					{
						for (int i = 1; i <= unchecked(num5 / 16); i++)
						{
							writeBits(bs2);
						}
						num5 &= 0xF;
					}
					writeBits(HTAC[num5 * 16 + category[32767 + DU[num]]]);
					writeBits(bitcode[32767 + DU[num]]);
				}
				if (num3 != 63)
				{
					writeBits(bs);
				}
				result = DC;
			}
			return result;
		}
	}

	private void RGB2YUV(BitmapData img, int xpos, int ypos)
	{
		int num = 0;
		checked
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Color pixelColor = img.getPixelColor(xpos + j, img.height - (ypos + i));
					float num2 = pixelColor.r * 255f;
					float num3 = pixelColor.g * 255f;
					float num4 = pixelColor.b * 255f;
					YDU[num] = 0.299f * num2 + 0.587f * num3 + 0.114f * num4 - 128f;
					UDU[num] = -0.16874f * num2 + -0.33126f * num3 + 0.5f * num4;
					VDU[num] = 0.5f * num2 + -0.41869f * num3 + -0.08131f * num4;
					num++;
				}
			}
		}
	}

	private void doEncoding()
	{
		isDone = false;
		initHuffmanTbl();
		initCategoryfloat();
		initQuantTables(sf);
		encode();
		isDone = true;
		image = null;
		Thread.CurrentThread.Abort();
	}

	private void encode()
	{
		byteout = new ByteArray();
		bytenew = 0;
		bytepos = 7;
		writeWord(65496);
		writeAPP0();
		writeDQT();
		writeSOF0(image.width, image.height);
		writeDHT();
		writeSOS();
		float dC = 0f;
		float dC2 = 0f;
		float dC3 = 0f;
		bytenew = 0;
		bytepos = 7;
		checked
		{
			for (int i = 0; i < image.height; i += 8)
			{
				for (int j = 0; j < image.width; j += 8)
				{
					RGB2YUV(image, j, i);
					dC = processDU(YDU, fdtbl_Y, dC, YDC_HT, YAC_HT);
					dC2 = processDU(UDU, fdtbl_UV, dC2, UVDC_HT, UVAC_HT);
					dC3 = processDU(VDU, fdtbl_UV, dC3, UVDC_HT, UVAC_HT);
					Thread.Sleep(0);
				}
			}
			if (bytepos >= 0)
			{
				BitString bitString = new BitString();
				bitString.len = bytepos + 1;
				bitString.val = (1 << bytepos + 1) - 1;
				writeBits(bitString);
			}
			writeWord(65497);
			isDone = true;
		}
	}

	public virtual void Main()
	{
	}
}
