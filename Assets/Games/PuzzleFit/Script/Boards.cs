
using System.Collections.Generic;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace BoardFit
{
    public class Boards : MonoBehaviour
    {
        [SerializeField]
        float hoverShadowDistance = 0.3f;
        [SerializeField]
        public Vector2Int boardSize;
        [SerializeField]
        List<SquareGrid> boardGrid;
        [SerializeField]
        Transform fillGridParent;

        Dictionary<Vector2Int, SquareGrid> Coordinates = new Dictionary<Vector2Int, SquareGrid>();
        Dictionary<Vector2Int, SquareGrid> FilledCoordinates = new Dictionary<Vector2Int, SquareGrid>();

        HashSet<SquareGrid> newShadow = new HashSet<SquareGrid>();
        HashSet<SquareGrid> shadowCache = new HashSet<SquareGrid>();

        private void Start()
        {
            int counter = 0;
            for(int i = 0; i < boardSize.y; i++) 
            {
                for (int j = 0; j < boardSize.x; j++)
                {
                    boardGrid[counter].Coord = new Vector2Int(j, i);
                    Coordinates.Add(boardGrid[counter].Coord, boardGrid[counter]);
                    counter++;
                }
            }
        }

        public bool Hover(Vector2 Coord, IReadOnlyList<Vector2Int> coords, out Vector2Int targetCoords)
        {
            targetCoords = Vector2Int.one * -1;
            bool foundCoord = false;
            float minDist = hoverShadowDistance;
            foreach (var item in Coordinates)
            {
                float dist = Vector2.Distance(item.Value.transform.position, Coord);
                if (dist < minDist)
                {
                    minDist = dist;
                    foundCoord = true;
                    targetCoords = item.Key;
                    break;
                }
            }

            if (foundCoord)
            {
                foreach (var item in coords)
                {
                    var finalCoord = item + targetCoords;
                    if (!Coordinates.TryGetValue(finalCoord, out var grid) || FilledCoordinates.ContainsKey(finalCoord))
                    {
                        newShadow.Clear();
                        RemoveAllShadow();
                        return false;
                    }
                    newShadow.Add(grid);
                }

                RemoveAllShadow();
                foreach (var item in newShadow)
                {
                    item.SetShadow(true);
                }

                var temp = shadowCache;
                shadowCache = newShadow;
                newShadow = temp;
            }
            else
            {
                newShadow.Clear();
                RemoveAllShadow();
            }
            return foundCoord;
        }

        public void RemoveAllShadow() 
        {
            foreach (var item in shadowCache)
            {
                if (newShadow.Contains(item)) continue;

                item.SetShadow(false);
            }
            shadowCache.Clear();
        }

        [Button]
        public async void Apply(Vector2Int Coord, PuzzleGenerator puzzle)
        {
            foreach (var item in puzzle.Grids)
            {
                Vector2Int finalCoord = Coord + item.Coord;
                if (FilledCoordinates.ContainsKey(finalCoord))
                {
                    Debug.LogError("This should never happen!");
                }

                FilledCoordinates.Add(finalCoord, item);

                item.transform.SetParent(null);
                item.TransformSpring.targetTransform.position = Coordinates[finalCoord].transform.position;
                item.TransformSpring.targetTransform.localScale = Vector3.one * 0.95f;
                item.transform.SetParent(fillGridParent);

                item.SetStyle(0);
            }

            await Task.Delay(2000);

            foreach (var item in puzzle.Grids)
            {
                item.TransformSpring.useTransformAsTarget = false;
                item.TransformSpring.targetTransform = null;
            }
            Destroy(puzzle.gameObject);
            return;
        }
    }
}