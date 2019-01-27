using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building")]
public class Building : ScriptableObject
{
	//图标
	public Sprite icon;
	//费用
	public HOMMResource cost;
	//需求的前置建筑
	public Building[] requirement;
	//建筑类型
	public BuildingType type;

	//获取建筑
	public static Building Get(string _name)
	{
		Building[] objects = Resources.LoadAll<Building>("ScriptableObject/Building/Instance");
		foreach (var item in objects)
		{
			if (item.name == _name)
				return item;
		}

		return null;
	}
}

public enum BuildingType { Null, Unit, Magic, Build, Castle, Blacksmith, Tavern}

[System.Serializable]
public struct HOMMResource
{
	public int gold, wood, ore, mercury, sulfur, crystal, gem;
}
