using UnityEngine;

namespace Mahjong
{
	[CreateAssetMenu(fileName = "Color Settings", menuName = "Color Settings", order = 0)]
	public class ColorSettings : ScriptableObject
	{
		public Color selectTileTint;
		public Color hoverLockedTileTint;
		public Color hoverOpenTileTint;
	}
}