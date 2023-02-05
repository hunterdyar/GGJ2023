using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Cutscene_Runner
{
	[CreateAssetMenu(fileName = "Cutscene", menuName = "Cutscene", order = 0)]
	public class Cutscene : ScriptableObject
	{
		[HideInInspector] public bool played;
		public AudioClip audioClip;
		
		public int levelToPlayOn;
		public float progressToPlayAfter;
		public string cutsceneTempText;

		public IEnumerator Routine(CutsceneReferences data)
		{
			Debug.Log("Cutscene started");
			data.master.SetActive(true);
			float duration = 0;
			if (audioClip != null)
			{
				data.AudioSource.clip = audioClip;
				data.AudioSource.Play();
				duration = audioClip.length;
			}

			float audioStartTime = Time.time;
			
			data.Text.text = cutsceneTempText;
			yield return new WaitForSeconds(2);
			
			
			data.master.SetActive(false);

			//Finish playing the clip as needed.
			if (audioClip != null)
			{
				while (data.AudioSource.isPlaying)
				{
					yield return null;
				}
			}

			Debug.Log("Cutscene Ended");
		}
	}
}