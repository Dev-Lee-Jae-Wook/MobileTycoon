using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Sensor;

namespace EverythingStore.AI
{
	public  class CustomerState_SaleStationWait : CustomerStateBase, IFSMState
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		private CustomerInteractionSensor _interactionSensor;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_SaleStationWait;
		#endregion

		#region Public Method
		public CustomerState_SaleStationWait(Customer owner) : base(owner)
		{
			_pickupAndDrop = owner.pickupAndDrop;
			_interactionSensor = owner.Sensor;
		}

		public void Enter()
		{
		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;
			_interactionSensor.RayCastAndInteraction();
			if(_pickupAndDrop.HasPickupObject() == true)
			{
				next = FSMStateType.Customer_MoveToCounter;
			}
			return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}
