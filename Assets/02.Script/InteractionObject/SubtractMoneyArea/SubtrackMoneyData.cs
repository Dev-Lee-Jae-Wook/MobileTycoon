using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	[CreateAssetMenu(fileName = "NewSubtrackMoneyData", menuName = "CustomData/SubtrackMoneyData")]
	public class SubtrackMoneyData : ScriptableObject
	{
		[field:SerializeField] public List<int> TargetMoneyList { get; private set; }
	}
}