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
		

			var right = pos + new Vector3Int(1, 0, 0);
			if (board.TryGetSpace(right, out var rightSpace))
			{
				if (!rightSpace.IsEmpty)
				{
					return false;
				}
			}
			var left = pos + new Vector3Int(-1, 0, 0);
			if (board.TryGetSpace(right, out var leftSpace))
			{
				if (!leftSpace.IsEmpty)
				{
					return false;
				}
			}

			return true;
		}

		public Vector3 GetWorldPos()
		{
			return new Vector3(pos.x, pos.y * 2, pos.z/2f);
		}
	}
}