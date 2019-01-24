using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_HeroInfo : Singleton<Panel_HeroInfo>
{
	public Panel_HeroPortrait panel_heroPortrait;

    public Text heroName;

    public Panel_PocketUnit[] pocketUnits;

    //英雄属性：攻防、法力、知识
    public Text[] text_stats;

    public Text text_mana;

    //更新英雄信息面板
    public void Set(Hero _hero)
    {
		//设置英雄头像信息
		panel_heroPortrait.Set(_hero);

		//设置英雄名称文本
        heroName.text = _hero.heroType.heroName;

        //更新英雄携带单位信息
        for (int i = 0; i < 7; i++)
        {
			if (_hero.pocketUnits[i] != null && _hero.pocketUnits[i].type != null)
				pocketUnits[i].Set(_hero.pocketUnits[i]);
            else
                pocketUnits[i].Clear();
        }

        //设置英雄属性
        text_stats[0].text = _hero.att + "";
        text_stats[1].text = _hero.def + "";
        text_stats[2].text = _hero.power + "";
        text_stats[3].text = _hero.knowledge + "";

        //魔法值
        text_mana.text = _hero.mana + "";
    }
}
