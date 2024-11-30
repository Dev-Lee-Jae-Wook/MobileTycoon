using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace EverythingStore.Upgrad {
	public class UpgreadItem : MonoBehaviour
	{
		#region Field
		[Title("UI Component")]
		[SerializeField] private Image _icon;
		[SerializeField] private TMP_Text _name;
		[SerializeField] private TMP_Text _description;
		[SerializeField] private TMP_Text _cost;
		[SerializeField] private Button _upgradButton;

		[Title("Player Upgrad Data")]

		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public void SetUp(Sprite icon, string name, string description, string cost,Action onUpgrad)
		{
			_icon.sprite = icon;
			_name.text = name;
			_description.text = description;
			_cost.text = cost;
			_upgradButton.onClick.AddListener(()=>onUpgrad());
		}

		public void UpdateItem(string description, string cost)
		{
			_description.text = description;
			_cost.text = cost;
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}
