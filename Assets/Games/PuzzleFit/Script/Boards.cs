
using System.Collections.Generic;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace BoardFit
{
    public class Boards : MonoBehaviour
    {
        [SerializeField]
        public Vector2Int boardSize;
        [SerializeField]
        List<SquareGrid> boardGrid;
        [SerializeField]
        Transform fillGridParent;

        Dictionary<Vector2Int, SquareGrid> Coordinates = new Dictionary<Vector2Int, SquareGrid>();
        Dictionary<Vector2Int, SquareGrid> FilledCoordinates = new Dictionary<Vector2Int, SquareGrid>();

        private void Start()
        {
            int counter = 0;
            for(int i = 0; i < boardSize.y; i++) 
            {
                for (int j = 0; j < boardSize.x; j++)
                {
                    Coordinates.Add(new Vector2Int(j,i) , boardGrid[counter]);
                    counter++;
                }
            }
        }

        private bool Hover(Vector2Int Coord)
        {
            return true;
        }

        [Button]
        private async void Apply(Vector2Int Coord, PuzzleGenerator puzzle)
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