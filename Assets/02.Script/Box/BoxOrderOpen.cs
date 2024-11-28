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
			//������ ������ ��ȣ �ۿ� �� Ȱ��ȭ
			_boxOrder.OnOrderDelivery += () => SetInteraction(false);
			//������ ������ ��ȣ �ۿ� Ȱ��ȭ
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