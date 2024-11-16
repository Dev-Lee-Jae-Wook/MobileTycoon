using EverythingStore.Delivery;
using UnityEngine;

namespace EverythingStore.AI.DeliveryTruckState
{
	public class BoxSendToStoreSorage : DeliveryTruckBase, IFSMState
	{
		private float _waitTime = 0.5f;
		private float _time;
		public BoxSendToStoreSorage(DeliveryTruck truck) : base(truck)
		{
		}

		public FSMStateType Type => FSMStateType.DeliveryTruck_BoxSendToStoreage;

		public void Enter()
		{
			truck.Delivery();
			_time = _waitTime;
		}

		public FSMStateType Excute()
		{
			if (_time <= 0.0f)
			{
				return FSMStateType.DeliveryTruck_MoveTo_ExitPoint;
			}

			_time -= Time.deltaTime;

			return Type;
		}

		public void Exit()
		{
			
		}
	}

}
