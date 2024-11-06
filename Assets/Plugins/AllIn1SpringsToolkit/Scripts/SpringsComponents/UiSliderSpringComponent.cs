using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpringsToolkit
{
	[AddComponentMenu(SpringsToolkitConstants.ADD_COMPONENT_PATH + "Ui Slider Spring Component")]
	public partial class UiSliderSpringComponent : SpringComponent
	{
		public SpringFloat fillAmountSpring = new SpringFloat();

		[SerializeField] private Image autoUpdatedSliderImage;
		[SerializeField, Range(0f, 1f)] private float targetFillAmount;

		protected override void RegisterSprings()
		{
			RegisterSpring(fillAmountSpring);
		}

		protected override void SetCurrentValueByDefault()
		{
			fillAmountSpring.SetCurrentValue(autoUpdatedSliderImage.fillAmount);
		}

		protected override void SetTargetByDefault()
		{
			fillAmountSpring.SetTarget(autoUpdatedSliderImage.fillAmount);
		}

		public void Update()
		{
			if (!initialized) { return; } 

			UpdateFillAmount();
		}

		private void UpdateFillAmount()
		{
			autoUpdatedSliderImage.fillAmount = fillAmountSpring.GetCurrentValue();
		}

		public override bool IsValidSpringComponent()
		{
			bool res = true;

			if(autoUpdatedSliderImage == null)
			{
				AddErrorReason($"{gameObject.name} autoUpdatedSliderImage is null.");
				res = false;
			}

			return res;
		}

		private void SetTargetInternal(float currentHp)
		{
			targetFillAmount = currentHp;
		}

		private void ReachEquilibriumInternal()
		{
			UpdateFillAmount();
		}

		private void SetCurrentValueInternal(float currentValues)
		{
			UpdateFillAmount();
		}

#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			if(autoUpdatedSliderImage == null)
			{
				autoUpdatedSliderImage = GetComponent<Image>();
			}
		}
#endif
	}
}
