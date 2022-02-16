using System;
using UnityEngine;

[Serializable]
public class Level : ScriptableObject
{
	public LevelCell[] cells = new LevelCell[0];

	public int startingCell;

	public Rect GetBounds
	{
		get
		{
			Rect result = new Rect(0f, 0f, 10f, 10f);
			for (int i = 0; i < cells.Length; i++)
			{
				LevelCell levelCell = cells[i];
				if (levelCell.TilePosition.x <= result.xMin)
				{
					result.xMin = levelCell.TilePosition.x - 1f;
				}
				if (levelCell.TilePosition.x >= result.xMax - 1f)
				{
					result.xMax = levelCell.TilePosition.x + 2f;
				}
				if (levelCell.TilePosition.y <= result.yMin)
				{
					result.yMin = levelCell.TilePosition.y - 1f;
				}
				if (levelCell.TilePosition.y >= result.yMax - 1f)
				{
					result.yMax = levelCell.TilePosition.y + 2f;
				}
			}
			return result;
		}
	}

	public LevelCell GetStartingRoom()
	{
		if (cells.Length == 0)
		{
			return null;
		}
		return cells[startingCell];
	}

	public LevelCell GetNeighbourCell(LevelCell cell, ExitDirection dir)
	{
		Vector2 tilePosition = cell.TilePosition;
		switch (dir)
		{
		case ExitDirection.NORTH:
			tilePosition.y += 1f;
			break;
		case ExitDirection.SOUTH:
			tilePosition.y -= 1f;
			break;
		case ExitDirection.EAST:
			tilePosition.x += 1f;
			break;
		case ExitDirection.WEST:
			tilePosition.x -= 1f;
			break;
		}
		LevelCell[] array = cells;
		foreach (LevelCell levelCell in array)
		{
			if (levelCell.TilePosition == tilePosition)
			{
				return levelCell;
			}
		}
		return null;
	}

	public LevelCell GetCell(Vector2 cellPos)
	{
		LevelCell[] array = cells;
		foreach (LevelCell levelCell in array)
		{
			if (levelCell.TilePosition == cellPos)
			{
				return levelCell;
			}
		}
		return null;
	}

	public void AddTile(LevelCell cell)
	{
		if (cells == null)
		{
			cells = new LevelCell[0];
		}
		LevelCell[] array = new LevelCell[cells.Length + 1];
		cells.CopyTo(array, 0);
		cells = array;
		cells[cells.Length - 1] = cell;
	}

	public bool RemoveTile(LevelCell cell)
	{
		if (cells == null)
		{
			cells = new LevelCell[0];
		}
		int i;
		for (i = 0; i < cells.Length && cells[i] != cell; i++)
		{
		}
		if (i != cells.Length)
		{
			LevelCell[] array = new LevelCell[cells.Length - 1];
			int num = 0;
			for (int j = 0; j < cells.Length; j++)
			{
				if (j != i)
				{
					array[num++] = cells[j];
				}
			}
			cells = array;
			startingCell = 0;
			return true;
		}
		return false;
	}

	public void ClearTiles()
	{
		cells = new LevelCell[0];
	}
}
