using EverythingStore.GameEvent;
using EverythingStore.Timer;
using Sirenix.OdinInspector;
using System;
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
			if (GameEventManager.Instance.GameTarget == GameTargetType.EndTarget)
			{
				StartAutoSave();
			}
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 등록시에는 Awake 단계에서 호출해주세요.
		/// </summary>
		/// <param name="saveData"></param>
		public void RegisterSave(ISave saveData)
		{
			_saveList.Add(saveData);
		}
		public void StartAutoSave()
		{
			AllSave();
			_coolTimer.StartCoolTime(_autoSaveCoolTime);
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
