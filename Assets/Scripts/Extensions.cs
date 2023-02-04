using System.Collections.Generic;

namespace DefaultNamespace
{
	public static class Extensions
	{
		/// <summary>
		/// Shuffle the list using the Fisher-Yates method.
		/// </summary>
		public static void Shuffle<T>(this IList<T> list)
		{
			System.Random rng = new System.Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				(list[k], list[n]) = (list[n], list[k]);
			}
		}

		public static T RandomItem<T>(this IList<T> list)
		{
			if (list.Count == 0)
			{
				throw new System.IndexOutOfRangeException("Can't select a random item from an empty list");
			}

			return list[UnityEngine.Random.Range(0, list.Count)];
		}

	}
}