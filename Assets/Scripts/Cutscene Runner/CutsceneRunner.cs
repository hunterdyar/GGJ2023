using System;
using System.Collections;
using Mahjong;
using UnityEngine;

namespace DefaultNamespace.Cutscene_Runner
{
	public class CutsceneRunner : MonoBehaviour
	{
		public static Action OnCutsceneStarted;
		public static Action OnCutsceneEnded;
		
		[SerializeField] private Cutscene[] _cutscenes;
		[SerializeField] private string CutsceneScene;
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
			InitCutscenes();
		}

		private void InitCutscenes()
		{
			foreach (var cut in _cutscenes)
			{
				cut.played = false;
			}
		}

		private void OnEnable()
		{
			MahGame.OnProgressMade += OnProgressMade;
		}
		private void OnDisable()
		{
			MahGame.OnProgressMade -= OnProgressMade;
		}
		private void OnProgressMade(float progress, int tilesRemoved)
		{
			foreach (var cutscene in _cutscenes)
			{
				// if(cutscene.levelToPlayOn )//current
				if (cutscene.progressToPlayAfter >= progress)
				{
					PlayCutscene(cutscene);
					return;
				}
			}
		}

		private void PlayCutscene(Cutscene cutscene)
		{
			OnCutsceneStarted?.Invoke();
			cutscene.played = true;
			StartCoroutine(DoPlayCutscene(cutscene));
		}

		private IEnumerator DoPlayCutscene(Cutscene scene)
		{
			//load or enable objects
			
			yield return StartCoroutine(Cutscene.Routine());
			OnCutsceneEnded?.Invoke();
		}
	}
}