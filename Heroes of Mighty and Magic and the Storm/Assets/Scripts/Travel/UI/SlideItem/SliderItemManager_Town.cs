using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItemManager_Town : SliderItemManager
{
	public override void ClickHighlightedItem(int _index)
	{
		base.ClickHighlightedItem(_index);

		//当前高亮的是这个，则进入城镇界面
		UIManager.instance.Get("town").GetComponent<Panel_Town>().Set(GameManager.instance.localPlayer.towns[_index]);
		UIManager.instance.Enter("town");
	}

	//高亮项
	public override void Highlight(int _index)
    {
		//如果城镇的高亮项不是-1，就取消然后刷新
		if (highlightedItemType != HighlightedItemType.Town)
		{
			highlightedItemType = HighlightedItemType.Town;

			TravelManager.instance.sliderItemManager_hero.DishighlightAll();
		}

		base.Highlight(_index);

		//移动镜头
		TravelCamMgr.instance.MoveCamera(GameManager.instance.localPlayer.towns[
			highlightedItemIndex - currentPages].transform.position);
	}

	public override int ObjectCount()
	{
		return GameManager.instance.localPlayer.towns.Count;
	}

	public override void SetItem(int _index)
	{
		base.SetItem(_index);

		items[_index].GetComponent<Panel_TownItem>().Set(GameManager.instance.localPlayer.towns[currentPages + _index]);
	}

	public override void ClearItem(int _index)
	{
		base.ClearItem(_index);

		items[_index].GetComponent<Panel_TownItem>().Clear();
	}

	public override bool IsHightlightedItemType()
	{
		return highlightedItemType == HighlightedItemType.Town;
	}
}