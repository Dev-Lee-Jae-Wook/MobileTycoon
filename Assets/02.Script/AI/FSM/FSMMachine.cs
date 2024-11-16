using Sirenix.OdinInspector;
using System;
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
		public bool IsRunning { get;  set; } = false;
		[ReadOnly] public FSMStateType CurrentStateType;
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

		/// <summary>
		/// ���� ���¸� ���� ��Ű�� ���ο� ���¿� �����մϴ�.
		/// </summary>
		public void ChangeState(FSMStateType nextState)
		{
			_currentState?.Exit();
			_currentState = _stateTable[nextState];
			_currentState.Enter();
			CurrentStateType = _currentState.Type;
		}
		#endregion

		#region Private Method

		#endregion
	}
}