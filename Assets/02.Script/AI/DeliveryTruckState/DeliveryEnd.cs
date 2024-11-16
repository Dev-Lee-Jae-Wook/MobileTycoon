using EverythingStore.Delivery;

namespace EverythingStore.AI.DeliveryTruckState
{
	public class DeliveryEnd : DeliveryTruckBase, IFSMState
	{
		public DeliveryEnd(DeliveryTruck truck) : base(truck)
		{
		}

		public FSMStateType Type => FSMStateType.DeliveryTruck_DeliveryEnd;

		public void Enter()
		{
			truck.FinshDelivery();
		}

		public FSMStateType Excute()
		{
			return Type;
		}

		public void Exit()
		{
			
		}
	}

}
