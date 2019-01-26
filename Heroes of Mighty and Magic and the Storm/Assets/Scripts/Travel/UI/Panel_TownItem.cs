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
			UIManager.instance.Enter("town", true);
		}
		else
		{
			highlightedTownItem.Dishighlight();
			Highlight();
		}
	}

	//高亮
	public void Highlight()
	{
		highlightedTownItem = this;

		//显示高亮边框
		border_highlight.SetActive(true);
	}
	//取消高亮
	public void Dishighlight()
	{
		highlightedTownItem = null;

		//隐藏高亮边框
		border_highlight.SetActive(false);
	}
}
