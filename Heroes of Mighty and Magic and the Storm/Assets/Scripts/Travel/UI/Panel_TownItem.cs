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

	public void Set(Town _town)
	{
		image_bg.sprite = _town.race.sprite_bg;
	}

	//鼠标点击
	public void OnPointerClick(PointerEventData data)
	{
		UIManager.instance.uis.GetUI("town").Enter();
	}
}
