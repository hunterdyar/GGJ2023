using System;
using System.Collections;
using DefaultNamespace.Cutscene_Runner;
using UnityEngine;

namespace Mahjong
{
	public class MahGame : MonoBehaviour
	{
		public static Action OnGameBegin;
		public static Action<GameState> OnGameStateChange;
		public static Action OnClueTileFound;
		public static Action OnAnyTilesRemoved;
		public GameState GameState => _gameState;
		private GameState _gameState = GameState.Init;
		private MahjongBoard _mahjongBoard;

		private int _tilePairsRemoved;
		private float _progress = 0;
		
		private void Awake()
		{
			_mahjongBoard = GetComponent<MahjongBoard>();
		}

		private void OnEnable()
		{
			CutsceneRunner.OnCutsceneStarted += CutsceneStarted;
			CutsceneRunner.OnCutsceneEnded += CutsceneEnded;
		}

		private void OnDisable()
		{
			CutsceneRunner.OnCutsceneStarted -= CutsceneStarted;
			CutsceneRunner.OnCutsceneEnded -= CutsceneEnded;
		}
		private void CutsceneStarted()
		{
			SetGameState(GameState.Cutscene);
		}

		private void CutsceneEnded()
		{
			SetGameState(GameState.Gameplay);
		}

		private IEnumerator Start()
		{
			//Starting Cutscene?
			_tilePairsRemoved = 0;
			_progress = 0;
			SetGameState(GameState.Gameplay);
			//hacky wait till all awakes AND starts have been called to invoke event.
			//im SORRY OKAY!?!?!
			yield return null;
			OnGameBegin?.Invoke();
			//0,0
		}

		public bool TryMatchTiles(Tile a, Tile b)
		{
			if (a == b)
			{
				return false;
			}
			if (a.Pattern.Matches(b.Pattern))
			{
				a.Remove();
				b.Remove();
				_tilePairsRemoved++;
				OnAnyTilesRemoved?.Invoke();
				if (a.IsClueTile && b.IsClueTile)
				{
					OnClueTileFound?.Invoke();
				}

				CheckForGameOver();

				return true;
			}
			else
			{
				return false;
			}
		}

		private void CheckForGameOver()
		{
			float progress = _mahjongBoard.GetProgressPercentage();
			if (progress >= 1-Mathf.Epsilon)
			{
				SetGameState(GameState.End);
			}
		}

		public void SetGameState(GameState newState)
		{
			if (_gameState != newState)
			{
				_gameState = newState;
				OnGameStateChange?.Invoke(newState);
			}
		}
	}
}