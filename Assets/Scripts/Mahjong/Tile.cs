using System.Collections;
using System.Collections.Generic;
using Mahjong;
using UnityEngine;
using Space = Mahjong.Space;

namespace Mahjong
{
    public class Tile : MonoBehaviour
    {
        public Space Space;
        public Pattern Pattern => _pattern;
        private Pattern _pattern;
        private SpriteRenderer _spriteRenderer;
        private bool _isHovering = false;
        private bool selected = false;
        private Color _nonHoverColor = Color.white;
        private RemoveObject _remove;
        public bool IsClueTile => _isClueTile;
        private bool _isClueTile;
        
        private void Awake()
        {
            _remove = GetComponent<RemoveObject>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void Init(Space space, Pattern pattern, bool isCutscene = false)
        {
            _isClueTile = isCutscene;
            _pattern = pattern;
            _spriteRenderer.sprite = pattern.Sprite;
            this.Space = space;
            transform.position = space.GetWorldPos();
            _spriteRenderer.sortingOrder = space.pos.z;
            if (isCutscene)
            {
                gameObject.name = "CUTSCENE TILE";
            }
        }

        public void Remove()
        {
            Space.ClearTile();
            //todo: Animate into oblivion.
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
                    _spriteRenderer.color = _nonHoverColor;
                }
                else
                {
                    bool selectable = Space.CanBeSelected();
                    Color col = selectable ? Color.green : Color.grey;
                    _spriteRenderer.color = col;
                }
            }
        }

        public void SetSelected(bool sel)
        {
            selected = sel;
            _nonHoverColor = selected ? Color.yellow : Color.white;
            _spriteRenderer.color = _nonHoverColor;
        }

        public bool CanSelect()
        {
            return Space.CanBeSelected();
        }
    }
}