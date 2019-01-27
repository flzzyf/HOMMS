using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_HeroUnits : MonoBehaviour
{
	public Panel_UnitPortrait[] panel_UnitPortraits;

	void Start()
	{
		//初始化序号
		for (int i = 0; i < panel_UnitPortraits.Length; i++)
		{
			panel_UnitPortraits[i].index = i;
		}
	}

	public void Set(PocketUnit[] _units)
	{
		//更新单位信息
		for (int i = 0; i < 7; i++)
		{
			if (_units[i] != null && _units[i].type != null)
				panel_UnitPortraits[i].Init(_units[i]);
			else
				panel_UnitPortraits[i].Clear();
		}
	}
}
