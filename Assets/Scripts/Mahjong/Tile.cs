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
        private Pattern _pattern;
        public void Init(Space space, Pattern pattern)
        {
            GetComponent<SpriteRenderer>().sprite = pattern.Sprite;
            this.Space = space;
            transform.position = space.GetWorldPos();
        }

    }
}