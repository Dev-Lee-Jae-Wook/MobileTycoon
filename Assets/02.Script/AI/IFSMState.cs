using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.AI
{
	public interface IFSMState
	{
		/// <summary>
		/// �ڽ��� ����
		/// </summary>
		FSMStateType Type { get; }

		/// <summary>
		/// ���¿� ���ٽ� ȣ��
		/// </summary>
		void Enter();

		/// <summary>
		/// Update�� ȣ��ȴ�.
		/// </summary>
		/// <returns>���� ����</returns>
		FSMStateType Excute();

		/// <summary>
		/// ���¸� Ż���� �� ȣ��ȴ�.
		/// </summary>
		void Exit();
	}
}
