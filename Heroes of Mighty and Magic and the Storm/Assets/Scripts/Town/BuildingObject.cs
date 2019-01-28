using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Building building;

	public Image image;
	public Image image_outline;
	public Outline outline;

	//属性变化时
	void OnValidate()
	{
		if (image == null || building == null || building.icon == null)
			return;

		//设置图像
		image.sprite = building.icon;
		image.rectTransform.sizeDelta = new Vector2(building.icon.texture.width, building.icon.texture.height);
		//设置光晕图像
		image_outline.sprite = building.icon;
		image_outline.rectTransform.sizeDelta = new Vector2(building.icon.texture.width, building.icon.texture.height);

		//HaloStart();
	}

	//光晕开始
	public void HaloStart()
	{
		SetHalo(1);
	}
	//光晕停止
	public void HaloStop()
	{
		SetHalo(0);
	}
	//设置光晕透明度
	void SetHalo(float _alpha)
	{
		Color color = outline.effectColor;
		color.a = _alpha;
		outline.effectColor = color;
	}

	//鼠标进入
	public void OnPointerEnter(PointerEventData eventData)
	{
		HaloStart();
	}
	//鼠标离开
	public void OnPointerExit(PointerEventData eventData)
	{
		HaloStop();
	}
	//鼠标点击
	public void OnPointerClick(PointerEventData eventData)
	{
		//点击招兵建筑
		if(building.type == BuildingType.Unit)
		{
			//获取城内单位组，英雄或者守城单位
			PocketUnit[] units = Panel_Town.currentTown.inTownUnits;
			if (units.AddUnit(building.unitType, 10))
			{
				print("加入成功");
				UIManager.instance.Get("town").GetComponent<Panel_Town>().panel_HeroUnits_Inside.Set(units);
			}
		}
		//点击市政厅，建造
		else if (building.type == BuildingType.Build)
		{
			UIManager.instance.Get("build").Enter();
		}
	}
}
