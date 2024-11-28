using EverythingStore.BoxBox;
using UnityEngine;

namespace EverythingStore.Delivery
{
	[CreateAssetMenu(fileName = "newBoxData", menuName = "CustomData/BoxData")]
	public class BoxData : ScriptableObject
	{
		[SerializeField] private BoxType _boxType;
		[SerializeField] private Sprite _sprite;
		[Range(0.0f, 1.0f)][SerializeField] private float _probabilityRera;
		[Range(0.0f, 1.0f)][SerializeField] private float _probabilityUnique;
		[SerializeField] private int _cost;

		public Sprite Sprite => _sprite;
		public BoxType BoxType => _boxType;
		public int Cost => _cost;

		public string GetContext()
		{
			float probabilityNoraml = 1.0f - (_probabilityRera + _probabilityUnique);
			int np = ConvertPercent(probabilityNoraml);
			int rp = ConvertPercent(_probabilityRera);
			int up = ConvertPercent(_probabilityUnique);
			return $"Normal : {np}%\nRera : {rp}%\nUnique : {up}%";
		}

		private int ConvertPercent(float percent)
		{
			return (int)Mathf.Round(percent * 100);
		}
	}
}
