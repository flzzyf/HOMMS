using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomColor
{
	public string name;
	public Color color;
}

static class CusomColorExtensions
{
	public static Color GetColor(this CustomColor[] _colors, string _name)
	{
		foreach (var item in _colors)
		{
			if (item.name == _name)
				return item.color;
		}

		return Color.black;
	}
}

