using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Sensor;

namespace EverythingStore.AI.CustomerState
{
	public  class SaleStationWait : CustomerStateBase, IFSMState
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		private CustomerInteractionSensor _interactionSensor;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_SaleStationWait;
		#endregion

		#region Public Method
		public SaleStationWait(Customer owner) : base(owner)
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
				next = FSMStateType.Customer_GoToCounter;
			}
			return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}
