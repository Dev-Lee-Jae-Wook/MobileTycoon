namespace EverythingStore.InteractionObject
{
	public interface IEnterableCustomer
	{
		/// <summary>
		/// 상호 작용이 가능한 오브젝트에 손님이 꽉차 있는지 확인
		/// </summary>
		bool IsEnterable();

		void AddEnterMoveCustomer();

		void RemoveEnterMoveCustomer();
	}
}