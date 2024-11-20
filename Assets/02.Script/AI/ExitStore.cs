using System;

namespace EverythingStore.AI
{
	public class ExitStore : IFSMState
	{
		public FSMStateType Type => FSMStateType.ExitStore;
		private Action _callBack;

		public ExitStore(Action callBack)
		{
			_callBack = callBack;
		}

		public void Enter()
		{
			_callBack?.Invoke();
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