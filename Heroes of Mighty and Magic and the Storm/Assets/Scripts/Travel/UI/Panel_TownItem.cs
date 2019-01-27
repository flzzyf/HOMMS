using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Panel_TownItem : MonoBehaviour, IPointerClickHandler
{
	public Image image_bg;
	public GameObject border_highlight;
	public GameObject icon_finishBuilding;

	[HideInInspector]
	public int index;

	//当前高亮的项
	public static Panel_TownItem highlightedTownItem;

	public void Set(Town _town)
	{
		image_bg.enabled = true;
		image_bg.sprite = _town.race.sprite_bg;
	}

	//鼠标点击
	public void OnPointerClick(PointerEventData data)
	{
		//如果当前没有高亮项，则高亮这个，并结束
		if(highlightedTownItem == null)
		{
			Highlight();

			return;
		}

		//如果高亮的是这个，则进入城镇界面。否则取消高亮项，并高亮这个
		if(highlightedTownItem == this)
		{
            int id = SliderItemManager.instance.sliderItem_town.currentPages + index;
			UIManager.instance.Get("town").GetComponent<Panel_Town>().Set(GameManager.currentPlayer.towns[id]);
			UIManager.instance.Enter("town", true);
		}
		else
		{
			highlightedTownItem.Highlight(false);
			Highlight();
		}
	}

	//高亮
	public void Highlight(bool _highlight = true)
	{
		if(_highlight)
			highlightedTownItem = this;
		else
			highlightedTownItem = null;

		//显示高亮边框
		border_highlight.SetActive(_highlight);
	}
	//清空
	public void Clear()
	{
		image_bg.enabled = false;

		icon_finishBuilding.SetActive(false);
	}
}
