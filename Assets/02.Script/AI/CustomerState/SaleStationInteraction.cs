using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.RayInteraction;

namespace EverythingStore.AI.CustomerState
{
	public  class SaleStationInteraction : CustomerStateBase, IFSMState
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		private CustomerRayInteraction _interactionSensor;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_Interaction_SaleStation;
		#endregion

		#region Public Method
		public SaleStationInteraction(Customer owner) : base(owner)
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
				next = FSMStateType.Customer_MoveTo_EnterPoint_Counter;
			}
			return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}
