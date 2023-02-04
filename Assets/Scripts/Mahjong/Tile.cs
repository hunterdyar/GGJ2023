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

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(Space space, Pattern pattern)
        {
            _pattern = pattern;
            _spriteRenderer.sprite = pattern.Sprite;
            this.Space = space;
            transform.position = space.GetWorldPos();
            _spriteRenderer.sortingOrder = space.pos.z;
        }

        public void Remove()
        {
            Space.ClearTile();
            //todo: Animate into oblivion.
            Destroy(gameObject);
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