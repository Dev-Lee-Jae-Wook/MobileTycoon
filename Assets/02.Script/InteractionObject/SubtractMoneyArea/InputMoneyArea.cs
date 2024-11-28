using EverythingStore.Actor.Player;
using EverythingStore.Timer;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class InputMoneyArea : MonoBehaviour
	{
		#region Field
		private int _targetMoney;

		[Title("CoolTime")]
		[SerializeField] private float _time;

		[Title("Debug")]
		[SerializeField] private Color _debugColor;
		[SerializeField] private Vector3 _size;

		private LayerMask _detectLayerMask;
		private CoolTime _coolTime;
		private Player _player;

		private int _subtractMoney = 1;
		private bool _isPlayerDown = false;
		private bool _isCompelte = false;
		#endregion

		#region Property
		#endregion

		#region Event
		/// <summary>
		/// 인자 : 남는 돈
		/// </summary>
		public event Action<int> OnUpdateMoney;

		public event Action OnCompelte;

		public event Action<int> OnSetUp;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_player = GameObject.Find("Player").GetComponent<Player>();
			_detectLayerMask = LayerMask.GetMask("Player");
			_coolTime = gameObject.AddComponent<CoolTime>();
			_coolTime.OnComplete += InputPlayerMoney;
		}
		private void OnDrawGizmos()
		{
			Gizmos.color = _debugColor;
			Gizmos.DrawWireCube(transform.position, _size);
		}

		private void Update()
		{
			//플레이어 접촉 시
			if (IsDetectPlayer() == true)
			{

				if (_coolTime.IsRunning == false && _isCompelte == false)
				{
					InputPlayerMoney();
					_coolTime.StartCoolTime(_time);
				}
			}
		}

		#endregion

		#region Public Method
		public void SetUp(int targetMoney)
		{
			_targetMoney = targetMoney;
			OnSetUp?.Invoke(_targetMoney);
			_isCompelte = false;
		}
		#endregion

		#region Private Method
		private bool IsDetectPlayer()
		{
			var hitColliders = Physics.OverlapBox(transform.position, _size * 0.5f, Quaternion.identity, _detectLayerMask);
			if (hitColliders.Length > 0 && hitColliders[0].gameObject == _player.gameObject)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 돈을 추가합니다.
		/// </summary>
		private void InputPlayerMoney()
		{
			int subtractMoney = _subtractMoney;

			if(_player.Wallet.CanSubstactMoney(subtractMoney) == false)
			{
				subtractMoney = _player.Wallet.Money;
			}

			_player.Wallet.SubtractMoney(_subtractMoney);
			_targetMoney -= _subtractMoney;
			OnUpdateMoney?.Invoke(_targetMoney);

			if (_targetMoney == 0)
			{
				_isCompelte = true;
				OnCompelte?.Invoke();
			}
		}


		#endregion

		#region Protected Method
		#endregion
	}
}
