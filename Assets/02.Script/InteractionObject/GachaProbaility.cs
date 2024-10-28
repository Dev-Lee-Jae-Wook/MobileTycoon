using System.Collections.Generic;
using UnityEngine;

//가챠 확률 데이터는 좀 더 개량이 필요합니다.
namespace EverythingStore.InteractionObject
{
	[CreateAssetMenu(fileName = "newGachaProbaility", menuName = "CustomData/GachaProbailityData", order = 0)]
	public class GachaProbaility : ScriptableObject
	{
		//아이템 확률
		[SerializeField] private List<SellObject> _items;

		public SellObject Gacha()
		{
			int rand = Random.Range(0, _items.Count);
			return _items[rand];
		}
	}
}