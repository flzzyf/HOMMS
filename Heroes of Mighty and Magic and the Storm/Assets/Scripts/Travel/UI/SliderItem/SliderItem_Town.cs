using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItem_Town : SliderItem<Panel_TownItem>
{
	public override void UpdateItems(int _page)
	{
		base.UpdateItems(_page);

		int num = GameManager.currentPlayer.towns.Count;

		//显示/隐藏翻页按钮
		if (currentPages > 0)
			button_pageUp.interactable = true;
		else
			button_pageUp.interactable = false;

		if (currentPages + 5 < num)
			button_pageDown.interactable = true;
		else
			button_pageDown.interactable = false;

		//显示按钮并更新图像
		int numberToShow = Mathf.Min(num - currentPages, 5);
		for (int i = 0; i < numberToShow; i++)
		{
			items[i].enabled = true;
			items[i].Set(GameManager.currentPlayer.towns[currentPages + i]);
		}
		for (int i = numberToShow; i < 5; i++)
		{
			items[i].Clear();
			items[i].enabled = false;
		}
	}
}