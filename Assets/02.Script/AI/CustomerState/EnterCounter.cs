using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;

namespace EverythingStore.AI.CustomerState
{
	public  class EnterCounter : CustomerStateBase, IFSMState
	{
		#region Field
		private FSMStateType _type;
		private IWaitingLine _waitInteraction;
		private FSMStateType _next;
		#endregion

		#region Property
		public FSMStateType Type => _type;
		#endregion

		#region Public Method
		public EnterCounter(Customer owner, FSMStateType type, IWaitingLine waitInteraction) : base(owner)
		{
			_type = type;
			_waitInteraction = waitInteraction;
		}

		public void Enter()
		{
			if(_waitInteraction.IsWaitingEmpty() == true)
			{
				_next = _waitInteraction.EnterInteraction(owner);
			}
			else
			{
				_next = _waitInteraction.EnterWaitingLine(owner);
			}
		}

		public FSMStateType Excute()
		{
			FSMStateType next = _next;

			return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}
