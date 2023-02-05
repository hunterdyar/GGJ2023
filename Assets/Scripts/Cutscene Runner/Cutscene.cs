using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Cutscene_Runner
{
	[CreateAssetMenu(fileName = "Cutscene", menuName = "Cutscene", order = 0)]
	public class Cutscene : ScriptableObject
	{
		[HideInInspector] public bool played;
		public AudioClip audioClip;
		public Sprite BGimage;
		public int levelToPlayOn;
		public float progressToPlayAfter;
		public string cutsceneTempText;
		public Cutscene chainCutscene;
		public IEnumerator Routine(CutsceneReferences data, CutsceneRunner context)
		{
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
			data.image.sprite = BGimage;
			//Set ... other images...?
			data.Text.text = cutsceneTempText;
			bool skip = false;
			
			
			//wait for audio seconds.
			if (audioClip != null)
			{
				while (data.AudioSource.isPlaying && skip == false)
				{
					skip = Input.GetMouseButtonDown(0);
					yield return null;
				}
			}

			//Go to next cutscene in chain.
			
			if (chainCutscene != null)
			{
				yield return context.StartCoroutine(chainCutscene.Routine(data, context));
			}

			data.master.SetActive(false);
		}
	}
}