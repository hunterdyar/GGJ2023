using UnityEngine;

namespace Mahjong
{
	[CreateAssetMenu(fileName = "Pattern", menuName = "Mahjong/Pattern", order = 0)]
	public class Pattern : ScriptableObject
	{
		public Sprite Sprite;

		public bool Matches(Pattern other)
		{
			//we can compare scriptableObjects fine, but im fixing future bugs :[
			return Sprite == other.Sprite;
		}
	}
}