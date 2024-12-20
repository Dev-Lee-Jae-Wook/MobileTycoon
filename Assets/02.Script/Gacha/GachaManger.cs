using EverythingStore.AssetData;
using EverythingStore.Sell;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Gacha
{
    public class GachaManger : MonoBehaviour
    {
		public static GachaManger Instance { get; private set; }

		#region Field
		[SerializeField] private GachaData _spawnableData;
		private Dictionary<SellObjectRank, List<SellObject>> _rankSellObjectList = new();
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			SetUpRankSpawnData();
			Instance = this;
		}
		#endregion

		#region Public Method
		public SellObject Gacha(GachaProbabilityData probabilityData)
		{
			var rank = RandomRank(probabilityData);
			var spawnList = _rankSellObjectList[rank];
			int randomIndex = Random.Range(0, spawnList.Count);
			return spawnList[randomIndex];
		}
		#endregion

		#region Private Method
		private void SetUpRankSpawnData()
		{
			_rankSellObjectList.Add(SellObjectRank.Normal, new List<SellObject>());
			_rankSellObjectList.Add(SellObjectRank.Rare, new List<SellObject>());
			_rankSellObjectList.Add(SellObjectRank.Unique, new List<SellObject>());

			foreach (var item in _spawnableData.SellObjectList)
			{
				switch (item.Rank)
				{
					case SellObjectRank.Normal:
						_rankSellObjectList[SellObjectRank.Normal].Add(item);
						break;
					case SellObjectRank.Rare:
						_rankSellObjectList[SellObjectRank.Rare].Add(item);
						break;
					case SellObjectRank.Unique:
						_rankSellObjectList[SellObjectRank.Unique].Add(item);
						break;
				}
			}
		}

		private SellObjectRank RandomRank(GachaProbabilityData probabilityData)
		{
			if(SucessRandom(probabilityData.ProbabilityUnique) == true)
			{
				return SellObjectRank.Unique;
			}
			else if(SucessRandom(probabilityData.ProbabilityRera) == true)
			{
				return SellObjectRank.Rare;
			}

			return SellObjectRank.Normal;
		}

		private bool SucessRandom(float probability)
		{
			return Random.Range(0.0f, 1.0f) <= probability;
		}

		#endregion

		#region Protected Method
		#endregion
	}
}