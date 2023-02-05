using UnityEngine;

namespace Mahjong
{
	[CreateAssetMenu(fileName = "Pattern", menuName = "Mahjong/Pattern", order = 0)]
	public class Pattern : ScriptableObject
	{
		public Sprite TilePattern;
		public Sprite BaseTile;
		public Sprite Shadow;

		public bool Matches(Pattern other)
		{
			//we can compare scriptableObjects fine, but im fixing future bugs :[
			return TilePattern == other.TilePattern;
		}
	}
}