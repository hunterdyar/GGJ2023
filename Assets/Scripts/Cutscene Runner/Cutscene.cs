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
			
			//Activate the HUD gameObject. We Would do a fade in here.
			data.master.SetActive(true);
			float duration = 0;
			
			//play an audio clip
			if (audioClip != null)
			{
				data.AudioSource.clip = audioClip;
				data.AudioSource.Play();
				duration = audioClip.length;
			}
			float audioStartTime = Time.time;
			
			//Set the Primary background image.
			//Set ... other images...?
			
			data.Text.text = cutsceneTempText;
			
			//wait for audio seconds.
			yield return new WaitForSeconds(duration);

			data.master.SetActive(false);

			//Finish playing the clip if needed.
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