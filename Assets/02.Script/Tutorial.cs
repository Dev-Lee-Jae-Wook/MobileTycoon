using EverythingStore.GameEvent;
using UnityEngine;

namespace EverythingStore
{
    public class Tutorial : Singleton<Tutorial>
    {

		#region Field
		private bool _isPickup = false;
		private bool _isGotoCounter = false;
		private bool _isSpawnMoney = false;
		private bool _isGetMoney = false;
		private bool _isEnterBoxOrder = false;
		private bool _isDeliveryBox = false;
		#endregion

		#region Property
		public bool isPickup => _isPickup;
		public bool isGotoCounter => _isGotoCounter;
		public bool isSpawnMoney => _isSpawnMoney;
		public bool isGetMoney => _isGetMoney;
		public bool isEnterBoxOrder => _isEnterBoxOrder;
		public bool isDeliveryBox => _isDeliveryBox;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			GameEventManager.Instance.OnEvent(GameEventType.Tutorial_GameStart);
		}
		#endregion

		#region Public Method
		public void PickUp()
		{
			_isPickup = true;
			GameEventManager.Instance.OnEvent(GameEventType.Totorial_Pickup);
		}

		public void GoToCounter()
		{
			_isGotoCounter = true;
			GameEventManager.Instance.OnEvent(GameEventType.Totorial_Counter);
		}

		public void SpawnMoney()
		{
			_isSpawnMoney = true;
			GameEventManager.Instance.OnEvent(GameEventType.Totorial_Money);
		}

		public void GetMoney()
		{
			_isGetMoney = true;
			GameEventManager.Instance.OnEvent(GameEventType.Totorial_BoxOrder);
		}

		public void EnterBoxOrder()
		{
			_isEnterBoxOrder = true;
			GameEventManager.Instance.OnEvent(GameEventType.Tutorial_EnterBoxOrder);
		}

		public void DeliveryBox()
		{
			_isDeliveryBox = true;
			GameEventManager.Instance.OnEvent(GameEventType.Tutorial_Delivery);
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		protected override void AwakeInit()
		{
		}
		#endregion

	}
}
