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
		
		public static int ClueCutsceneCount;//dont yell at me it's late
		private int _clueCutsceneIndex;
		
		[Header("Config. All Optional.")] [SerializeField] private Cutscene _openingCutscene;
		[SerializeField] private Cutscene[] _clueCutscenes;
		[SerializeField] private Cutscene _closingCutscene;

		//
		[Header("Scene References")] [SerializeField]
		private CutsceneReferences _data;
		
		private void Awake()
		{
			InitCutscenes();
			
			_data.master.SetActive(false);
		}

		private void InitCutscenes()
		{
			//static refs
			ClueCutsceneCount = _clueCutscenes.Length;
			_clueCutsceneIndex = 0;
			//
			if (_openingCutscene != null)
			{
				_openingCutscene.played = false;
			}

			foreach (var cut in _clueCutscenes)
			{
				cut.played = false;
			}

			if (_closingCutscene != null)
			{
				_closingCutscene.played = false;
			}
		}

		private void OnEnable()
		{
			MahGame.OnClueTileFound += OnClueTileFound;
			MahGame.OnGameStateChange += OnGameStateChange;
			MahGame.OnGameBegin += OnGameBegin;
		}

		private void OnDisable()
		{
			MahGame.OnClueTileFound -= OnClueTileFound;
			MahGame.OnGameStateChange -= OnGameStateChange;
			MahGame.OnGameBegin -= OnGameBegin;

		}

		private void OnGameBegin()
		{
			if (_openingCutscene != null)
			{
				WaitThenPlayCutscene(_openingCutscene, 0);
			}
		}

		private void OnGameStateChange(GameState obj)
		{
			if (obj == GameState.End && _closingCutscene != null)
			{
				WaitThenPlayCutscene(_closingCutscene);
			}
		}

		private void OnClueTileFound()
		{
			if (_clueCutsceneIndex < _clueCutscenes.Length)
			{
				WaitThenPlayCutscene(_clueCutscenes[_clueCutsceneIndex]);
				_clueCutsceneIndex++;
			}
			else
			{
				Debug.LogError("wrong number of cutscene tiles found");
			}
		}

		private void WaitThenPlayCutscene(Cutscene cutscene,float delay = 0.5f)
		{
			OnCutsceneStarted?.Invoke();
			cutscene.played = true;
			StartCoroutine(DoWaitThenPlayCutscene(delay,cutscene));
		}

		private IEnumerator DoWaitThenPlayCutscene(float delay,Cutscene scene)
		{
			//Let animations finish.
			yield return new WaitForSeconds(delay);
			//load or enable objects
			yield return StartCoroutine(scene.Routine(_data,this));
			OnCutsceneEnded?.Invoke();
		}
	}
}