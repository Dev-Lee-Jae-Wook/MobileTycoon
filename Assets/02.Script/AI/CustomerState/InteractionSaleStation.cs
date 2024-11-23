using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using EverythingStore.RayInteraction;

namespace EverythingStore.AI.CustomerState
{
	public  class InteractionSaleStation : CustomerStateBase, IFSMState
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		private CustomerRayInteraction _interactionSensor;
		private Counter _counter;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_Interaction_SaleStation;
		#endregion

		#region Public Method
		public InteractionSaleStation(Customer owner, Counter counter) : base(owner)
		{
			_pickupAndDrop = owner.pickupAndDrop;
			_interactionSensor = owner.Sensor;
			_counter = counter;
		}

		public void Enter()
		{
		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;
			if (_counter.IsEnterable() == true)
			{
				_interactionSensor.RayCastAndInteraction();
				if (_pickupAndDrop.HasPickupObject() == true)
				{
					_counter.AddEnterMoveCustomer();
					next = FSMStateType.Customer_MoveTo_EnterPoint_Counter;
				}
			}
				return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}
