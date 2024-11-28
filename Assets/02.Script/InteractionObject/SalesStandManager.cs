using EverythingStore.InteractionObject;
using EverythingStore.Upgrad;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Manger
{
    public class SalesStandManager : MonoBehaviour
    {
		#region Field
		[SerializeField] private Transform _lockAreaParnet;

		[SerializeField] private int[] _unlockCosts;

		[Title("ReadOnly")]
		[ReadOnly][SerializeField]private SalesStand[] _salesStands;
		 [ReadOnly][SerializeField]private List<UpgradSystemInt> _salesStandUpgradList = new();
		[ReadOnly][SerializeField] private InputMoneyArea[] _lockAreas;

		private int _lockAreaCount = 0;
		private int _salesStandCount = 1;
		#endregion

		#region Property
		public SalesStand[] SalesStands => _salesStands;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_salesStands = GetComponentsInChildren<SalesStand>();
			_lockAreas = _lockAreaParnet.GetComponentsInChildren<InputMoneyArea>();
			foreach (var item in _salesStands)
			{
				_salesStandUpgradList.Add(item.GetComponentInChildren<UpgradSystemInt>());
			}
		}


		private void Start()
		{
			foreach (var item in _salesStandUpgradList)
			{
				item.OnAllComplete += LockArea;
			}

			for (int i = 0; i < _lockAreas.Length; i++)
			{
				_lockAreas[i].SetUp(_unlockCosts[i]);
				_lockAreas[i].OnCompelte += UnlockSalesStand;
				_lockAreas[i].gameObject.SetActive(false);
			}

			SetSaleStand(2, 2);
		}
		#endregion

		#region Public Method
		public SalesStand EnterSalesStand()
		{
			SalesStand saleStand = null;
			for(int i = 0; i < _salesStands.Length && saleStand == null; i++)
			{
				if (_salesStands[i].IsEnterable() == true)
				{
					saleStand = _salesStands[i];
				}
			}
			return saleStand;
		}
		#endregion

		#region Private Method
		private void UnlockSalesStand()
		{
			_salesStands[_salesStandCount].gameObject.SetActive(true);
			_salesStandCount++;
			_lockAreas[_lockAreaCount].gameObject.SetActive(false);
			_lockAreaCount++;
		}

		private void LockArea()
		{
			if (_lockAreaCount < _lockAreas.Length)
			{
				_lockAreas[_lockAreaCount].gameObject.SetActive(true);
			}
		}

		private void SetSaleStand(int openSalesStand, int lastUpgrad)
		{
			int lastIndex = openSalesStand - 1;
			foreach(var item in _salesStands)
			{
				item.gameObject.SetActive(false);
			}

			for (int i = 0; i < openSalesStand; i++)
			{
				_salesStands[i].gameObject.SetActive(true);
			}

			//열린 판매대의 갯수 - 1 개는 전부 다 풀 업그레이드
			for (int i = 0; i < lastIndex; i++)
			{
				_salesStands[i].MaxUpgrad();
				UnlockSalesStand();
			}

			//마지막 판매대 LastUpgrad에 맞추어서 업그레이드
			while (lastUpgrad > 0)
			{
				_salesStandUpgradList[lastIndex].Upgrad();
				lastUpgrad--;
			}
			_salesStandCount = openSalesStand;
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
