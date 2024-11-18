using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Optimization
{
	public enum PooledObjectType
	{ 
		Box_Normal,
		Box_Rare,
		Box_Unique,
		Box_Lengendary,

		//Normal
		SellObject_Bucket,
		SellObject_Crap,
		SellObject_Penguin,
		SellObject_Pigeon,
		//Rare
		SellObject_FlamingoTube,
		SellObject_Heart,
		SellObject_JapanCastle,
		SellObject_Scorpion,
		//Unique
		SellObject_PC,
		SellObject_PolarBear,
		SellObject_TRax,
		SellObject_UnicornTube,
		Customer,
		Package,
		Money,
	}
}
