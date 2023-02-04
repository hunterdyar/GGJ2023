using UnityEngine;

namespace Mahjong
{
	public class Space
	{
		public MahjongBoard board;
		public Tile tile = null;
		public Vector3Int pos;
		public bool IsEmpty => tile == null;
		public bool CanBeSelected()
		{
			//is there a piece above us?
			var above = pos + new Vector3Int(pos.x, pos.x, pos.z + 1);
			if (board.TryGetSpace(above, out var space))
			{
				if (!space.IsEmpty)
				{
					return false;
				}
			}

			//is either right free or left free
			
			bool rightFree = true;
			var right = pos + new Vector3Int(1, 0, 0);
			if (board.TryGetSpace(right, out var rightSpace))
			{
				if (!rightSpace.IsEmpty)
				{
					rightFree = false;
				}
			}

			bool leftFree = true;

			var left = pos + new Vector3Int(-1, 0, 0);
			if (board.TryGetSpace(left, out var leftSpace))
			{
				if (!leftSpace.IsEmpty)
				{
					leftFree = false;
				}
			}

			return leftFree || rightFree;
		}

		public Vector3 GetWorldPos()
		{
			return new Vector3(pos.x, pos.y * 2, -pos.z/2f);
		}

		public void ClearTile()
		{
			tile = null;
		}
	}
}