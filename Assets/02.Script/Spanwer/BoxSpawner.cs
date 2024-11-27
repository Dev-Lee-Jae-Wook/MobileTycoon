using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using UnityEngine;

namespace EverythingStore.Spanwer
{
    public class BoxSpawner : MonoBehaviour
    {
		#region Field
		[SerializeField] private ObjectPoolManger _poolManger;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField]private float _coolTime;

		private Box _box;
		private bool _isCoolTime;
		private float _currentCoolTime;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			SpawnBox();
		}

		private void Update()
		{
			CoolTimeUpdate();
		}
		#endregion

		#region Public Method
		#endregion

		#region Private Method
		private void CoolTimeUpdate()
		{
			//쿨타임이 아니면 반환
			if(_isCoolTime == false)
			{
				return;
			}

			_currentCoolTime -= Time.deltaTime;
			//쿨타임이 지난 경우
			if(_currentCoolTime <= 0.0f)
			{
				SpawnBox();
				_isCoolTime = false;
			}
		}

		private void SpawnBox()
		{
			_box = _poolManger.GetPoolObject(PooledObjectType.Box_Normal).GetComponent<Box>();
			_box.transform.parent = _spawnPoint;
			_box.transform.localPosition = Vector3.zero;
			_box.OnEmtpyBox += DestoryBox;
			_box.Init(_poolManger);
			_box.SetInteraction(true);
		}

		private void DestoryBox()
		{
			_box.GetComponent<PooledObject>().Release();
			SetCollTime();
		}

		/// <summary>
		/// 쿨타임 초기화하고 활성화합니다.
		/// </summary>
		private void SetCollTime()
		{
			_isCoolTime = true;
			_currentCoolTime = _coolTime;
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
