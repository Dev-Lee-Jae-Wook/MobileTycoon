using EverythingStore.BoxBox;
using EverythingStore.Gacha;
using UnityEngine;

namespace EverythingStore.Delivery
{
	[CreateAssetMenu(fileName = "newBoxData", menuName = "CustomData/BoxData")]
	public class BoxData : ScriptableObject
	{
		[SerializeField] private BoxType _boxType;
		[SerializeField] private Sprite _sprite;
		[SerializeField] private GachaProbabilityData _gachaProbabilityData;
		[SerializeField] private int _cost;

		public Sprite Sprite => _sprite;
		public BoxType BoxType => _boxType;
		public int Cost => _cost;

		public string GetContext()
		{
			int np = ConvertPercent(_gachaProbabilityData.ProbabilityNormal);
			int rp = ConvertPercent(_gachaProbabilityData.ProbabilityRera);
			int up = ConvertPercent(_gachaProbabilityData.ProbabilityUnique);
			return $"Normal : {np}%\nRera : {rp}%\nUnique : {up}%";
		}

		private int ConvertPercent(float percent)
		{
			return (int)Mathf.Round(percent * 100);
		}
	}
}
