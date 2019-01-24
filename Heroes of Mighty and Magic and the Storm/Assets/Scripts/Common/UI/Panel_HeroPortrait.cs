using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_HeroPortrait : MonoBehaviour
{
	//头像图、背景
	public Image image_portrait;
	public Image image_bg;
	//等级框
	public GameObject[] levelBorders;
	//当前段位
	int currentRank = 0;

	//设置英雄
	public void Set(Hero _hero)
	{
		//设置头像和背景图
		image_portrait.sprite = _hero.heroType.icon;
		image_bg.sprite = _hero.heroType.race.sprite_bg;

		//段位序号0-5
		int rank = Mathf.Min(_hero.level / 3, levelBorders.Length - 1);
		//如果没显示正确段位框
		if(currentRank != rank)
		{
			//显示段位框
			levelBorders[currentRank].SetActive(false);
			levelBorders[rank].SetActive(true);

			currentRank = rank;
		}
	}
}
