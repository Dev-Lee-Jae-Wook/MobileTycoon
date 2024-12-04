using EverythingStore.InputMoney;
using EverythingStore.InteractionObject;
using EverythingStore.Upgrad;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Manager
{
    public class SalesStandManager : MonoBehaviour
    {
		#region Field
		[SerializeField]private SalesStand[] _salesStands;
		[SerializeField] private UnlockArea[] _lockAreas;
		 private List<UpgradSystemInt> _salesStandUpgradList = new();
		#endregion

		#region Property
		public SalesStand[] SalesStands => _salesStands;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			foreach (var item in _salesStands)
			{
				var upgrad = item.GetComponentInChildren<UpgradSystemInt>();
				_salesStandUpgradList.Add(upgrad);
			}
		}


		private void Start()
		{
			foreach (var lockArea in _lockAreas)
			{
				lockArea.gameObject.SetActive(false);
			}

			SetSaleStand(1, 0);
			_salesStandUpgradList[0].OnAllComplete += ()=> ActiveLockArea(0);
			_salesStandUpgradList[1].OnAllComplete += () => ActiveLockArea(1);
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

		private void ActiveLockArea(int index)
		{
			_lockAreas[index].gameObject.SetActive(true);
		}

		private void SetSaleStand(int openSalesStand, int lastUpgrad)
		{
			int lastSalesStandIndex = openSalesStand - 1;
			foreach(var item in _salesStands)
			{
				item.gameObject.SetActive(false);
			}

			for (int i = 0; i < openSalesStand; i++)
			{
				_salesStands[i].gameObject.SetActive(true);
			}

			//열린 판매대의 갯수 - 1 개는 전부 다 풀 업그레이드
			for (int i = 0; i < lastSalesStandIndex; i++)
			{
				_salesStands[i].MaxUpgrad();
			}

			//마지막 판매대 LastUpgrad에 맞추어서 업그레이드
			while (lastUpgrad > 0)
			{
				_salesStandUpgradList[lastSalesStandIndex].Upgrad();
				lastUpgrad--;
			}
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
