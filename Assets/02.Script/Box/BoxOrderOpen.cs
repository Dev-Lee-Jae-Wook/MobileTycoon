using EverythingStore.Actor.Player;
using EverythingStore.GameEvent;
using EverythingStore.InteractionObject;
using TMPro;
using UnityEngine;

namespace EverythingStore.BoxBox
{
	public class BoxOrderOpen : MonoBehaviour, ISwitch
	{
		[SerializeField] private BoxOrder _boxOrder;
		[SerializeField] private DeliveryTruck _truck;
		[SerializeField] private PlayerInput _playerInput;
		[SerializeField] private TMP_Text _display;

		private bool _isInteraction = true;

		private void Start()
		{
			//배달 시작
			_boxOrder.OnOrderDelivery += Delivery;
			//배달 완료
			_truck.OnFinshDelivey += BoxOrderAble;

			_boxOrder.OnClose += () => _playerInput.SetControler(true);

			BoxOrderAble();
		}

		public void SwitchAction()
		{
			if(_isInteraction == false)
			{
				return;
			}

			if(GameEventManager.Instance.GameTarget == GameTargetType.Tutorial_BoxOrder)
			{
				Tutorial.Instance.EnterBoxOrder();
			}

			_boxOrder.Open();
			_playerInput.SetControler(false);
		}

		public void SetInteraction(bool isInteraction)
		{
			_isInteraction = isInteraction;
		}

		private void Delivery()
		{
			SetInteraction(false);
			_display.text = "Box delivery in progress";
		}

		private void BoxOrderAble()
		{
			SetInteraction(true);
			_display.text = "Box orders available";
		}
	}
}