﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_HeroUI : CustomUI
{
    public Image portrait;
    public Image portraitBG;
    public Text heroName;

    public Panel_MoraleAndLuck moraleAndLuck;

    public Panel_UnitPortrait[] pocketUnits;

    //英雄属性：攻防、法力、知识
    public Text[] text_stats;

    public HeroUI_Skill panel_exp;
    public HeroUI_Skill panel_mana;

    public HeroUI_Skill[] skills;

    void Start()
    {
        //设置单位UI的序号
        for (int i = 0; i < pocketUnits.Length; i++)
        {
            pocketUnits[i].index = i;
        }
    }

    //设置界面英雄
    public void Set(Hero _hero)
    {
        //英雄头像
        portrait.sprite = _hero.heroType.icon;
        portraitBG.sprite = _hero.heroType.race.sprite_bg;

        heroName.text = _hero.heroType.heroName;

        //士气和运气
        moraleAndLuck.SetMorale(_hero.morale);
        moraleAndLuck.SetLuck(_hero.luck);

        //更新单位信息
        for (int i = 0; i < 7; i++)
        {
            if(_hero.pocketUnits[i] != null && _hero.pocketUnits[i].type != null)
                pocketUnits[i].Init(_hero.pocketUnits[i]);
            else
                pocketUnits[i].Clear();
        }

        //设置英雄属性
        text_stats[0].text = _hero.att + "";
        text_stats[1].text = _hero.def + "";
        text_stats[2].text = _hero.power + "";
        text_stats[3].text = _hero.knowledge + "";

        //英雄技能栏
        for (int i = 0; i < _hero.skills.Count; i++)
        {
            skills[i].Set(_hero.skills[i]);
        }
        for (int i = _hero.skills.Count; i < skills.Length; i++)
        {
            skills[i].Clear();
        }

        //经验值和魔法
        panel_mana.text_name.SetText(_hero.mana + "/" + _hero.mana_max);
    }

    public override void Enter()
    {
		base.Enter();

        Set(TravelManager.instance.currentHero);
    }

    public override void Quit()
    {
		base.Quit();

        //退出英雄面板时再次刷新底部英雄信息栏
        Panel_HeroInfo.instance.Set(TravelManager.instance.currentHero);

        //重置选中单位项
        if(Panel_UnitPortrait.selectedPanel != null)
            Panel_UnitPortrait.selectedPanel.Deselect();
    }

    //一键分兵
    public void SmartSplit()
    {
		if (Panel_UnitPortrait.selectedPanel == null)
			return;

		for (int i = 0; i < pocketUnits.Length && Panel_UnitPortrait.selectedPanel.unitNum > 1; i++)
		{
			//单位栏为空则，选中的单位数量-1，在这一栏位创建1个副本
			if (pocketUnits[i].unitType == null)
			{
				Panel_UnitPortrait.selectedPanel.unitNum--;

				pocketUnits[i].Set(Panel_UnitPortrait.selectedPanel.unitType, 1);

				//在真正英雄单位栏创建单位
				PocketUnit unit = new PocketUnit(Panel_UnitPortrait.selectedPanel.unitType, 1);
				TravelManager.instance.currentHero.pocketUnits[i] = unit;
			}
		}
		//更新数量
		Panel_UnitPortrait.selectedPanel.Set(Panel_UnitPortrait.selectedPanel.unitType, Panel_UnitPortrait.selectedPanel.unitNum);
		//取消选中
		Panel_UnitPortrait.selectedPanel.Deselect();
	}
}
