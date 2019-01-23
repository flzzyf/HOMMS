using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomObject<T>
{
	public string name;
	public T obj;
}

static class CusomColorExtensions
{
	public static Color GetColor(this CustomObject<Color>[] _colors, string _name)
	{
		foreach (var item in _colors)
		{
			if (item.name == _name)
				return item.obj;
		}

		return Color.black;
	}
}

