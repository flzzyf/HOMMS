using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItem_Hero : SliderItem<Panel_HeroItem>
{
	void Start()
	{
		//初始化序号
		for (int i = 0; i < items.Length; i++)
		{
			items[i].index = i;
		}
	}

	public override void UpdateItems(int _page)
	{
		base.UpdateItems(_page);

		int heroNumber = GameManager.currentPlayer.heroes.Count;

		//显示/隐藏翻页按钮
		if (currentPages > 0)
			button_pageUp.interactable = true;
		else
			button_pageUp.interactable = false;

		if (currentPages + 5 < heroNumber)
			button_pageDown.interactable = true;
		else
			button_pageDown.interactable = false;

		//显示按钮并更新图像
		int numberToShow = Mathf.Min(heroNumber - currentPages, 5);
		for (int i = 0; i < numberToShow; i++)
		{
			items[i].enabled = true;
			items[i].Set(GameManager.currentPlayer.heroes[currentPages + i]);
		}
		for (int i = numberToShow; i < 5; i++)
		{
			items[i].Clear();
			items[i].enabled = false;
		}
	}
}
