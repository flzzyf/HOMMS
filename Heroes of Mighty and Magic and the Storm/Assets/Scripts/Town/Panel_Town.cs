using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Town : CustomUI
{
	public Panel_HeroUnits panel_HeroUnits_Inside;
	public Panel_HeroUnits panel_HeroUnits_Outside;

	public Panel_HeroPortrait panel_HeroPortrait_Inside;
	public Panel_HeroPortrait panel_HeroPortrait_Outside;

	//设置城镇
	public void Set(Town _town)
	{
		//如果城内有英雄，或者有驻军，设置单位栏和头像
		if (_town.hero_inside != null)
		{
			panel_HeroUnits_Inside.Set(_town.hero_inside.pocketUnits);
			panel_HeroPortrait_Inside.Set(_town.hero_inside);
		}
		else if (_town.townUnits != null)
		{
			panel_HeroUnits_Inside.Set(_town.townUnits);
			panel_HeroPortrait_Inside.Clear();
		}

		//城外英雄
		if (_town.hero_outside != null)
		{
			panel_HeroUnits_Outside.Set(_town.hero_outside.pocketUnits);
			panel_HeroPortrait_Outside.Set(_town.hero_outside);
		}
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
