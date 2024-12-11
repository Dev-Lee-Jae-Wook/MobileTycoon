using UnityEngine;

namespace EverythingStore.Save
{
	public interface ISave
	{
		string SaveFileName { get; }

		/// <summary>
		/// Save File이 없는 경우 호출 됩니다.
		/// </summary>
		void InitSaveData();

		/// <summary>
		/// 저장을 진행해주세요.
		/// </summary>
		void Save();
	}
}