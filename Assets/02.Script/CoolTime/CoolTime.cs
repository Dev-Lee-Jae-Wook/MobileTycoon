using System;
using UnityEngine;

namespace EverythingStore.Timer
{
	public class CoolTime:MonoBehaviour
	{
		#region Field
		private float _currentTime;
		#endregion

		#region Property
		public bool IsRunning { get; private set; } = false;
		#endregion

		#region Event
		public event Action<float> OnStartTime;
		public event Action<float> OnUpdateTime;
		public event Action OnComplete;
		#endregion

		#region UnityCycle
		private void Update()
		{
			if(IsRunning == true)
			{
				_currentTime -= Time.deltaTime;
				OnUpdateTime?.Invoke(_currentTime);
				if (_currentTime <= 0.0f)
				{
					IsRunning = false;
					OnComplete?.Invoke();
				}
			}
		}
		#endregion

		#region Public Method
		/// <summary>
		/// ��Ÿ���� �����մϴ�. ��Ÿ���� ������ �̺�Ʈ�� ȣ��˴ϴ�.
		/// </summary>
		public void StartCoolTime(float coolTime)
		{
			_currentTime = coolTime;
			IsRunning = true;
			OnStartTime?.Invoke(_currentTime);
		}

		/// <summary>
		/// ��Ÿ���� ����ϴ�. �̺�Ʈ�� ȣ����� �ʽ��ϴ�.
		/// </summary>
		public void StopCoolTime()
		{
			IsRunning = false;
		}
		#endregion

	}
}