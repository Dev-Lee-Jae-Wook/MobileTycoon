using EverythingStore.Actor.Player;
using EverythingStore.Timer;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace EverythingStore.InteractionObject
{
	public class SubtractMoneyArea : MonoBehaviour
	{
		#region Field
		[Title("Target")]
		[SerializeField] private int _targetMoney;
		private int _money = 0;

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
		private Action _onComplet;
		#endregion

		#region Property

		#endregion

		#region Event

		public event Action<int> OnUpdateMoney;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_player = GameObject.Find("Player").GetComponent<Player>();
			_detectLayerMask = LayerMask.GetMask("Player");
			_coolTime = gameObject.AddComponent<CoolTime>();
			_coolTime.OnComplete += AddTargetMoney;
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
				if(_coolTime.IsPlaying == false)
				{
					_coolTime.StartCoolTime(_time);
				}
			}
        }

		#endregion

		#region Public Method
		public void SetupTarget(int targetMoney, Action onComplete)
		{
			_money = 0;
			_targetMoney = targetMoney;
			_isTargetCompelte = false;
			_onComplet = onComplete;
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

		private void AddTargetMoney()
		{
			if(_isTargetCompelte == true)
			{
				return;
			}

			if(_player.Money <= 0)
			{
				return;
			}

			_player.SubtractMoney(_subtractMoney);
			_money += _subtractMoney;
			OnUpdateMoney?.Invoke(_money);
			
			if(_money == _targetMoney)
			{
				_isTargetCompelte = true;
				_onComplet?.Invoke();
			}
		}


		#endregion

		#region Protected Method
		#endregion
	}
}
