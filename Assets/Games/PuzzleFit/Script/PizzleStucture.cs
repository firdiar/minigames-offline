using System.Collections.Generic;
using UnityEngine;

namespace BoardFit
{
    [CreateAssetMenu(fileName = "PizzleStucture_", menuName = "Scriptable Objects/BoardFit/PizzleStucture")]
    public class PizzleStucture : ScriptableObject
    {
        [SerializeField]
        public string StructureName;
        [SerializeField]
        List<Vector2Int> coords;

        public IReadOnlyList<Vector2Int> Coords => coords;
    }
}