using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAuctionCustomerAnimationEventAction
    {

		/// <summary>
		/// 캐릭터가 앉기 이벤트
		/// </summary>
		event Action OnAnimationSitdown;


        /// <summary>
        /// 캐릭터 일어나기
        /// </summary>
        event Action OnAnimationSitup;


        /// <summary>
        /// 자리에서 박수치기
        /// </summary>
        event Action OnAnimationSittingClap;


        /// <summary>
        /// 분하다
        /// </summary>
        event Action OnAnimationResentful;

        /// <summary>
        /// 손들기
        /// </summary>
        event Action OnAnimationRaising;

        /// <summary>
        /// 리 액션 종료
        /// </summary>
        event Action OnReactionEnd;
	}
}
