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
        Boards board;
        [SerializeField]
        PuzzleGenerator prefab;

        [SerializeField]
        public List<RectTransform> spawnPointScreen = new List<RectTransform>();

        [SerializeField]
        List<PuzzleStucture> allStructure = new List<PuzzleStucture>();

        public List<PuzzleGenerator> ActiveGenerator = new List<PuzzleGenerator>();

        private async void Start()
        {
            await Task.Delay(1000);
            SpawnPuzzle();
        }

        [Button]
        public async void SpawnPuzzle() 
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

                await Task.Delay(500);
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

        public void TryPushPuzzle(Vector3 position, PuzzleGenerator puzzle)
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

            if (puzzleCount == 0)
            {
                SpawnPuzzle();
            }

            CheckGameOver();
        }

        private void CheckGameOver() 
        {
            Debug.Log("Check GameOver");
        }
    }
}