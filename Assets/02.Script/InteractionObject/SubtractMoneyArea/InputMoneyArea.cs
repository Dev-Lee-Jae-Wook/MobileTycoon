using EverythingStore.Actor.Player;
using EverythingStore.Timer;
using EverythingStore.Upgrad;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class InputMoneyArea : MonoBehaviour
	{
		#region Field
		[SerializeField] InputMoneyAreaUI _ui;
		private int _targetMoney;
		private int _progressMoney;

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
		/// 인자 :  현재 넣은 돈
		/// </summary>
		public event Action<int> OnUpdateMoney;

		public event Action OnCompelte;
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

		public void Initialize(int targetMoney, int progressMoney)
		{
			_targetMoney = targetMoney;
			_progressMoney = progressMoney;
			_ui.Initialize(this, _targetMoney, _progressMoney);
			_isCompelte = _progressMoney >= _targetMoney;

			OnUpdateMoney?.Invoke(_progressMoney);
			if(_isCompelte == true)
			{
				gameObject.SetActive(false);
			}
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

			if(subtractMoney == 0)
			{
				return;
			}

			_player.Wallet.SubtractMoney(subtractMoney);
			_progressMoney += subtractMoney;
			OnUpdateMoney?.Invoke(_progressMoney);

			if (_progressMoney == _targetMoney)
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
