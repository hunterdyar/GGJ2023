using System;
using UnityEngine;

namespace Mahjong
{
	public class MahjongInput : MonoBehaviour
	{
		[SerializeField] private MahGame _game;
		private Camera _camera;
		private Tile _currentHoveringTile;
		private Tile _selectedTile;
		private bool _inputEnabled;
		[SerializeField] private AudioClip _matchClip;
		[SerializeField] private AudioClip _errorClip;
		[SerializeField] private AudioClip _selectClip;
		[SerializeField] private AudioSource _source;

		private void Awake()
		{
			_source = GetComponent<AudioSource>();
		}

		private void Start()
		{
			_camera = Camera.main;
		}

		private void OnEnable()
		{
			MahGame.OnGameStateChange += OnGameStateChange;
		}

		private void OnDisable()
		{
			MahGame.OnGameStateChange -= OnGameStateChange;
		}

		private void OnGameStateChange(GameState state)
		{
			_inputEnabled = state == GameState.Gameplay;
		}

		void Update()
		{
			if (_inputEnabled)
			{
				HoverTick();
				SelectTick();
			}
		}

		private void SelectTick()
		{
			if(Input.GetMouseButtonDown(0))
			{
				if (_currentHoveringTile != null)
				{
					//select the same tile again
					if (_selectedTile == _currentHoveringTile)
					{
						//selected and hovering are the same, so both are known not null.
						_selectedTile.SetSelected(false);
						return;
					}
					
					//Select the new tile ("green")
					if (_currentHoveringTile.CanSelect())
					{
						//try matching a tile.
						if (_selectedTile != null)
						{
							TryMatch(_currentHoveringTile, _selectedTile);
						}
						else
						{
							//Select the tile.
							_currentHoveringTile.SetSelected(true);
							_selectedTile = _currentHoveringTile;
							if (_selectClip != null)
							{
								_source.PlayOneShot(_selectClip);
							}
						}
					}
				}
			}
		}

		private void TryMatch(Tile a, Tile b)
		{
			//don't error, which will clear previous.
			if (a == b)
			{
				return;
			}
			
			if (_game.TryMatchTiles(a,b))
			{
				_currentHoveringTile = null;
				_selectedTile = null;
				if (_matchClip != null)
				{
					_source.PlayOneShot(_matchClip);
				}
			}
			else
			{
				//ERR
				_selectedTile.SetSelected(false);
				_selectedTile = null;
				if (_errorClip != null)
				{
					_source.PlayOneShot(_errorClip);
				}
			}
		}

		private void HoverTick()
		{
			//Get if any tile is hovering under the mouse.
			var mouseScreenPos = Input.mousePosition;
			var mouseWorldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
			var col = Physics2D.OverlapPoint(mouseWorldPos);
			if (col != null)
			{
				var tile = col.GetComponent<Tile>();
				if (tile != null)
				{
					SetHovering(tile);
				}
			}
			else
			{
				SetHovering(null);
			}
		}

		private void SetHovering(Tile tile)
		{
			if (tile == null)
			{
				if (_currentHoveringTile != null)
				{
					_currentHoveringTile.SetHover(false);
					_currentHoveringTile = null;
				}
				return;
			}
			//if not null...
			if (_currentHoveringTile == tile)
			{
				return;
			}
			else
			{
				if (_currentHoveringTile != null)
				{
					_currentHoveringTile.SetHover(false);
				}

				_currentHoveringTile = tile;
				tile.SetHover(true);
			}
			
		}
	}
}