using System.Collections.Generic;
using UnityEngine;

namespace BoardFit
{
    [CreateAssetMenu(fileName = "PuzzleStucture_", menuName = "Scriptable Objects/BoardFit/PizzleStucture")]
    public class PuzzleStucture : ScriptableObject
    {
        [SerializeField]
        public string StructureName;
        [SerializeField]
        List<Vector2Int> coords;

        public IReadOnlyList<Vector2Int> Coords => coords;
    }
}