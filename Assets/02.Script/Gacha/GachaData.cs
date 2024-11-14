using EverythingStore.InteractionObject;
using EverythingStore.Sell;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Gacha
{
	[CreateAssetMenu(fileName ="NewGachaData", menuName ="CustomData/GachaData")]
	public class GachaData : ScriptableObject
	{
		[field:SerializeField] public List<SellObject> SellObjectList { get; private set; }
	}

}