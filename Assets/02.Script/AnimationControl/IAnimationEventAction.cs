using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAnimationEventAction
    {
        /// <summary>
        /// 캐릭터가 앉기 이벤트
        /// </summary>
        event Action OnAnimationSitdown;


        /// <summary>
        /// 캐릭터 일어나기
        /// </summary>
        event Action OnAnimationSitup;
    }
}
