using EverythingStore.GameEvent;
using UnityEngine;

namespace EverythingStore
{
    public class Tutorial : Singleton<Tutorial>
    {

		#region Public Method
		public void PickUp()
		{
			GameEventManager.Instance.OnEvent(GameTargetType.Tutorial_Pickup);
		}

		public void GoToCounter()
		{
			GameEventManager.Instance.OnEvent(GameTargetType.Tutorial_Counter);
		}

		public void SpawnMoney()
		{
			GameEventManager.Instance.OnEvent(GameTargetType.Tutorial_Money);
		}

		public void GetMoney()
		{
			GameEventManager.Instance.OnEvent(GameTargetType.Tutorial_BoxOrder);
		}

		public void EnterBoxOrder()
		{
			GameEventManager.Instance.OnEvent(GameTargetType.Tutorial_EnterBoxOrder);
		}

		public void DeliveryBox()
		{
			GameEventManager.Instance.OnEvent(GameTargetType.Tutorial_Delivery);
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
