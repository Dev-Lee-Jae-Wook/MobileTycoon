using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAuctionCustomerAnimationEventAction
    {

		/// <summary>
		/// ĳ���Ͱ� �ɱ� �̺�Ʈ
		/// </summary>
		event Action OnAnimationSitdown;


        /// <summary>
        /// ĳ���� �Ͼ��
        /// </summary>
        event Action OnAnimationSitup;


        /// <summary>
        /// �ڸ����� �ڼ�ġ��
        /// </summary>
        event Action OnAnimationSittingClap;


        /// <summary>
        /// ���ϴ�
        /// </summary>
        event Action OnAnimationResentful;

        /// <summary>
        /// �յ��
        /// </summary>
        event Action OnAnimationRaising;

        /// <summary>
        /// �� �׼� ����
        /// </summary>
        event Action OnReactionEnd;
	}
}
