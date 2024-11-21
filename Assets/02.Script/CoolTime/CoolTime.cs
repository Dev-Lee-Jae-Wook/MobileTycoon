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
		public bool IsPlaying { get; private set; } = false;
		#endregion

		#region Event
		public event Action<float> OnStartTime;
		public event Action<float> OnUpdateTime;
		public event Action OnComplete;
		#endregion

		#region UnityCycle
		private void Update()
		{
			if(IsPlaying == true)
			{
				_currentTime -= Time.deltaTime;
				OnUpdateTime?.Invoke(_currentTime);
				if (_currentTime <= 0.0f)
				{
					IsPlaying = false;
					OnComplete?.Invoke();
				}
			}
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 쿨타임을 시작합니다. 쿨타임이 지나면 이벤트가 호출됩니다.
		/// </summary>
		public void StartCoolTime(float coolTime)
		{
			_currentTime = coolTime;
			IsPlaying = true;
			OnStartTime?.Invoke(_currentTime);
		}

		/// <summary>
		/// 쿨타임을 멈춤니다. 이벤트는 호출되지 않습니다.
		/// </summary>
		public void StopCoolTime()
		{
			IsPlaying = false;
		}
		#endregion

	}
}