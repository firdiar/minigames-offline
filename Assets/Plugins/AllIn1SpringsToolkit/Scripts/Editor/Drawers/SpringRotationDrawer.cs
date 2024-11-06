#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using static AllIn1SpringsToolkit.SpringsEditorUtility;

namespace AllIn1SpringsToolkit
{
	public class SpringRotationDrawer : SpringDrawer
	{
		private SpringRotationEditorObject springRotationEditorObject;

		public SpringRotationDrawer(SerializedProperty property, bool isFoldout, bool isDebugger) : 
			base(parentProperty: property, isFoldout: isFoldout, drawClampingArea: false, isDebugger: isDebugger)
		{}

		public SpringRotationDrawer(bool isFoldout) : base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: false, isDebugger: false)
		{}

		public SpringRotationDrawer(bool isFoldout, bool isDebugger) : base(parentProperty: null, isFoldout: isFoldout, drawClampingArea: false, isDebugger: isDebugger)
		{
		}

		public override void RefreshSerializedProperties(SerializedProperty parentProperty)
		{
			base.RefreshSerializedProperties(parentProperty);

			springRotationEditorObject = (SpringRotationEditorObject)springEditorObject;
		}

		protected override SpringEditorObject CreateSpringEditorObjectInstance(SerializedProperty parentProperty)
		{
			SpringEditorObject res = new SpringRotationEditorObject(parentProperty);
			return res;
		}

		protected override void DrawUpdate(ref Rect currentRect)
		{
			springRotationEditorObject.Update = DrawVector3Bool(position: currentRect, label: FIELD_NAME_UPDATE_AXIS, componentsLabels: COMPONENTS_LABELS_XYZ, labelWidth: LabelWidth, vector3Bool: springRotationEditorObject.Update);
		}

		protected override void DrawForce(ref Rect currentRect)
		{
			springRotationEditorObject.ForceEuler = DrawCustomVector3(position: currentRect, label: FIELD_NAME_FORCE, LabelWidth, componentsLabels: COMPONENTS_LABELS_XYZ, vector3: springRotationEditorObject.ForceEuler);
		}

		protected override void DrawDrag(ref Rect currentRect)
		{
			springRotationEditorObject.DragEuler = DrawCustomVector3(position: currentRect, label: FIELD_NAME_DRAG, LabelWidth, componentsLabels: COMPONENTS_LABELS_XYZ, vector3: springRotationEditorObject.DragEuler);
		}

		public override void DrawCurrentValue(ref Rect currentRect, string label, float labelWidth)
		{
			DrawCustomVector3(currentRect, label, labelWidth, COMPONENTS_LABELS_XYZ, springRotationEditorObject.CurrentEuler, threeDecimalsOnly: true);
		}

		protected override void DrawVelocity(ref Rect currentRect)
		{
			DrawCustomVector3(currentRect, FIELD_NAME_VELOCITY, LabelWidth, COMPONENTS_LABELS_XYZ, springRotationEditorObject.VelocityEuler, threeDecimalsOnly: true);
		}

		public override void DrawTarget(ref Rect currentRect, string label, float labelWidth)
		{
			springRotationEditorObject.TargetEuler = DrawCustomVector3(currentRect, label, labelWidth, COMPONENTS_LABELS_XYZ, springRotationEditorObject.TargetEuler);
		}

		protected override void DrawClampingRange(ref Rect currentRect)
		{
		
		}

		protected override void DrawNudgeOperationValues(ref Rect currentRect)
		{
			springRotationEditorObject.OperationValue = DrawCustomVector3(currentRect, FIELD_NAME_OPERATION_VALUE, LabelWidth, COMPONENTS_LABELS_XYZ, springRotationEditorObject.OperationValue);
		}

		protected override void DrawClampTarget(ref Rect currentRect)
		{}

		protected override void DrawClampCurrentValue(ref Rect currentRect)
		{
		
		}

		protected override void DrawStopSpringOnCurrentValueClamp(ref Rect currentRect)
		{
		
		}
	}
}

#endif