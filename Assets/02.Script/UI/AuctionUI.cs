using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EverythingStore.AuctionSystem
{
    public class AuctionUI : MonoBehaviour
    {
		#region Field
		[SerializeField] private AuctionManger _manger;
		[SerializeField] private TMP_Text _dispaly;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_manger.OnUpdateBidMoney += UpdateMoney;
		}
		#endregion

		#region Public Method
		#endregion

		#region Private Method
		private void UpdateMoney(int money)
		{
			_dispaly.text = money.ToString();
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
