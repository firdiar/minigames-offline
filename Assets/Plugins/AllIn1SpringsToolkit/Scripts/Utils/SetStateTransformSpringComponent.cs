using System;
using UnityEngine;

namespace AllIn1SpringsToolkit
{
    public class SetStateTransformSpringComponent : MonoBehaviour
    {
        [Header("When to set the state")]
        [SerializeField] private TransformSpringComponent transformSpringComponent;
        [SerializeField] private bool onStart, onEnable, onUpdate;

        [Space, Header("Position")]
        [SerializeField] private bool setPositionEnabled;
        [SerializeField] private bool setPositionTarget, setPositionAtEquilibrium;
        [SerializeField] private Vector3 newPositionTarget;
        [SerializeField] private bool setPositionValue;
        [SerializeField] private Vector3 newPositionValue;
    
        [Space, Header("Rotation")]
        [SerializeField] private bool setRotationEnabled;
        [SerializeField] private bool setRotationTarget, setRotationAtEquilibrium;
        [SerializeField] private Quaternion newRotationTarget;
        [SerializeField] private bool setRotationValue;
        [SerializeField] private Quaternion newRotationValue;
    
        [Space, Header("Scale")]
        [SerializeField] private bool setScaleEnabled;
        [SerializeField] private bool setScaleTarget, setScaleAtEquilibrium;
        [SerializeField] private Vector3 newScaleTarget;
        [SerializeField] private bool setScaleValue;
        [SerializeField] private Vector3 newScaleValue;
        
        private void SetState()
        {
            if (setPositionEnabled)
            {
                if (setPositionTarget)
                {
                    transformSpringComponent.positionSpring.SetTarget(newPositionTarget);
                }
                
                if (setPositionAtEquilibrium)
                {
                    transformSpringComponent.positionSpring.ReachEquilibrium();
                }
                
                if (setPositionValue)
                {
                    transformSpringComponent.positionSpring.SetCurrentValue(newPositionValue);
                }
            }
            
            if (setRotationEnabled)
            {
                if (setRotationTarget)
                {
                    transformSpringComponent.rotationSpring.SetTarget(newRotationTarget);
                }
                
                if (setRotationAtEquilibrium)
                {
                    transformSpringComponent.rotationSpring.ReachEquilibrium();
                }
                
                if (setRotationValue)
                {
					transformSpringComponent.rotationSpring.SetCurrentValue(newRotationValue);
				}
			}
            
            if (setScaleEnabled)
            {
                if (setScaleTarget)
                {
                    transformSpringComponent.scaleSpring.SetTarget(newScaleTarget);
                }
                
                if (setScaleAtEquilibrium)
                {
                    transformSpringComponent.scaleSpring.ReachEquilibrium();
                }
                
                if (setScaleValue)
                {
                    transformSpringComponent.scaleSpring.SetCurrentValue(newScaleValue);
                }
            }
        }
        
        private void Start()
        {
            if (onStart)
            {
                SetState();
            }
        }
        
        private void OnEnable()
        {
            if (onEnable)
            {
                SetState();
            }
        }

        private void Update()
        {
            if (onUpdate)
            {
                SetState();
            }
        }

        private void Reset()
        {
            if (transformSpringComponent == null)
            {
                transformSpringComponent = GetComponent<TransformSpringComponent>();
            }
        }
    }
}