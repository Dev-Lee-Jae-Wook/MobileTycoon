using EverythingStore.InteractionObject;
using UnityEngine;

namespace EverythingStore.BoxBox
{
	public class BoxOrderOpen : MonoBehaviour, ISwitch
	{
		[SerializeField] private BoxOrder _boxOrder;
		[SerializeField] private DeliveryTruck _truck;

		private bool _isInteraction = true;

		private void Start()
		{
			//오더를 넣으면 상호 작용 비 활성화
			_boxOrder.OnOrderDelivery += () => SetInteraction(false);
			//오더를 넣으면 상호 작용 활성화
			_truck.OnFinshDelivey += () => SetInteraction(true);
		}

		public void SwitchAction()
		{
			if(_isInteraction == false)
			{
				return;
			}

			_boxOrder.Open();
		}

		public void SetInteraction(bool isInteraction)
		{
			_isInteraction = isInteraction;
		}
	}
}