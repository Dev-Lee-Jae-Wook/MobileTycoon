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
		public event Action OnComplete;
		#endregion

		#region UnityCycle
		private void Update()
		{
			if(IsPlaying == true)
			{
				_currentTime -= Time.deltaTime;
				if(_currentTime <= 0.0f)
				{
					IsPlaying = false;
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
			IsPlaying = true;
		}

		/// <summary>
		/// ��Ÿ���� ����ϴ�. �̺�Ʈ�� ȣ����� �ʽ��ϴ�.
		/// </summary>
		public void StopCoolTime()
		{
			IsPlaying = false;
		}
		#endregion

	}
}