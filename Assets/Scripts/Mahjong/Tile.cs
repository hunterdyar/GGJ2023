using System.Collections;
using System.Collections.Generic;
using Mahjong;
using UnityEngine;
using Space = Mahjong.Space;

namespace Mahjong
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private ColorSettings _colorSettings;
        public Space Space;
        public Pattern Pattern => _pattern;
        private Pattern _pattern;
        private SpriteRenderer _patternRenderer;
        private SpriteRenderer _baseTileRenderer;
        private SpriteRenderer _shadowRenderer;
        private bool _isHovering = false;
        private bool selected = false;
        private Color _nonHoverColor = Color.white;
        private RemoveObject _remove;
        public bool IsClueTile => _isClueTile;
        private bool _isClueTile;
        
        private void Awake()
        {
            _remove = GetComponent<RemoveObject>();
            _patternRenderer = (SpriteRenderer)GameObject.Find("Pattern").GetComponent("SpriteRenderer");
            _baseTileRenderer = (SpriteRenderer)GameObject.Find("BaseTile").GetComponent("SpriteRenderer");
            _shadowRenderer = (SpriteRenderer)GameObject.Find("Shadow").GetComponent("SpriteRenderer");
        }

        public void Init(Space space, Pattern pattern, bool isClue = false)
        {
            _isClueTile = isClue;
            _pattern = pattern;
            _patternRenderer.sprite = pattern.TilePattern;
            _baseTileRenderer.sprite = pattern.BaseTile;
            _shadowRenderer.sprite = pattern.Shadow; 
            this.Space = space;
            transform.position = space.GetWorldPos();
            _patternRenderer.sortingOrder = space.pos.z;
            _baseTileRenderer.sortingOrder = space.pos.z-1; 
            _shadowRenderer.sortingOrder = space.pos.z-2; 
            if (isClue)
            {
                gameObject.name = "CUTSCENE TILE";
            }
            else
            {
                gameObject.name = "Tile - " + space.pos.z + " - "+ pattern.name;
            }
        }

        public void Remove()
        {
            Space.ClearTile();
            if (_remove != null)
            {
                _remove.Remove();
            }
            else
            {
                Destroy(gameObject);
            }

        }
        public void SetHover(bool hovering)
        {
            if (hovering != _isHovering)
            {
                _isHovering = hovering;
                if (!_isHovering)
                {
                    _patternRenderer.color = _nonHoverColor;
                }
                else
                {
                    bool selectable = Space.CanBeSelected();
                    Color col = selectable ? _colorSettings.hoverOpenTileTint : _colorSettings.hoverLockedTileTint;
                    _patternRenderer.color = col;
                }
            }
        }

        public void SetSelected(bool sel)
        {
            selected = sel;
            _nonHoverColor = selected ? _colorSettings.selectTileTint : Color.white;
            _patternRenderer.color = _nonHoverColor;
        }

        public bool CanSelect()
        {
            return Space.CanBeSelected();
        }
    }
}