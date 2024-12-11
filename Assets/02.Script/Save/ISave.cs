using UnityEngine;

namespace EverythingStore.Save
{
	public interface ISave
	{
		string SaveFileName { get; }

		/// <summary>
		/// Save File�� ���� ��� ȣ�� �˴ϴ�.
		/// </summary>
		void InitSaveData();

		/// <summary>
		/// ������ �������ּ���.
		/// </summary>
		void Save();
	}
}