namespace EverythingStore.InteractionObject
{
	public interface IEnterableCustomer
	{
		/// <summary>
		/// ��ȣ �ۿ��� ������ ������Ʈ�� �մ��� ���� �ִ��� Ȯ��
		/// </summary>
		bool IsEnterable();

		void AddEnterMoveCustomer();

		void RemoveEnterMoveCustomer();
	}
}