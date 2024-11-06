using System;
using UnityEngine;

namespace AllIn1SpringsToolkit.Scripts.Utils
{
    [DefaultExecutionOrder(505)]
    public class SpringBoneUpdater : MonoBehaviour
    {
        [SerializeField] private SpringBone[] bonesToUpdate;

        private void LateUpdate()
        {
            foreach(SpringBone springBone in bonesToUpdate)
            {
                springBone.UpdateMethod();
            }
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            bonesToUpdate = GetComponentsInChildren<SpringBone>();
        }

        private void OnValidate()
        {
            foreach(SpringBone springBone in bonesToUpdate)
            {
                springBone.SetAutoUpdate(false);
            }
        }
        #endif
    }
}
