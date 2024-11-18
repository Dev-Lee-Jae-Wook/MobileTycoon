using EverythingStore.Actor.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.BoxBox
{
	public class BoxOrderItem : MonoBehaviour
	{
		#region Field
		[SerializeField] private BoxOrder _order;
		[SerializeField] private BoxOrderData _data;
		[SerializeField] private int _cost;
		[SerializeField] private TMP_Text _nameText;
		[SerializeField] private TMP_Text _costText;
		private Button _button;
		private Wallet _wallet;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region Public Method
		public void Init(Wallet palyerWallet)
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(TryBuyItem);
			_wallet = palyerWallet;

			_nameText.text = $"{_data.Type} x {_data.Amount}";
			_costText.text = $"Money : {_cost}";
		}
		#endregion

		#region Private Method
		private void OrderBoxData()
		{
			_order.AddOrderData(_data);
		}

		private void TryBuyItem()
		{
			if(_wallet.CanSubstactMoney(_cost) == false)
			{
				Debug.Log("구매 불가");
				return;
			}

			_wallet.SubtractMoney(_cost);
			OrderBoxData();
		}
		#endregion

		#region Protected Method
		#endregion



	}
}
