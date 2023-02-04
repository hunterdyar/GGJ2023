using UnityEngine;

namespace Mahjong
{
	[CreateAssetMenu(fileName = "Pattern", menuName = "Mahjong/Pattern", order = 0)]
	public class Pattern : ScriptableObject
	{
		public Sprite Sprite;
	}
}