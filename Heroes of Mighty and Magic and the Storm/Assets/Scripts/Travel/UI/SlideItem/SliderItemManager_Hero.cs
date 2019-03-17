using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItemManager_Hero : SliderItemManager
{
	public override void  ClickHighlightedItem(int _index)
	{
		base.ClickHighlightedItem(_index);

		//设置英雄，进入界面
		UIManager.instance.Get("hero").GetComponent<Panel_HeroUI>().Set(GameManager.currentPlayer.heroes[_index]);
		UIManager.instance.Enter("hero");
	}

	//高亮项，不再需要取消高亮
	public override void Highlight(int _index)
    {
		//如果城镇的高亮项不是-1，就取消然后刷新
		if (highlightedItemType != HighlightedItemType.Hero)
		{
			highlightedItemType = HighlightedItemType.Hero;

			TravelManager.instance.sliderItemManager_town.DishighlightAll();
		}

		base.Highlight(_index);

		//设置当前英雄
		TravelManager.currentHero = GameManager.currentPlayer.heroes[_index];

		//移动镜头
		TravelCamMgr.instance.MoveCamera(TravelManager.currentHero.transform.position);
		//更新右下角英雄信息
		Panel_HeroInfo.instance.Set(TravelManager.currentHero);
	}

	//刷新当前页面的物体
	public override void UpdateItems()
	{
		base.UpdateItems();
	}

	public override int ObjectCount()
	{
		return GameManager.currentPlayer.heroes.Count;
	}

	public override void SetItem(int _index)
	{
		base.SetItem(_index);
		items[_index].GetComponent<Panel_HeroItem>().Set(GameManager.currentPlayer.heroes[currentPages + _index]);
	}

	public override void ClearItem(int _index)
	{
		base.ClearItem(_index);

		items[_index].GetComponent<Panel_HeroItem>().Clear();
	}

	public override bool IsHightlightedItemType()
	{
		return highlightedItemType == HighlightedItemType.Hero;
	}
}
