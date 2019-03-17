using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Travel : CustomUI
{
    public Panel_Resources panel_Resources;

    public GameObject parent_travelObject;

    public override void Enter(bool _quitCurrentUI = false)
    {
        base.Enter(_quitCurrentUI);

        //显示旅行模式物体
        parent_travelObject.SetActive(true);

		//进入旅行界面，刷新右下角信息栏

		//刷新底部英雄信息栏
		//if (SliderItemManager_Hero.highlightedItemIndex != -1)
		//{
		//	Panel_HeroInfo.instance.Set(TravelManager.currentHero);
		//}

		//设置资源
		panel_Resources.Set(GameManager.currentPlayer.resources);

		//播放BGM
		SoundManager.instance.PlayBGM("BGM_Grass");
    }

    public override void Quit()
    {
        base.Quit();

        //隐藏旅行模式物体
        parent_travelObject.SetActive(false);

		//暂停播放BGM
		SoundManager.instance.StopPlay("BGM_Grass");
	}
}
