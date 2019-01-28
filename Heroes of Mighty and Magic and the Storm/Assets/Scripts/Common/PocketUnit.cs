using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PocketUnit
{
	public UnitType type;
	public int num;

	public PocketUnit(UnitType _type, int _num)
	{
		type = _type;
		num = _num;
	}
}

static class PocketUnitExtension
{
	//向单位组加入单位，返回成功与否
	public static bool AddUnit(this PocketUnit[] _units, UnitType _type, int _num)
	{
		for (int i = 0; i < _units.Length; i++)
		{
			//是要加入的单位类型则叠加，是空则新增
			if(_units[i].type == _type)
			{
				_units[i].num += _num;

				return true;
			}
			else if(_units[i].type == null)
			{
				_units[i].type = _type;
				_units[i].num = _num;

				return true;
			}
		}

		return false;
	}
}