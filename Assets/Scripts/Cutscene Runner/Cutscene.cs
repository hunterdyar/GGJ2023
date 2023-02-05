using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

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
		[FormerlySerializedAs("cutsceneTempText")] public string bottomBarText;
		public Cutscene chainCutscene;
		public IEnumerator Routine(CutsceneReferences data, CutsceneRunner context)
		{
			Debug.Log("A coutine routine or subroutine started.");
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
			data.Text.text = bottomBarText;
			int skip = 0;
			float timer = 0;
			float minTime = 2f;
			
			//wait for audio seconds.
			while ((audioClip != null && data.AudioSource.isPlaying) || skip >=2 || timer < minTime)
			{
				//"Update"
				timer += Time.deltaTime;
				skip = skip + (Input.GetMouseButtonDown(0) ? 1 : 0);//check after because we start the cutscene with a click :p
				yield return null;
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