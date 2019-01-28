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

		//刷新底部英雄信息栏
		//Panel_HeroInfo.instance.Set(PlayerManager.instance.players[GameManager.player].heroes[SliderItemManager_Hero.highlightedItemIndex]);

		//设置资源
		panel_Resources.Set(GameManager.currentPlayer.resources);
	}

	public override void Quit()
	{
		base.Quit();

		//隐藏旅行模式物体
		parent_travelObject.SetActive(false);
	}
}
