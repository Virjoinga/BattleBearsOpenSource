using System;
using System.IO;

[Serializable]
public class ByteArray
{
	private MemoryStream stream;

	private BinaryWriter writer;

	public ByteArray()
	{
		stream = new MemoryStream();
		writer = new BinaryWriter(stream);
	}

	public virtual void writeByte(byte value)
	{
		writer.Write(value);
	}

	public virtual byte[] GetAllBytes()
	{
		byte[] array = new byte[checked((int)stream.Length)];
		stream.Position = 0L;
		stream.Read(array, 0, array.Length);
		return array;
	}
}
