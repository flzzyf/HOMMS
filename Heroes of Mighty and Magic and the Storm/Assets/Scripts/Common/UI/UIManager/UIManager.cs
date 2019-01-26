using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	public CustomObjectUI[] uis;

	//每个界面的底部信息文本
	public static Text text_bottomInfo;
}

[System.Serializable]
public class CustomObjectUI : CustomObject<CustomUI> { }

static class CusomUIExtensions
{
	public static CustomUI GetUI (this CustomObject<CustomUI>[] _objs, string _name)
	{
		foreach (var item in _objs)
		{
			if (item.name == _name)
				return item.obj;
		}

		return null;
	}
}
