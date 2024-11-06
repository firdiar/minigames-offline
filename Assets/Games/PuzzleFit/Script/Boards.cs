
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

                item.Coord = finalCoord;
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

        public bool IsAnySpaceAvailable(PuzzleGenerator puzzle)
        {
            foreach(var pair in Coordinates)
            {
                if(IsAvailable(pair.Key , puzzle.Structure)) return true;
            }

            return true;
        }
        private bool IsAvailable(Vector2Int coord, IReadOnlyList<Vector2Int> puzzleCoord)
        {
            foreach (var itemCoord in puzzleCoord)
            {
                Vector2Int finalCoord = coord + itemCoord;
                bool isCoordExist = Coordinates.ContainsKey(finalCoord);
                bool isCoordFilled = FilledCoordinates.ContainsKey(finalCoord);
                if (isCoordFilled || !isCoordExist)
                {
                    return false;
                }
            }

            return true;
        }


        List<SquareGrid> clearedGrids = new List<SquareGrid>();

        public int CheckComplete() 
        {
            int totalScore = 0;
            clearedGrids.Clear();
            for (int i = 0; i < boardSize.x; i++)
            {
                totalScore += CheckVertical(i, clearedGrids);
            }

            for (int i = 0; i < boardSize.y; i++)
            {
                totalScore += CheckHorizontal(i, clearedGrids);
            }

            Vector2Int squareSize = new Vector2Int(3 , 3);
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    Vector2Int coord = new Vector2Int(i * squareSize.x, j * squareSize.y);
                    totalScore += CheckSquare(coord, squareSize, clearedGrids);
                }
            }

            DestroyGrids();

            foreach (var item in clearedGrids)
            {
                FilledCoordinates.Remove(item.Coord);
            }
            clearedGrids.Clear();

            return totalScore;
        }

        private async void DestroyGrids() 
        {
            var ongoingDestroy = new List<SquareGrid> (clearedGrids.Count);
            ongoingDestroy.AddRange(clearedGrids);
            for (int i = ongoingDestroy.Count - 1; i >= 0; i--)
            {
                ongoingDestroy[i].DestroySelf();
                await Task.Delay(50);
            }

            ongoingDestroy.Clear();
        }

        private int CheckVertical(int column, List<SquareGrid> destroyList)
        {
            for(int i = 0; i < boardSize.y; i++) 
            {
                Vector2Int coord = new Vector2Int(column, i);
                if(!FilledCoordinates.ContainsKey(coord)) 
                {
                    return 0;
                }
            }

            for (int i = 0; i < boardSize.y; i++)
            {
                Vector2Int coord = new Vector2Int(column, i);
                destroyList.Add(FilledCoordinates[coord]);
            }

            return boardSize.y;
        }

        private int CheckHorizontal(int row, List<SquareGrid> destroyList)
        {
            for (int i = 0; i < boardSize.x; i++)
            {
                Vector2Int coord = new Vector2Int(i, row);
                if (!FilledCoordinates.ContainsKey(coord))
                {
                    return 0;
                }
            }

            for (int i = 0; i < boardSize.x; i++)
            {
                Vector2Int coord = new Vector2Int(i, row);
                destroyList.Add(FilledCoordinates[coord]);
            }

            return boardSize.x;
        }

        private int CheckSquare(Vector2Int startCoord, Vector2Int sizeSquare, List<SquareGrid> destroyList)
        {
            for (int i = 0; i < sizeSquare.y; i++)
            {
                for (int j = 0; j < sizeSquare.x; j++)
                {
                    var coord = startCoord + new Vector2Int(j , i);
                    if (!FilledCoordinates.ContainsKey(coord))
                    {
                        return 0;
                    }
                }
            }

            for (int i = 0; i < sizeSquare.y; i++)
            {
                for (int j = 0; j < sizeSquare.x; j++)
                {
                    var coord = startCoord + new Vector2Int(j, i);
                    destroyList.Add(FilledCoordinates[coord]);
                }
            }

            return sizeSquare.x * sizeSquare.y;
        }
    }
}