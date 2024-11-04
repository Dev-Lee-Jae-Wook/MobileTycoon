using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;

namespace EverythingStore.AI.CustomerState
{
	public class GoToCounter : CustomerStateBase, IFSMState
	{
		#region Field
		private Counter _counter;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_GoToCounter;
		#endregion

		#region Public Method
		public GoToCounter(Customer owner) : base(owner)
		{
			_counter = owner.Counter;
		}

		public void Enter()
		{

		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;

			//카운터에 아무도 없는 경우
			if(_counter.IsEmpty() == true)
			{
				_counter.SetCustomer(owner);
				next = FSMStateType.Customer_MoveToCounter;
			}
            else
            {
				_counter.EnterWaitingLine(owner);
				next = FSMStateType.Customer_TriggerWait;
            }

            return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}
