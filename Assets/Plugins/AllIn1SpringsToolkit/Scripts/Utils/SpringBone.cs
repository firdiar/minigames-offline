using UnityEngine;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace AllIn1SpringsToolkit.Scripts.Utils
{
    [DefaultExecutionOrder(500)]
    public class SpringBone : MonoBehaviour
    {
		[SerializeField] private bool autoUpdate = true;
		[SerializeField] private bool autoInitialize = true;
        [SerializeField] private Transform transformToReactTo;
        [SerializeField] private Transform boneTransform;
        
        [Space, Header("Transform Spring")]
        [SerializeField] private TransformSpringComponent transformSpringComponent;

        [Space, Header("Movement Inertia")]
        [SerializeField] private bool movementInertiaEnabled;
        [SerializeField] private float movementInertia;
        
        private Vector3 reactToLastPosition;
        private Quaternion initialRotation;
        private Vector3 reactToInitialUpVector;
        
        private void Start()
        {
            if(autoInitialize)
            {
                Initialize();   
            }
        }

        public void Initialize()
        {
            initialRotation = Quaternion.Inverse(transformToReactTo.rotation) * boneTransform.rotation;
            transformSpringComponent.enabled = true;
            transformSpringComponent.rotationSpring.SetTarget(transformToReactTo.rotation * initialRotation);
            transformSpringComponent.rotationSpring.ReachEquilibrium();
            reactToInitialUpVector = transformToReactTo.up;

            if(movementInertiaEnabled)
            {
                reactToLastPosition = transformToReactTo.position;
            }
        }

        private void LateUpdate()
        {
            if(autoUpdate)
            {
                UpdateMethod();   
            }
        }

        public void UpdateMethod()
        {
            Quaternion targetRotation = transformToReactTo.rotation * initialRotation;
            transformSpringComponent.rotationSpring.SetTarget(targetRotation);
            boneTransform.rotation = transformSpringComponent.rotationSpring.GetCurrentValue();
            
            if(movementInertiaEnabled)
            {
                Vector3 reactToPositionDelta = transformToReactTo.position - reactToLastPosition;
                Vector3 currentUpVector = boneTransform.rotation * reactToInitialUpVector;
                Vector3 rotationAxis = Vector3.Cross(reactToPositionDelta, currentUpVector).normalized;
    
                float angle = reactToPositionDelta.magnitude * movementInertia * Time.deltaTime;
                if(angle > 0f)
                {
                    Vector3 localRotationAxis = Quaternion.Inverse(boneTransform.rotation) * rotationAxis;
                    transformSpringComponent.rotationSpring.AddVelocity(localRotationAxis * angle);
                }

                reactToLastPosition = transformToReactTo.position;
            }
        }
        
        public void SetAutoUpdate(bool value)
        {
            autoUpdate = value;
        }
        
        public void SetNewTransformToReactTo(Transform newTransformToReactTo)
        {
            transformToReactTo = newTransformToReactTo;
        }

        #region Editor helpers
        #if UNITY_EDITOR
        private void Reset()
        {
            transformToReactTo = transform.parent;
            boneTransform = transform;
            
            transformSpringComponent = GetComponent<TransformSpringComponent>();
            if(transformSpringComponent == null)
            {
                transformSpringComponent = gameObject.AddComponent<TransformSpringComponent>();
            }
            
            TryApplyTransformSpringBonePreset();
        }

        [ContextMenu("Try Apply Transform Spring Bone Preset")]
        private void TryApplyTransformSpringBonePreset()
        {
            if(transformSpringComponent != null)
            {
                // Find and apply the "TransformSpring-BoneSetup.preset"
                string[] guids = UnityEditor.AssetDatabase.FindAssets("TransformSpring-BoneSetup t:preset");
                if(guids.Length > 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    UnityEditor.Presets.Preset preset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.Presets.Preset>(path);
                    if(preset != null)
                    {
                        if(preset.CanBeAppliedTo(transformSpringComponent))
                        {
                            preset.ApplyTo(transformSpringComponent);
                            UnityEditor.EditorUtility.SetDirty(transformSpringComponent);
                        }
                        else
                        {
                            Debug.LogWarning("Preset is not compatible with TransformSpringComponent.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("TransformSpring-BoneSetup preset found but couldn't be loaded.");
                    }
                }
                else
                {
                    Debug.LogError("TransformSpring-BoneSetup preset not found in the project (it was in the Demo).");
                }
            }
        }

        [ContextMenu("Auto setup for all children")]
        private void AddBonesToHierarchy()
        {
            AddBonesToTransform(transform);
            SetChildSpringsToThis();
        }

        private void AddBonesToTransform(Transform current)
        {
            foreach (Transform child in current)
            {
                TransformSpringComponent newTransformSpringComponent = child.GetComponent<TransformSpringComponent>();
                if(newTransformSpringComponent == null)
                {
                    newTransformSpringComponent = child.gameObject.AddComponent<TransformSpringComponent>();
                }

                SpringBone springBone = child.GetComponent<SpringBone>();
                if(springBone == null)
                {
                    springBone = child.gameObject.AddComponent<SpringBone>();
                }
                
                AddBonesToTransform(child);
            }
        }

        [ContextMenu("Copy Spring Component To Children")]
        private void SetChildSpringsToThis()
        {
            ComponentUtility.CopyComponent(transformSpringComponent);
            TransformSpringComponent[] springComponents = GetComponentsInChildren<TransformSpringComponent>();
            foreach(TransformSpringComponent springComponent in springComponents)
            {
                bool previousUseTransformAsTarget = springComponent.useTransformAsTarget;
                Transform previousTargetTransform = springComponent.targetTransform;
                ComponentUtility.PasteComponentValues(springComponent);
                springComponent.useTransformAsTarget = previousUseTransformAsTarget;
                springComponent.targetTransform = previousTargetTransform;
            }
        }
        #endif
        #endregion
    }
}