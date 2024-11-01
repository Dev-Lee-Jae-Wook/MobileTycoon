using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.AI
{
    public class FSMMachine : MonoBehaviour
    {
		#region Field
		private IFSMState _currentState;
		/// <summary>
		/// ���ǵ� �� �ִ� ���¸� ��� ���̺��̴�.
		/// </summary>
		private Dictionary<FSMStateType, IFSMState> _stateTable = new();
		#endregion

		#region Property
		public bool IsRunning { get; private set; } = false;
		public FSMStateType CurrentStateType => _currentState.Type;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Update()
		{
			if (IsRunning == false)
			{
				return;
			}

			var nextState =  _currentState.Excute();
			
			//���� ���°� ���� ���¿� �ٸ��ٸ�
			if(nextState != _currentState.Type)
			{
				ChangeState(nextState);
			}
		}


		#endregion

		#region Public Method
		/// <summary>
		/// �ӽſ��� ���Ǵ� ���µ��� �����մϴ�.
		/// </summary>
		public void SetUpState(List<IFSMState> states)
		{
			_stateTable.Clear();
			foreach (var state in states)
			{
				_stateTable[state.Type] = state;
			}
		}

		/// <summary>
		/// �ӽ��� �����մϴ�.
		/// </summary>
		public void StartMachine(FSMStateType type)
		{
			ChangeState(type);
			IsRunning = true;
		}

		/// <summary>
		/// �ӽ��� ����ϴ�.
		/// </summary>
		public void StopMachine()
		{
			IsRunning = false;
		}
		#endregion

		#region Private Method
		
		/// <summary>
		/// ���� ���¸� ���� ��Ű�� ���ο� ���¿� �����մϴ�.
		/// </summary>
		private void ChangeState(FSMStateType nextState)
		{
			_currentState?.Exit();
			_currentState = _stateTable[nextState];
			_currentState.Enter();
		}
		#endregion
	}
}
