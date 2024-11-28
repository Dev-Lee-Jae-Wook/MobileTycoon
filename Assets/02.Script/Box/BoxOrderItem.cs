using EverythingStore.Delivery;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.BoxBox
{
	public class BoxOrderItem : MonoBehaviour
	{
		#region Field
		[SerializeField] private BoxData _boxData;
		[SerializeField] private Image _image;
		[SerializeField] private Image _block;
		[SerializeField] private TMP_Text _name;
		[SerializeField] private TMP_Text _cost;
		[SerializeField] private TMP_Text _amount;

		private Button _button;
		#endregion

		#region Property
		public BoxData BoxData => _boxData;

		public int Cost => _boxData.Cost;
		#endregion
		#region Event
		#endregion
		#region UnityCycle
		#endregion

		#region Public Method
		public void Init(Action onClick)
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(()=> onClick());

			Setup();
		}

		public void SetBlock(bool isBlock)
		{
			_button.interactable = !isBlock;
			_block.gameObject.SetActive(isBlock);
		}
		#endregion

		#region Private Method
		[Button("Setup")]
		private void Setup()
		{
			_image.sprite = _boxData.Sprite;
			_name.text = $"{_boxData.BoxType} Box";
			_cost.text = _boxData.Cost.ToString();
			gameObject.name = $"BoxOrder_{_boxData.BoxType}";
			_amount.text = null;
		}
		#endregion

		#region Protected Method
		#endregion



	}
}
