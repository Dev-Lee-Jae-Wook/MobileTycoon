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
		/// 전의될 수 있는 상태를 담는 테이블이다.
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
			
			//다음 상태가 현재 상태와 다르다면
			if(nextState != _currentState.Type)
			{
				ChangeState(nextState);
			}
		}


		#endregion

		#region Public Method
		/// <summary>
		/// 머신에서 사용되는 상태들을 설정합니다.
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
		/// 머신을 시작합니다.
		/// </summary>
		public void StartMachine(FSMStateType type)
		{
			ChangeState(type);
			IsRunning = true;
		}

		/// <summary>
		/// 머신을 멈춥니다.
		/// </summary>
		public void StopMachine()
		{
			IsRunning = false;
		}
		#endregion

		#region Private Method
		
		/// <summary>
		/// 이전 상태를 종료 시키고 새로운 상태에 접근합니다.
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
