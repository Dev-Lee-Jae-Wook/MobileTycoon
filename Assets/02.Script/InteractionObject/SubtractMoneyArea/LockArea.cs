using EverythingStore.Actor.Player;
using EverythingStore.Timer;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class LockArea : MonoBehaviour
	{
		#region Field
		[Title("Target")]
		[SerializeField] private int _targetMoney;

		[Title("CoolTime")]
		[SerializeField] private float _time;

		[Title("Debug")]
		[SerializeField] private Color _debugColor;
		[SerializeField] private Vector3 _size;

		private LayerMask _detectLayerMask;
		private CoolTime _coolTime;
		private Player _player;

		private int _subtractMoney = 1;

		private bool _isTargetCompelte = false;
		private bool _isPlayerDown = false;
		#endregion

		#region Property
		public int MaxMoney { get; private set; }
		#endregion

		#region Event
		/// <summary>
		/// 인자 : 남는 돈
		/// </summary>
		public event Action<int> OnUpdateMoney;

		/// <summary>
		/// 인자 1 : 현재 레벨
		/// 인자 2 : 목표 돈
		/// </summary>
		public event Action<String,int,int> OnSetupTargetMoney;

		/// <summary>
		/// 플레이어가 접촉했을 때 호출됩니다.
		/// </summary>
		public event Action OnPlayerDown;

		/// <summary>
		/// 플레이어가 접촉을 끝낼 때 호출 됩니다.
		/// </summary>
		public event Action OnPlayerUp;

		public event Action OnCompelte;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_player = GameObject.Find("Player").GetComponent<Player>();
			_detectLayerMask = LayerMask.GetMask("Player");
			_coolTime = gameObject.AddComponent<CoolTime>();
			_coolTime.OnComplete += AddTargetMoney;
			MaxMoney = _targetMoney;
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
				if (_isPlayerDown == false)
				{
					_isPlayerDown = true;
					OnPlayerDown?.Invoke();
				}

				if (_coolTime.IsPlaying == false)
				{
					_coolTime.StartCoolTime(_time);
				}
			}
			else if(_isPlayerDown == true)
			{
				_isPlayerDown = false;
				OnPlayerUp?.Invoke();
			}
		}

		#endregion

		#region Public Method
		public void SetupTarget(string name, int lv, int targetMoney, Action onComplete)
		{
			//lv은 UI에게 넘겨주어야된다.
			_targetMoney = targetMoney;
			_isTargetCompelte = false;
			OnSetupTargetMoney?.Invoke(name, lv, targetMoney);
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
		private void AddTargetMoney()
		{
			if (_isTargetCompelte == true)
			{
				return;
			}

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
				_isTargetCompelte = true;
				OnCompelte?.Invoke();
				gameObject.SetActive(false);
			}
		}


		#endregion

		#region Protected Method
		#endregion
	}
}
