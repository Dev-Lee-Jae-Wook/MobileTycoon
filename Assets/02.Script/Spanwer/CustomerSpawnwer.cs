using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EverythingStore.Spanwer
{
	public class CustomerSpawner : MonoBehaviour
	{
		#region Field
		[Title("Prefab")]
		[SerializeField] private Customer _prefab;

		[Title("Customer Init Data")]
		[SerializeField] private Counter _counter;
		[SerializeField] private SalesStand _salesStand;
		[SerializeField] private Transform _exitPoint;

		[Title("CoolTime")]
		[SerializeField] private float _coolTime;
		private bool _isCoolTime;
		private float _currentCoolTime;

		[Title("Store Data")]
		[SerializeField] private int _maxCustomer;
		[ReadOnly] public int customerCount;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			StartWaitSpawn();
		}
		#endregion

		#region Public Method
		private void Update()
		{
			CoolTimeUpdate();
		}
		#endregion

		#region Private Method
		/// <summary>
		/// �մ��� �����մϴ�.
		/// </summary>
		private void SpawnCustomer()
		{
			var newCustomer = Instantiate(_prefab);
			newCustomer.transform.position = transform.position;
			newCustomer.Setup(_counter, _salesStand, _exitPoint.position);
			newCustomer.OnExitStore += CustomerDelete;
			customerCount++;

			//�ִ� �մԸ� ���� �ʾҴٸ� ��Ÿ���� ������
			if (IsFullCustomer() == false)
			{
				StartWaitSpawn();
			}
		}

		private void CoolTimeUpdate()
		{
			//��Ÿ���� �ƴϸ� ��ȯ
			if (_isCoolTime == false)
			{
				return;
			}

			_currentCoolTime -= Time.deltaTime;
			//��Ÿ���� ���� ���
			if (_currentCoolTime <= 0.0f)
			{
				_isCoolTime = false;
				SpawnCustomer();
			}
		}

		/// <summary>
		/// ��Ÿ�� �ʱ�ȭ�ϰ� Ȱ��ȭ�մϴ�.
		/// </summary>
		private void StartWaitSpawn()
		{
			_isCoolTime = true;
			_currentCoolTime = _coolTime;
		}

		private void CustomerDelete(GameObject customer)
		{
			bool isFull = IsFullCustomer();

			Destroy(customer);
			customerCount--;

			if(isFull == true)
			{
				StartWaitSpawn();
			}
		}

		private bool IsFullCustomer()
		{
			return customerCount >= _maxCustomer;
		}
		#endregion

	}
}