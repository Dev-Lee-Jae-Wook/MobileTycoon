using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.AI
{
	public interface IFSMState
	{
		/// <summary>
		/// 자신의 상태
		/// </summary>
		FSMStateType Type { get; }

		/// <summary>
		/// 상태에 접근시 호출
		/// </summary>
		void Enter();

		/// <summary>
		/// Update시 호출된다.
		/// </summary>
		/// <returns>다음 상태</returns>
		FSMStateType Excute();

		/// <summary>
		/// 상태를 탈출할 때 호출된다.
		/// </summary>
		void Exit();
	}
}
