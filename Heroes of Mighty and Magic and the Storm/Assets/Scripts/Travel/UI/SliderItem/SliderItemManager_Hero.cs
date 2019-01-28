using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItemManager_Hero : SliderItemManager<Panel_HeroItem>
{
	void Start()
	{
		//初始化序号
		for (int i = 0; i < items.Length; i++)
		{
			items[i].index = i;
			items[i].onClick += ItemClicked;
		}
	}

	//项被点击
	void ItemClicked(int _index)
	{
		int index = currentPages + _index;

		//如果当前高亮项不是这个
		if (highlightedItemIndex != index)
		{
			//如果当前有高亮项，则取消高亮
			if (highlightedItemIndex != -1)
				Highlight(highlightedItemIndex - currentPages, false);

			//高亮英雄
			TravelManager.instance.HighlightHero(GameManager.currentPlayer.heroes[index]);
			Highlight(index);
		}
		else
		{
			//当前高亮的是这个，则设置英雄，进入界面
			UIManager.instance.Get("hero").GetComponent<Panel_HeroUI>().Set(GameManager.currentPlayer.heroes[index]);
			UIManager.instance.Enter("hero");
		}
	}

	//高亮项
	public void Highlight(int _index, bool _highlight = true)
	{
		items[_index].Highlight(_highlight);

		highlightedItemIndex = _highlight ? _index : -1;
	}

	//翻到某页
	public override void MoveToPage(int _page)
	{
		base.MoveToPage(_page);

		int heroNumber = GameManager.currentPlayer.heroes.Count;
		
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
