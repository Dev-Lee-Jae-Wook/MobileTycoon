using Sirenix.OdinInspector;
using UnityEngine;

namespace EverythingStore.Gacha
{
	[CreateAssetMenu(fileName = "newGachaProbabilityData", menuName = "CustomData/Box/GachaProbabilityData")]
	public class GachaProbabilityData: ScriptableObject
	{
		[Range(0.0f, 1.0f)][SerializeField] private float _probabilityNormal;
		[Range(0.0f, 1.0f)][SerializeField] private float _probabilityRera;
		[Range(0.0f, 1.0f)][SerializeField] private float _probabilityUnique;

		public float ProbabilityRera => _probabilityRera;
		public float ProbabilityUnique => _probabilityUnique;
		public float ProbabilityNormal => _probabilityNormal;

		[Button("Update ASC")]
		private void UpdateASC()
		{
			float probability = 1.0f - _probabilityNormal;
			if (_probabilityRera < probability)
			{
				probability -= _probabilityRera;
			}
			else
			{
				_probabilityRera = probability;
				probability = 0.0f;
			}

			_probabilityUnique = probability;
		}

		[Button("Update DESC")]
		private void UpdateDESC()
		{
			float probability = 1.0f - _probabilityUnique;
			if (_probabilityRera < probability)
			{
				probability -= _probabilityRera;
			}
			else
			{
				_probabilityRera = probability;
				probability = 0.0f;
			}

			_probabilityNormal = probability;
		}
	} 
}
