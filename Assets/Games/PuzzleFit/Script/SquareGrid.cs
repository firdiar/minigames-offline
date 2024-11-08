using AllIn1SpringsToolkit;
using DG.Tweening;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using static UnityEditor.Progress;

namespace BoardFit
{
    public class SquareGrid : MonoBehaviour
    {
        [System.Serializable]
        public class ColorStyle
        {
            public Color main;
            public Color light;
            public Color dark;
        }

        public TransformSpringComponent TransformSpring;
        [SerializeField]
        GameObject shadow;

        [Header("Style")]
        [SerializeField]
        SpriteRenderer lightSprite;
        [SerializeField]
        SpriteRenderer mainSprite;
        [SerializeField]
        SpriteRenderer darkSprite; 
        [SerializeField]
        bool applyDefaultStyle;
        [SerializeField]
        [ShowIf("applyDefaultStyle")]
        int defaultSyle;
        [SerializeField]
        List<ColorStyle> colors;

        [Header("Renderer")]
        [SerializeField]
        int bonusRenderer = 10;
        [SerializeField]
        List<SpriteRenderer> allRenderer;

        public Vector2Int Coord { get; set; }

        private void Start()
        {
            if (applyDefaultStyle)
            {
                SetStyle(defaultSyle);
            }
        }

        public void SetBonusRenderer(int bonus)
        {
            bonusRenderer = bonus - bonusRenderer;
            foreach (var renderer in allRenderer)
            {
                renderer.sortingOrder += bonus;
            }
        }

        [Button]
        public void SetStyle(int styleIndex) 
        {
            if (!Application.isPlaying)
            {
                Debug.Log("Apply color");
                lightSprite.color = colors[styleIndex].light;
                mainSprite.color = colors[styleIndex].main;
                darkSprite.color = colors[styleIndex].dark;
                return;
            }

            lightSprite.DOColor(colors[styleIndex].light , 0.2f);
            mainSprite.DOColor(colors[styleIndex].main, 0.2f);
            darkSprite.DOColor(colors[styleIndex].dark, 0.2f);
        }

        public void SetShadow(bool enabled)
        {
            shadow.SetActive(enabled);
        }

        private void OnDestroy()
        {
            if (TransformSpring != null && TransformSpring.targetTransform != null)
            {
                var target = TransformSpring.targetTransform.gameObject;
                if (Application.isPlaying)
                {
                    Destroy(target);
                }
                else
                {
                    DestroyImmediate(target);
                }
            }
        }

        public void DestroySelf() 
        {
            TransformSpring.enabled = false;
            transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => Destroy(gameObject));
        }
    }
}