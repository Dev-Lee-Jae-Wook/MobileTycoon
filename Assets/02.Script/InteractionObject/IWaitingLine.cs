using EverythingStore.Actor.Customer;
using EverythingStore.AI;

namespace EverythingStore.InteractionObject
{
	public interface IWaitingLine
	{
		/// <summary>
		/// 대기열이 비었는지를 반환
		/// </summary>
		bool IsWaitingEmpty();

		FSMStateType EnterInteraction(Customer customer);

		FSMStateType EnterWaitingLine(Customer customer);
	}
}