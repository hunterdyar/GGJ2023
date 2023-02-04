using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Mahjong
{
    public class MahjongBoard : MonoBehaviour
    {
        private Dictionary<Vector3Int, Space> _board;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private List<Pattern> allPatterns;
        
        void Start()
        {
            CreateNewBoard();
        }

        
        public void CreateNewBoard()
        {
            //Create empty board
            _board = new Dictionary<Vector3Int, Space>();
            foreach (var pos in GetLayout())
            {
                var space = new Space
                {
                    pos = pos,
                    tile = null,
                    board = this
                };
                _board.Add(pos,space);
            }
            //Get all "addable" spaces.
            
            //first add 4 tiles to our starting center. 
            var startPat = allPatterns[Random.Range(0, allPatterns.Count)];
            _board.TryGetValue(Vector3Int.zero, out var start1);
            CreateTile(startPat, start1);
            _board.TryGetValue(new Vector3Int(1,0,0), out var start2);
            CreateTile(startPat, start2);
            //first add 4 tiles to our starting center.
            var startPat2 = allPatterns[Random.Range(0, allPatterns.Count)];
            _board.TryGetValue(new Vector3Int(1, 1, 0), out var start3);
            CreateTile(startPat2, start3);
            _board.TryGetValue(new Vector3Int(0, 1, 0), out var start4);
            CreateTile(startPat2, start4);
            //
            
            //While we still have spaces to add tiles to
            int emptyTiles = NumberEmptyTiles();
            int previousEmptyTileCount = 1000000;
            bool escape = false;
            while (NumberEmptyTiles() > 2 && NumberEmptyTiles() != previousEmptyTileCount && !escape)
            {
                previousEmptyTileCount = NumberEmptyTiles();
                //clone the list to a new bag.
                var patternBag = new List<Pattern>(allPatterns);
                patternBag.Shuffle();
                foreach (var pattern in patternBag)
                {
                    var possible = GetAddableSpaces();
                    if (possible.Count >= 2)
                    {
                        var a = possible[Random.Range(0, possible.Count)];
                        var b = a;
                        while (b == a)
                        {
                            b = possible[Random.Range(0, possible.Count)];
                        }

                        CreateTile(pattern, a);//this SHOULD reduce the numberEmptyTiles();
                        CreateTile(pattern, b);
                    }
                    else
                    {
                        // Debug.LogError("no addable spaces?");
                        //done?
                        escape = true;
                    }
                }
            }
            
        }

        private void CreateTile(Pattern pattern, Space space)
        {   
            //instantiate prefab.
            var t = Instantiate(tilePrefab);
            
            //give it the pattern
            t.Init(space,pattern);

            //un-empty the tile.
            space.tile = t;
        }

        public List<Tile> GetAllCurrentTiles()
        {
            return _board.Values.ToList().Where(x => !x.IsEmpty).Select(item => item.tile).ToList();
        }

        public int NumberEmptyTiles()
        {
            return _board.Count(x => x.Value.IsEmpty);
        }

        public List<Space> GetAddableSpaces()
        {
            var tiles = GetAllCurrentTiles();
            var addable = new HashSet<Space>();
            foreach(var tile in tiles)
            {
                var s = tile.Space;
                //we could put pieces on top of the tile, or to the left or right.
                //are there any pieces above us?
               
                var above = s.pos + new Vector3Int(s.pos.x, s.pos.x, s.pos.z + 1);
                if (TryGetSpace(above, out var space))
                {
                    if (space.IsEmpty)
                    {
                        addable.Add(space);
                    }
                }
                //spaces will conflict with each other.

                var right = s.pos + new Vector3Int(1, 0, 0);
                if (TryGetSpace(right, out var rightSpace))
                {
                    if (rightSpace.IsEmpty)
                    {
                        addable.Add(rightSpace);
                    }
                }

                var left = s.pos + new Vector3Int(-1, 0, 0);
                if (TryGetSpace(left, out var leftSpace))
                {
                    if (leftSpace.IsEmpty)
                    {
                        addable.Add(leftSpace);
                    }
                }

                var up = s.pos + new Vector3Int(0, 1, 0);
                if (TryGetSpace(up, out var upSpace))
                {
                    if (upSpace.IsEmpty)
                    {
                        addable.Add(upSpace);
                    }
                }

                var down = s.pos + new Vector3Int(0, -1, 0);
                if (TryGetSpace(down, out var downSpace))
                {
                    if (downSpace.IsEmpty)
                    {
                        addable.Add(downSpace);
                    }
                }
            }

            return addable.ToList();
        }
        public List<Space> GetSpacesCanBeSelected()
        {
            return _board.Values.Where(x => x.CanBeSelected()).ToList();
        }
        private void ClearExisting()
        {
            //lets just reload the scene.
            if (_board != null)
            {
                //loop through and destroy the objects.
                _board.Clear();
            }
        }

        //board layouts?
        protected virtual List<Vector3Int> GetLayout()
        {
            List<Vector3Int> layout = new List<Vector3Int>();
            
            //Generate the middle block
            for (int i = -3; i <= 4; i++)
            {
                for (int j = -4; j <= 3; j++)
                {
                    layout.Add(new Vector3Int(i,j,0));
                }
            }

            //Generate the middle block layer 2
            for (int i = -2; i <= 3; i++)
            {
                for (int j = -3; j <= 2; j++)
                {
                    layout.Add(new Vector3Int(i, j, 1));
                }
            }

            //Generate the middle block layer 3
            for (int i = -1; i <= 2; i++)
            {
                for (int j = -2; j <= 1; j++)
                {
                    layout.Add(new Vector3Int(i, j, 2));
                }
            }

            //Generate the middle block layer 4
            for (int i = 0; i <= 1; i++)
            {
                for (int j = -1; j <= 0; j++)
                {
                    layout.Add(new Vector3Int(i, j, 3));
                }
            }
            //generate the WINGS
            //top+bottom row
            layout.Add(new Vector3Int(-5,3,0));
            layout.Add(new Vector3Int(-4, 3, 0));
            layout.Add(new Vector3Int(5, 3, 0));
            layout.Add(new Vector3Int(6, 3, 0));
            layout.Add(new Vector3Int(-5, -4, 0));
            layout.Add(new Vector3Int(-4, -4, 0));
            layout.Add(new Vector3Int(5, -4, 0));
            layout.Add(new Vector3Int(6, -4, 0));
            
            for(int k = -2;k <= 1;k++)
            {
                layout.Add(new Vector3Int(-4,k,0));
                layout.Add(new Vector3Int(5, k, 0));
            }
            layout.Add(new Vector3Int(-5,0,0));
            layout.Add(new Vector3Int(-5, -1, 0));
            layout.Add(new Vector3Int(6, 0, 0));
            layout.Add(new Vector3Int(6, -1, 0));
            return layout;
        }

        public bool TryGetSpace(Vector3Int pos, out Space space)
        {
            return _board.TryGetValue(pos, out space);
        }
    }
}