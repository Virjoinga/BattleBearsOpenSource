using System;
using UnityEngine;

[Serializable]
public class LevelCell
{
	public Rect rect;

	public RoomTile roomTile;

	[NonSerialized]
	private bool dragging;

	public string Name
	{
		get
		{
			if (roomTile == null)
			{
				return "NULL";
			}
			return roomTile.name;
		}
	}

	public Vector2 Position
	{
		get
		{
			return new Vector2(rect.x, rect.y);
		}
	}

	public Vector2 TilePosition
	{
		get
		{
			return new Vector2(rect.x / 100f, rect.y / 100f);
		}
	}

	public bool Dragging
	{
		get
		{
			return dragging;
		}
		set
		{
			dragging = value;
		}
	}

	public void SnapToGrid(Vector2 mPos)
	{
		if (mPos.x >= 0f)
		{
			rect.x = (int)(mPos.x / 100f) * 100;
		}
		else
		{
			rect.x = ((int)(mPos.x / 100f) - 1) * 100;
		}
		if (mPos.y >= 0f)
		{
			rect.y = (int)(mPos.y / 100f) * 100;
		}
		else
		{
			rect.y = ((int)(mPos.y / 100f) - 1) * 100;
		}
	}

	public LevelCell(RoomTile tile)
	{
		rect = new Rect(0f, 0f, 100f, 100f);
		roomTile = tile;
	}
}
