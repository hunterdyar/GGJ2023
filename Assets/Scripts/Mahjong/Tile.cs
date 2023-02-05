using System;
using System.Collections;
using System.Collections.Generic;
using Mahjong;
using UnityEngine;
using UnityEngine.Rendering;
using Space = Mahjong.Space;

namespace Mahjong
{
    public class Tile : MonoBehaviour
    {
        public AnimationCurve ease;
        private int z;
        [SerializeField] private ColorSettings _colorSettings;
        public Space Space;
        public Pattern Pattern => _pattern;
        private Pattern _pattern;
        public SpriteRenderer _patternRenderer;
        public SpriteRenderer _baseTileRenderer;
        public SpriteRenderer _shadowRenderer;
        private bool _isHovering = false;
        private bool selected = false;
        private Color _nonHoverColor => CanSelect() ? _colorSettings.regularOpenTileTint : _colorSettings.regularLockedTileTint;
        private bool animatingOut = false;
        public bool IsClueTile => _isClueTile;
        private bool _isClueTile;
        private Coroutine _colorRoutine;

        private void Awake()
        {
            _patternRenderer = transform.Find("Pattern").GetComponent<SpriteRenderer>();
            _baseTileRenderer = transform.Find("Pattern/BaseTile").GetComponent<SpriteRenderer>();
            _shadowRenderer = transform.Find("Pattern/BaseTile/Shadow").GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            MahGame.OnAnyTilesRemoved += UpdateBaseColor;
            MahGame.OnGameBegin += UpdateBaseColor;
        }

        private void OnDisable()
        {
            MahGame.OnAnyTilesRemoved += UpdateBaseColor;
            MahGame.OnGameBegin -= UpdateBaseColor;

        }

        private void UpdateBaseColor()
        {
            Color col = selected ? _colorSettings.selectTileTint : _nonHoverColor;
            //update...
            if (!animatingOut)
            {
                if (_baseTileRenderer.color != col)
                {
                    if (_colorRoutine != null)
                    {
                        StopCoroutine(_colorRoutine);
                    }
                    _colorRoutine= StartCoroutine(LerpBaseToColor(col));
                }
            }
        }

        private IEnumerator LerpBaseToColor(Color end)
        {
            Color start = _baseTileRenderer.color;
            float t = 0;
            while (t < 1)
            {
                _baseTileRenderer.color = Color.Lerp(start, end, t);
                t += Time.deltaTime * 5f;
                yield return null;
            }

            _baseTileRenderer.color = end;
        }

        public void Init(Space space, Pattern pattern, bool isClue = false)
        {
            z = space.pos.z;
            _isClueTile = isClue;
            _pattern = pattern;
            _patternRenderer.sortingOrder = space.pos.z*5+2;
            _baseTileRenderer.sortingOrder = space.pos.z*5+1; 
            _shadowRenderer.sortingOrder = space.pos.z*5; 
            _patternRenderer.sprite = pattern.TilePattern;
            _baseTileRenderer.sprite = pattern.BaseTile;
            _shadowRenderer.sprite = pattern.Shadow; 
            this.Space = space;
            transform.position = space.GetWorldPos();
            
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
            if (_colorRoutine != null)
            {
                StopCoroutine(_colorRoutine);
            }
            Space.ClearTile();
            StartCoroutine(AnimateOut());
            
        }
        public void SetHover(bool hovering)
        {
            if (hovering != _isHovering)
            {
                _isHovering = hovering;
                if (!_isHovering)
                {
                    if (selected)
                    {
                        _baseTileRenderer.color = selected ? _colorSettings.selectTileTint : _nonHoverColor;
                    }
                    else
                    {
                        _baseTileRenderer.color = _nonHoverColor;
                    }

                }
                else
                {
                    bool selectable = Space.CanBeSelected();
                    Color col = selectable ? _colorSettings.hoverOpenTileTint : _colorSettings.hoverLockedTileTint;
                    _baseTileRenderer.color = col;
                }
            }
        }

        public void SetSelected(bool sel)
        {
            selected = sel;
            Color color = selected ? _colorSettings.selectTileTint : _nonHoverColor;
            _baseTileRenderer.color = color;
        }

        public bool CanSelect()
        {
            return Space.CanBeSelected();
        }

        IEnumerator AnimateOut()
        {
            animatingOut = true;

            _patternRenderer.sortingOrder = z * 50 + 2;
            _baseTileRenderer.sortingOrder = z * 50 + 1;
            _shadowRenderer.sortingOrder = z * 50;
            _baseTileRenderer.color = Color.white;
            Vector3 scale = transform.localScale;
            Vector3 scaleEnd = scale * 2;
            Color col = Color.white;
            Color colEnd = new Color(col.r, col.g, col.b, 0);
            float t = 0;
            while (t < 1)
            {
                //opacity
                _baseTileRenderer.color = Color.Lerp(col, colEnd, ease.Evaluate(t));
                _shadowRenderer.color = Color.Lerp(col, colEnd, ease.Evaluate(t));
                _patternRenderer.color = Color.Lerp(col, colEnd, ease.Evaluate(t));
                transform.localScale = Vector3.Lerp(scale, scaleEnd, ease.Evaluate(t));
                
                t += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }

    }
}