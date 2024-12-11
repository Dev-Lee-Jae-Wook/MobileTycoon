using EverythingStore.Timer;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EverythingStore.Save
{
	public class SaveManager : Singleton<SaveManager>
	{
		#region Field
		[ReadOnly][SerializeField] private List<ISave> _saveList = new();
		[SerializeField] private float _autoSaveCoolTime;

		private CoolTime _coolTimer;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle

		private void Start()
		{
			_coolTimer.StartCoolTime(_autoSaveCoolTime);
		}
		#endregion

		#region Public Method
		public void RegisterSave(ISave saveData)
		{
			_saveList.Add(saveData);
		}

		#endregion

		#region Private Method
		private void AllSave()
		{
			foreach (var saveItem in _saveList)
			{
				saveItem.Save();
			}

			Debug.Log($"[Save] Auto Save");
			_coolTimer.StartCoolTime(_autoSaveCoolTime);
		}



		#endregion

		#region Protected Method
		protected override void AwakeInit()
		{
			_coolTimer = gameObject.AddComponent<CoolTime>();
			_coolTimer.OnComplete += AllSave;
		}
		#endregion

	}
}
