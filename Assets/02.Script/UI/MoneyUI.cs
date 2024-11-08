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
		private void Awake()
		{
			UpdateMoney(_player.Money);
			_player.OnMoneyChange += UpdateMoney;
		}
		#endregion


		#region Private Method
		private void UpdateMoney(int money)
		{
			_text.text = $"Money : {money:D5}";
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
