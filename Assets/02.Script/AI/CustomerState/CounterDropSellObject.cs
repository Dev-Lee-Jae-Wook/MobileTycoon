using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Sensor;

namespace EverythingStore.AI.CustomerState
{
	public  class CounterDropSellObject : CustomerStateBase, IFSMState
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		private CustomerInteractionSensor _interactionSensor;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_CounterDropSellObject;
		#endregion

		#region Public Method
		public CounterDropSellObject(Customer owner) : base(owner)
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
			
			if(_pickupAndDrop.HasPickupObject() == false)
			{
				next = FSMStateType.Customer_CounterCaculationWait;
			}

			return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}
