using EverythingStore.Actor.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EverythingStore.UI
{
    public class MoneyUI : MonoBehaviour
    {
		#region Field
		[SerializeField] private TMP_Text _text;
		[SerializeField] private Player _player;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			UpdateMoney(_player.Wallet.GetFormatSuffix());
			_player.Wallet.OnUpdateString += UpdateMoney;
		}
		#endregion


		#region Private Method
		private void UpdateMoney(string money)
		{
			_text.text = $"Money : {money}";
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
