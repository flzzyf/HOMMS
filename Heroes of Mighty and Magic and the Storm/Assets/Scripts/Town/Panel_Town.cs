﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Town : CustomUI
{
	public Panel_HeroUnits panel_HeroUnits_Inside;
	public Panel_HeroUnits panel_HeroUnits_Outside;

	public Panel_HeroPortrait panel_HeroPortrait_Inside;
	public Panel_HeroPortrait panel_HeroPortrait_Outside;

	public SliderItem_Town sliderItem_Town;

	public Panel_Resources panel_Resources;

	//设置城镇
	public void Set(Town _town)
	{
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

		//进入城镇时，设置显示的城镇项页数和旅行界面选择的页数一样
		int index = SliderItemManager.instance.sliderItem_town.currentPages + Panel_TownItem.highlightedTownItem.index;
		sliderItem_Town.currentPages = index;
		sliderItem_Town.UpdateItems(index);
		//取消高亮之前高亮项，然后高亮第一个
		Panel_TownItem.highlightedTownItem.Highlight(false);
		sliderItem_Town.items[0].Highlight(true);

		//设置资源
		panel_Resources.Set(GameManager.currentPlayer.resources);
	}

	//退出
	public override void Quit()
	{
		//隐藏这个界面
		base.Quit();

		//进入冒险界面
		UIManager.instance.Enter("travel");
	}
}
