using System.Collections.Generic;
using UnityEngine;
using TriInspector;
using DG.Tweening;
using static UnityEditor.Progress;
using UnityEngine.UIElements;

namespace BoardFit
{
    public class PuzzleGenerator : MonoBehaviour
    {
        [SerializeField]
        SquareGrid prefab;
        [SerializeField]
        float scale = 1;
        [SerializeField]
        float spacing = 1;

        [SerializeField]
        PizzleStucture structure;

        public IReadOnlyList<Vector2Int> Structure => structure.Coords;

        [Header("Follows")]
        [SerializeField]
        Transform gridParent;
        [SerializeField]
        Transform targetParent;

        [Header("Runtime")]
        [SerializeField]
        [ReadOnly]
        List<SquareGrid> grids = new List<SquareGrid>();

        public IReadOnlyList<SquareGrid> Grids => grids;


        [Button]
        public void MoveHover() 
        {
            scale = 0.7f;
            Generate();
            foreach (var grid in grids)
            {
                grid.SetStyle(1);
            }
        }

        public void Generate(PizzleStucture structure)
        { 
            this.structure = structure;
            Generate();
        }

        [Button] 
        public void Generate() 
        {
            Destroy();

            var defaultScale = Vector3.one * scale;
            var center = transform.position;
            foreach (var cord in Structure)
            {
                var spawnCord = ((Vector2)cord * spacing);
                var instance = Instantiate(prefab ,(Vector2) center + spawnCord, Quaternion.identity, gridParent);

                var target = new GameObject("TargetGrid");
                target.transform.SetParent(targetParent);
                target.transform.localPosition = spawnCord;
                instance.TransformSpring.targetTransform = target.transform;
                instance.TransformSpring.useTransformAsTarget = true;

                grids.Add(instance);
                instance.Coord = cord;

                if (Application.isPlaying)
                {
                    instance.transform.localScale = Vector3.zero;
                    instance.TransformSpring.SetCurrentValueScale(Vector3.zero);
                    target.transform.localScale = defaultScale;
                }
                else
                {
                    instance.transform.localScale = defaultScale;
                }
                
            }
            return;
        }

        [Button]
        public void Destroy() 
        {
            foreach (var item in grids)
            {
                if (item == null) continue;

                if (Application.isPlaying)
                {
                    item.TransformSpring.enabled = false;
                    item.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(item.gameObject));
                }
                else
                {
                    DestroyImmediate(item.gameObject);
                }
            }

            grids.Clear();
        }

        [Button]
        public void SetScale(float scale)
        {
            this.scale = scale;
            foreach (var item in grids)
            {
                if (item == null) continue;

                //item.TransformSpring.SetTargetScale(Vector3.one * scale);
                item.TransformSpring.targetTransform.localScale = Vector3.one * scale;
            }
        }

        [Button]
        public void SetSpacing(float spacing)
        {
            this.spacing = spacing;
            foreach (var item in grids)
            {
                if (item == null) continue;

                var spawnCord = ((Vector2)item.Coord * spacing);
                item.TransformSpring.targetTransform.localPosition = spawnCord;
            }
        }
    }
}