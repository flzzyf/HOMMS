using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Town : CustomUI
{
    public Panel_HeroUnits panel_HeroUnits_Inside;
    public Panel_HeroUnits panel_HeroUnits_Outside;

    public Panel_HeroPortrait panel_HeroPortrait_Inside;
    public Panel_HeroPortrait panel_HeroPortrait_Outside;

    public SliderItemManager_Town sliderItemManager_Town;

    public Panel_Resources panel_Resources;

    public static Town currentTown;

    //设置城镇
    public void Set(Town _town)
    {
        currentTown = _town;

        //如果城内有英雄，或者有驻军，设置单位栏和头像
        if (_town.hero_inside != null)
        {
            panel_HeroUnits_Inside.Set(_town.hero_inside.pocketUnits);
            panel_HeroPortrait_Inside.Set(_town.hero_inside);
        }
        else if (_town.townUnits != null && _town.townUnits.Length > 0)
        {
            panel_HeroUnits_Inside.Set(_town.townUnits);
            panel_HeroPortrait_Inside.Clear();
        }
        else
        {
            panel_HeroUnits_Inside.Clear();
            panel_HeroPortrait_Inside.Clear();
        }

        //城外英雄
        if (_town.hero_outside != null)
        {
            panel_HeroUnits_Outside.Set(_town.hero_outside.pocketUnits);
            panel_HeroPortrait_Outside.Set(_town.hero_outside);
        }
        else
        {
            panel_HeroUnits_Outside.Clear();
            panel_HeroPortrait_Outside.Clear();
        }
    }

    public override void Enter(bool _quitCurrentUI = false)
    {
        base.Enter(_quitCurrentUI);

        //进入城镇时，设置显示的城镇项页数和旅行界面选择的页数一样
        int index = SliderItemManager.highlightedItemIndex;
        sliderItemManager_Town.MoveToPage(index);


        //设置资源
        panel_Resources.Set(GameManager.instance.localPlayer.resources);

		//播放音乐
		SoundManager.instance.PlayBGM("BGM_Castle");
	}

	//退出
	public override void Quit()
    {
        //隐藏这个界面
        base.Quit();

        //进入冒险界面
        UIManager.instance.Enter("travel");

		//更新选中城镇
		//

		//暂停播放BGM
		SoundManager.instance.StopPlay("BGM_Castle");
	}
}
