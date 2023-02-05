using System;
using DefaultNamespace.Cutscene_Runner;
using UnityEngine;

namespace Mahjong
{
	public class MahGame : MonoBehaviour
	{
		public static Action<GameState> OnGameStateChange;
		/// <summary>
		/// When removing a tile, broadcasts the progress (percentage) and total number tiles removed so far.
		/// </summary>
		public static Action<float, int> OnProgressMade;
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

		private void Start()
		{
			//Starting Cutscene?
			_tilePairsRemoved = 0;
			_progress = 0;
			SetGameState(GameState.Gameplay);
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
				AfterTilesRemoved();
				return true;
			}
			else
			{
				return false;
			}
		}

		private void AfterTilesRemoved()
		{
			//calculate percentage of win. should we play a cutscene?
			_progress = _mahjongBoard.GetProgressPercentage();
			//broadcast event of win.
			Debug.Log(_progress);
			//check if win
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