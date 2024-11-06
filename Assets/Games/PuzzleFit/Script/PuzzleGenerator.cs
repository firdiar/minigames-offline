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
        PuzzleStucture structure;

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
        public void NormalMode()
        {
            SetScale(0.4f);
            SetSpacing(0.4f);

            foreach (var grid in grids)
            {
                grid.SetStyle(0);
                grid.SetBonusRenderer(20);
            }
        }

        [Button]
        public void AssignMode()
        {
            SetScale(0.95f);
            SetSpacing(0.95f);

            foreach (var grid in grids)
            {
                grid.SetStyle(0);
                grid.SetBonusRenderer(10);
            }
        }

        [Button]
        public void HoverMode() 
        {
            SetScale(0.7f);
            SetSpacing(1.1f);

            foreach (var grid in grids)
            {
                grid.SetStyle(1);

                grid.SetBonusRenderer(40);
            }
        }

        public void Generate(float scale, float space , PuzzleStucture structure)
        {
            this.scale = scale;
            this.spacing = space;
            this.structure = structure;
            Generate();
        }

        public void Generate(PuzzleStucture structure)
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
                    item.DestroySelf();
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