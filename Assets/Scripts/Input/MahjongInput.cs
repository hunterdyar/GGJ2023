using System;
using UnityEngine;

namespace Mahjong
{
	public class MahjongInput : MonoBehaviour
	{
		private Camera _camera;
		private Tile _currentHoveringTile;
		private Tile _selectedTile;
		private void Start()
		{
			_camera = Camera.main;
		}

		void Update()
		{
			HoverTick();
			SelectTick();
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
						}
					}
				}
			}
		}

		private void TryMatch(Tile a, Tile b)
		{
			if (a == b)
			{
				return;
			}
			if (a.Pattern.Matches(b.Pattern))
			{
				a.Remove();
				b.Remove();
				_currentHoveringTile = null;
				_selectedTile = null;
			}
			else
			{
				//ERR
				_selectedTile.SetSelected(false);
				_selectedTile = null;
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
		}

		private void SetHovering(Tile tile)
		{
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