using System.Collections.Generic;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;
using static UnityEditor.Progress;

namespace BoardFit
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        UIView uiView;
        [SerializeField]
        Boards board;
        [SerializeField]
        PuzzleGenerator prefab;

        int score;

        [SerializeField]
        public List<RectTransform> spawnPointScreen = new List<RectTransform>();

        [SerializeField]
        List<PuzzleStucture> allStructure = new List<PuzzleStucture>();

        public List<PuzzleGenerator> ActiveGenerator = new List<PuzzleGenerator>();

        private async void Start()
        {
            await Task.Delay(1000);
            await SpawnPuzzle();
        }

        [Button]
        public async Task SpawnPuzzle() 
        {
            ActiveGenerator.Clear();
            var mainCam = Camera.main;
            foreach (var item in spawnPointScreen)
            {
                var worldPoint = mainCam.ScreenToWorldPoint(item.position);
                worldPoint.z = 0;

                var generator = Instantiate(prefab, worldPoint, Quaternion.identity);
                ActiveGenerator.Add(generator);

                generator.Generate( 0.4f , 0.4f ,GetRandom());

                await Task.Delay(200);
            }
        }

        private PuzzleStucture GetRandom() 
        {
            return allStructure[Random.Range(0 , allStructure.Count)];
        }

        public void ShowShadow(Vector3 position, IReadOnlyList<Vector2Int> coords)
        {
            board.Hover(position, coords, out _);
        }

        public async void TryPushPuzzleAsync(Vector3 position, PuzzleGenerator puzzle)
        {
            var idx = ActiveGenerator.FindIndex(item => item == puzzle);
            if (board.Hover(position, puzzle.Structure, out var coord))
            {
                board.Apply(coord, puzzle);
                ActiveGenerator[idx] = null;
            }
            else
            {
                var worldPoint = Camera.main.ScreenToWorldPoint(spawnPointScreen[idx].position);
                worldPoint.z = 0;
                ActiveGenerator[idx].transform.position = worldPoint;
                ActiveGenerator[idx].NormalMode();
            }

            board.RemoveAllShadow();

            int puzzleCount = 0;
            foreach (var item in ActiveGenerator)
            {
                if (item != null) puzzleCount++;
            }

            var completeGrid = board.CheckComplete();
            if(completeGrid > 0)
            {
                score += completeGrid;
                uiView.SetScore(score);
            }

            if (puzzleCount == 0)
            {
                await SpawnPuzzle();
            }
            CheckGameOver();
        }

        [Button]
        private void CheckGameOver() 
        {
            bool isGameOver = true;
            
            foreach(var item in ActiveGenerator)
            {
                if(item == null) continue;

                Debug.Log("Check game over: "+ item);
                if (board.IsAnySpaceAvailable(item))
                {
                    isGameOver = false;
                    break;
                }
            }

            if (isGameOver)
            {
                uiView.ShowGameOver();
            }
        }

    }
}