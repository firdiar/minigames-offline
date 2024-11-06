using UnityEngine;

namespace AllIn1SpringsToolkit
{
	[System.Serializable]
	public class SpringRotation : Spring
	{
		public const int SPRING_SIZE = 9;

		private const int FORWARD_X = 0;
		private const int FORWARD_Y = 1;
		private const int FORWARD_Z = 2;

		private const int UP_X = 6;
		private const int UP_Y = 7;
		private const int UP_Z = 8;

		private const int LOCAL_AXIS_X = 3;
		private const int LOCAL_AXIS_Y = 4;
		private const int LOCAL_AXIS_Z = 5;

		public SpringRotation() : base(SPRING_SIZE)
		{

		}

		public override int GetSpringSize()
		{
			return SPRING_SIZE;
		}

		public override bool HasValidSize()
		{
			return (springValues.Length == SPRING_SIZE);
		}

		#region CURRENT VALUES
		private Vector3 CurrentLocalAxis
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[LOCAL_AXIS_X].GetCurrentValue(),
					springValues[LOCAL_AXIS_Y].GetCurrentValue(),
					springValues[LOCAL_AXIS_Z].GetCurrentValue());

				return res;
			}
			set
			{
				springValues[LOCAL_AXIS_X].SetCurrentValue(value.x);
				springValues[LOCAL_AXIS_Y].SetCurrentValue(value.y);
				springValues[LOCAL_AXIS_Z].SetCurrentValue(value.z);
			}
		}

		private Vector3 CurrentForward
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[FORWARD_X].GetCurrentValue(),
					springValues[FORWARD_Y].GetCurrentValue(),
					springValues[FORWARD_Z].GetCurrentValue()).normalized;

				return res;
			}
			set
			{
				springValues[FORWARD_X].SetCurrentValue(value.x);
				springValues[FORWARD_Y].SetCurrentValue(value.y);
				springValues[FORWARD_Z].SetCurrentValue(value.z);
			}
		}

		private Vector3 CurrentUp
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[UP_X].GetCurrentValue(),
					springValues[UP_Y].GetCurrentValue(),
					springValues[UP_Z].GetCurrentValue());

				return res;
			}
			set
			{
				springValues[UP_X].SetCurrentValue(value.x);
				springValues[UP_Y].SetCurrentValue(value.y);
				springValues[UP_Z].SetCurrentValue(value.z);
			}
		}

		public Quaternion GetCurrentGlobalRotation()
		{
			Quaternion res = Quaternion.LookRotation(CurrentForward, CurrentUp);
			return res;
		}

		public Quaternion GetCurrentValue()
		{
			Quaternion globalQuat = GetCurrentGlobalRotation();

			Vector3 forward = globalQuat * Vector3.forward;
			Vector3 up = globalQuat * Vector3.up;
			Vector3 right = globalQuat * Vector3.right;

			Quaternion res =
				Quaternion.AngleAxis(CurrentLocalAxis.x, right) *
				Quaternion.AngleAxis(CurrentLocalAxis.y, up) *
				Quaternion.AngleAxis(CurrentLocalAxis.z, forward) *
				globalQuat;

			return res;
		}

		private Quaternion GetSecondaryRotation()
		{
			Quaternion globalQuat = GetCurrentGlobalRotation();

			Vector3 forward = globalQuat * Vector3.forward;
			Vector3 up = globalQuat * Vector3.up;
			Vector3 right = globalQuat * Vector3.right;

			Quaternion res = Quaternion.AngleAxis(CurrentLocalAxis.x, right) *
				Quaternion.AngleAxis(CurrentLocalAxis.y, up) *
				Quaternion.AngleAxis(CurrentLocalAxis.z, forward);

			return res;
		}

		public void SetCurrentValue(Quaternion currentQuaternion)
		{
			CurrentForward = (currentQuaternion * Vector3.forward).normalized;
			CurrentUp = (currentQuaternion * Vector3.up).normalized;
		}

		public void SetCurrentValue(Vector3 currentEuler)
		{
			CurrentLocalAxis = currentEuler;
		}
		#endregion

		#region TARGET
		private Vector3 TargetLocalAxis
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[LOCAL_AXIS_X].GetTarget(),
					springValues[LOCAL_AXIS_Y].GetTarget(),
					springValues[LOCAL_AXIS_Z].GetTarget());

				return res;
			}
			set
			{
				springValues[LOCAL_AXIS_X].SetTarget(value.x);
				springValues[LOCAL_AXIS_Y].SetTarget(value.y);
				springValues[LOCAL_AXIS_Z].SetTarget(value.z);
			}
		}

		private Vector3 TargetForward
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[FORWARD_X].GetTarget(),
					springValues[FORWARD_Y].GetTarget(),
					springValues[FORWARD_Z].GetTarget());

				return res;
			}
			set
			{
				springValues[FORWARD_X].SetTarget(value.x);
				springValues[FORWARD_Y].SetTarget(value.y);
				springValues[FORWARD_Z].SetTarget(value.z);
			}
		}

		private Vector3 TargetUp
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[UP_X].GetTarget(),
					springValues[UP_Y].GetTarget(),
					springValues[UP_Z].GetTarget());

				return res;
			}
			set
			{
				springValues[UP_X].SetTarget(value.x);
				springValues[UP_Y].SetTarget(value.y);
				springValues[UP_Z].SetTarget(value.z);
			}
		}

		public void SetTarget(Quaternion target)
		{
			TargetForward = (target * Vector3.forward).normalized;
			TargetUp = (target * Vector3.up).normalized;
		}

		public void SetTarget(Vector3 targetValues)
		{
			TargetLocalAxis = targetValues;
		}

		public Quaternion GetTarget()
		{
			Quaternion globalQuat = GetCurrentGlobalRotation();
		
			Vector3 forward = globalQuat * Vector3.forward;
			Vector3 up = globalQuat * Vector3.up;
			Vector3 right = globalQuat * Vector3.right;

			Quaternion res =
				Quaternion.AngleAxis(TargetLocalAxis.x, right) *
				Quaternion.AngleAxis(TargetLocalAxis.y, up) *
				Quaternion.AngleAxis(TargetLocalAxis.z, forward) *
				globalQuat;

			return res;
		}
		#endregion

		#region VELOCITY
		private Vector3 VelocityLocalAxis
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[LOCAL_AXIS_X].GetVelocity(),
					springValues[LOCAL_AXIS_Y].GetVelocity(),
					springValues[LOCAL_AXIS_Z].GetVelocity());

				return res;
			}
			set
			{
				springValues[LOCAL_AXIS_X].SetVelocity(value.x);
				springValues[LOCAL_AXIS_Y].SetVelocity(value.y);
				springValues[LOCAL_AXIS_Z].SetVelocity(value.z);
			}
		}

		public void AddVelocity(Vector3 eulerTarget)
		{
			const float velocityFactor = 150;
			VelocityLocalAxis += eulerTarget * velocityFactor;
		}

		public void SetVelocity(Vector3 eulerTarget)
		{
			VelocityLocalAxis = eulerTarget;
		}

		public Vector3 GetVelocity()
		{
			Vector3 res = VelocityLocalAxis;
			return res;
		}
		#endregion

		#region CANDIDATE VALUES
		private Vector3 CandidateLocalAxis
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[LOCAL_AXIS_X].GetCandidateValue(),
					springValues[LOCAL_AXIS_Y].GetCandidateValue(),
					springValues[LOCAL_AXIS_Z].GetCandidateValue());

				return res;
			}
			set
			{
				springValues[LOCAL_AXIS_X].SetCandidateValue(value.x);
				springValues[LOCAL_AXIS_Y].SetCandidateValue(value.y);
				springValues[LOCAL_AXIS_Z].SetCandidateValue(value.z);
			}
		}

		private Vector3 CandidateForward
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[FORWARD_X].GetCandidateValue(),
					springValues[FORWARD_Y].GetCandidateValue(),
					springValues[FORWARD_Z].GetCandidateValue());

				return res;
			}
			set
			{
				springValues[FORWARD_X].SetCandidateValue(value.x);
				springValues[FORWARD_Y].SetCandidateValue(value.y);
				springValues[FORWARD_Z].SetCandidateValue(value.z);
			}
		}

		private Vector3 CandidateUp
		{
			get
			{
				Vector3 res = new Vector3(
					springValues[UP_X].GetCandidateValue(),
					springValues[UP_Y].GetCandidateValue(),
					springValues[UP_Z].GetCandidateValue());

				return res;
			}
			set
			{
				springValues[UP_X].SetCandidateValue(value.x);
				springValues[UP_Y].SetCandidateValue(value.y);
				springValues[UP_Z].SetCandidateValue(value.z);
			}
		}

		public override void ProcessCandidateValue()
		{
			Vector3 deltaLocalAxis = CandidateLocalAxis - CurrentLocalAxis;
			const float maxRotationPerFrame = 80f;
			deltaLocalAxis = Vector3.ClampMagnitude(deltaLocalAxis, maxRotationPerFrame);
			CandidateLocalAxis = deltaLocalAxis + CurrentLocalAxis;

			base.ProcessCandidateValue();
		}
		#endregion
	}
}