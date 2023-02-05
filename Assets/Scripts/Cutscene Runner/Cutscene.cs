using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Cutscene_Runner
{
	[CreateAssetMenu(fileName = "Cutscene", menuName = "Cutscene", order = 0)]
	public class Cutscene : ScriptableObject
	{
		[HideInInspector] public bool played;
		
		public int levelToPlayOn;
		public float progressToPlayAfter;
		public string cutsceneTempText;

		public static IEnumerator Routine()
		{
			Debug.Log("Cutscene started");
			yield return new WaitForSeconds(2);
			Debug.Log("Cutscene Ended");
		}
	}
}