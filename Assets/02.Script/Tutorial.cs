using EverythingStore.GameEvent;
using UnityEngine;

namespace EverythingStore
{
    public class Tutorial : Singleton<Tutorial>
    {

		#region Field
		public bool isPickup = false;
		public bool isGotoCounter = false;
		public bool isSpawnMoney = false;
		public bool isGetMoney = false;
		public bool isEnterBoxOrder =false;
		#endregion

		#region Property
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
