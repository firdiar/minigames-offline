using System;
using UnityEngine;

namespace AllIn1SpringsToolkit.Demo.Scripts
{
    public class SpringBonesDemoController : DemoElement
    {
        [Space, Header("References")]
        [SerializeField] private Transform parentTransform;

        [Space, Header("Movement Parameters")]
        [SerializeField] private float verticalMoveSpeed;
        [SerializeField] private float rotateZMoveSpeed;
        [SerializeField] private float rotateYMoveSpeed;
        [SerializeField] private float movementAmplitude, rotationAmplitude;

        private enum MoveMode
        {
            VerticalMove,
            RotateZ,
            RotateY,
        }

        private MoveMode currentMoveMode;
        private float iniYPos;
        private float timeSinceEnter, currentMoveTime, currentRotationZTime, currentRotationYTime;

        private void Start()
        {
            currentMoveMode = MoveMode.VerticalMove;
            iniYPos = parentTransform.localPosition.y;
            currentMoveTime = 0f;
            currentRotationZTime = 0f;
            currentRotationYTime = 0f;
            timeSinceEnter = 0f;
        }

        private void Update()
        {
            if(!IsOpen())
            {
                return;
            }
            
            timeSinceEnter += Time.deltaTime;
            if(timeSinceEnter < 0.5f)
            {
                return;
            }
    
            switch(currentMoveMode)
            {
                case MoveMode.VerticalMove:
                    currentMoveTime += Time.deltaTime;
                    float verticalOffset = Mathf.Sin(currentMoveTime * verticalMoveSpeed) * movementAmplitude;
                    parentTransform.localPosition = new Vector3(parentTransform.localPosition.x, iniYPos + verticalOffset, parentTransform.localPosition.z);
                    break;
                case MoveMode.RotateZ:
                    currentRotationZTime += Time.deltaTime;
                    float rotationZ = Mathf.Sin(currentRotationZTime * rotateZMoveSpeed) * rotationAmplitude;
                    parentTransform.localRotation = Quaternion.Euler(parentTransform.localRotation.eulerAngles.x, parentTransform.localRotation.eulerAngles.y, rotationZ);
                    break;
                case MoveMode.RotateY:
                    currentRotationYTime += Time.deltaTime;
                    float rotationY = Mathf.Sin(currentRotationYTime * rotateYMoveSpeed) * rotationAmplitude;
                    parentTransform.localRotation = Quaternion.Euler(parentTransform.localRotation.eulerAngles.x, rotationY, parentTransform.localRotation.eulerAngles.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void ToggleMoveMode()
        {
            currentMoveMode = (MoveMode)(((int)currentMoveMode + 1) % System.Enum.GetValues(typeof(MoveMode)).Length);
        }
    }
}