using UnityEngine;

namespace AllIn1SpringsToolkit
{
	public partial class RigidbodySpringComponent
	{
		#region PUBLIC_POSITION_SPRING_METHODS
		
		public Vector3 GetTargetPosition() => positionSpring.GetTarget();
		public void SetTargetPosition(Vector3 target) => positionSpring.SetTarget(target);
		public void SetTargetPosition(float target) => SetTargetPosition(Vector3.one * target);
		public Vector3 GetCurrentValuePosition() => positionSpring.GetCurrentValue();
		public void SetCurrentValuePosition(Vector3 currentValues) => positionSpring.SetCurrentValue(currentValues);
		public void SetCurrentValuePosition(float currentValues) => SetCurrentValuePosition(Vector3.one * currentValues);
		public Vector3 GetVelocityPosition() => positionSpring.GetVelocity();
		public void SetVelocityPosition(Vector3 velocity) => positionSpring.SetVelocity(velocity);
		public void SetVelocityPosition(float velocity) => SetVelocityPosition(Vector3.one * velocity);
		public void AddVelocityPosition(Vector3 velocityToAdd) =>	positionSpring.AddVelocity(velocityToAdd);
		public void ReachEquilibriumPosition() => positionSpring.ReachEquilibrium();
		public Vector3 GetForcePosition() => positionSpring.GetForce();
		public void SetForcePosition(Vector3 force) => positionSpring.SetForce(force);
		public void SetForcePosition(float force) => SetForcePosition(Vector3.one * force);
		public Vector3 GetDragPosition() => positionSpring.GetDrag();
		public void SetDragPosition(Vector3 drag) => positionSpring.SetDrag(drag);
		public void SetDragPosition(float drag) => SetDragPosition(Vector3.one * drag);
		public void SetUnifiedForcePosition(float unifiedForce)
		{
			positionSpring.SetUnifiedForceAndDragEnabled(true);
			positionSpring.SetUnifiedForce(unifiedForce);
		}
		public void SetUnifiedDragPosition(float unifiedDrag)
		{
			positionSpring.SetUnifiedForceAndDragEnabled(true);
			positionSpring.SetUnifiedDrag(unifiedDrag);
		}
		public void SetUnifiedForceAndDragPosition(float unifiedForce, float unifiedDrag)
		{
			SetUnifiedForcePosition(unifiedForce);
			SetUnifiedDragPosition(unifiedDrag);
		}
		public void SetMinValuesPosition(Vector3 minValue) => positionSpring.SetMinValues(minValue);
		public void SetMinValuesPosition(float minValue) => SetMinValuesPosition(Vector3.one * minValue);
		public void SetMaxValuesPosition(Vector3 maxValue) => positionSpring.SetMaxValues(maxValue);
		public void SetMaxValuesPosition(float maxValue) => SetMaxValuesPosition(Vector3.one * maxValue);
		public void SetClampCurrentValuesPosition(bool clampTargetX, bool clampTargetY, bool clampTargetZ) => positionSpring.SetClampCurrentValues(clampTargetX, clampTargetY, clampTargetZ);
		public void SetClampTargetPosition(bool clampTargetX, bool clampTargetY, bool clampTargetZ) => positionSpring.SetClampTarget(clampTargetX, clampTargetY, clampTargetZ);
		public void StopSpringOnClampPosition(bool stopX, bool stopY, bool stopZ) => positionSpring.StopSpringOnClamp(stopX, stopY, stopZ);
		
		#endregion
		
		#region PUBLIC_ROTATION_SPRING_METHODS
		
		public Quaternion GetTargetRotation() => rotationSpring.GetTarget();
		public void SetTargetRotation(Quaternion targetQuaternion)
		{
			rotationSpring.SetTarget(targetQuaternion);
		}
		public void SetTargetRotation(Vector3 targetEuler)
		{
			rotationSpring.SetTarget(targetEuler);
		}
		public Quaternion GetCurrentValueRotation() => rotationSpring.GetCurrentValue();
		public void SetCurrentValueRotation(Quaternion currentQuaternion) => rotationSpring.SetCurrentValue(currentQuaternion);
		public void SetCurrentValueRotation(Vector3 currentEuler) => rotationSpring.SetCurrentValue(currentEuler);
		public Vector3 GetVelocityRotation() => rotationSpring.GetVelocity();
		public void SetVelocityRotation(Vector3 velocity) => rotationSpring.SetVelocity(velocity);
		public void AddVelocityRotation(Vector3 velocityToAdd) => rotationSpring.AddVelocity(velocityToAdd);
		public void ReachEquilibriumRotation() => rotationSpring.ReachEquilibrium();
		public float GetUnifiedForceRotation() => rotationSpring.GetUnifiedForce();
		public float GetUnifiedDragRotation() => rotationSpring.GetUnifiedDrag();
		public void SetUnifiedForceRotation(float unifiedForce)
		{
			rotationSpring.SetUnifiedForceAndDragEnabled(true);
			rotationSpring.SetUnifiedForce(unifiedForce);
		}
		public void SetUnifiedDragRotation(float unifiedDrag)
		{
			rotationSpring.SetUnifiedForceAndDragEnabled(true);
			rotationSpring.SetUnifiedDrag(unifiedDrag);
		}
		public void SetUnifiedForceAndDragRotation(float unifiedForce, float unifiedDrag)
		{
			SetUnifiedForceRotation(unifiedForce);
			SetUnifiedDragRotation(unifiedDrag);
		}
		
		#endregion
	}
}