using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	public CustomObjectUI[] uis;

	//每个界面的底部信息文本
	public static Text text_bottomInfo;

	//当前界面
	public static CustomUI currentUI;

	//进入UI
	public void Enter(string _name, bool _quitCurrentUI = false)
	{
		//退出当前UI
		if (_quitCurrentUI && currentUI != null)
		{
			Quit();
		}

		currentUI = uis.GetUI(_name);

		currentUI.Enter(_quitCurrentUI);
	}

	//退出当前UI
	public void Quit()
	{
		currentUI.Quit();

		currentUI = null;
	}
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
