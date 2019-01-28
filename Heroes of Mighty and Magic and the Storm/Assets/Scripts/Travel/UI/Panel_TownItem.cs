using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Panel_TownItem : MonoBehaviour, IPointerClickHandler
{
	public Image image_bg;
	public GameObject border_highlight;
	public GameObject icon_finishBuilding;

	[HideInInspector]
	public int index;

	//被点击事件
	public delegate void ItemClick(int _index);
	public ItemClick onClick;

	public void Set(Town _town)
	{
		image_bg.enabled = true;
		image_bg.sprite = _town.race.sprite_bg;
	}

	//鼠标点击
	public void OnPointerClick(PointerEventData data)
	{
		onClick.Invoke(index);
	}

	//高亮
	public void Highlight(bool _highlight = true)
	{
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
