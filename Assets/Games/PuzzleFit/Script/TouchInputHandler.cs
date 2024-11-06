using DG.Tweening.Core.Easing;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BoardFit
{
    public class TouchInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField]
        float distancePickArea;
        [SerializeField]
        GameManager gameManager;

        int holdingPuzzleIndex;
        PuzzleGenerator holdingPuzzle;
        Transform holdingPuzzleTransform;
        Camera cameraRef;
        private void Awake()
        {
            cameraRef = Camera.main;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var indexTap = CheckNearestDistanceIndex(eventData.position);
            if (indexTap < 0 || gameManager.ActiveGenerator[indexTap] == null)
            {
                holdingPuzzleIndex = -1;
                holdingPuzzle = null;
                holdingPuzzleTransform = null;
                return;
            }

            holdingPuzzleIndex = indexTap;
            holdingPuzzle = gameManager.ActiveGenerator[indexTap];
            holdingPuzzleTransform = holdingPuzzle.transform;
            holdingPuzzle.HoverMode();

            var worldPos = cameraRef.ScreenToWorldPoint(eventData.position);
            worldPos.z = 0;

            holdingPuzzle.transform.position = worldPos;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (holdingPuzzle != null)
            {
                var worldPos = cameraRef.ScreenToWorldPoint(eventData.position);
                worldPos.z = 0;
                gameManager.TryPushPuzzleAsync(worldPos, holdingPuzzle);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (holdingPuzzleIndex < 0) return;

            var worldPos = cameraRef.ScreenToWorldPoint(eventData.position);
            worldPos.z = 0;

            holdingPuzzle.transform.position = worldPos;
            gameManager.ShowShadow(worldPos, holdingPuzzle.Structure);
        }

        private int CheckNearestDistanceIndex(Vector2 screenTap) 
        {
            int result = 0;
            foreach (var item in gameManager.spawnPointScreen)
            {
                var dist = Vector2.Distance(item.position, screenTap);
                if (dist < distancePickArea)
                {
                    return result;
                }
                result++;
            }

            return -1;
        }
    }
}