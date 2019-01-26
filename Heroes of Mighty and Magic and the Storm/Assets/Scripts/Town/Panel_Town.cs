using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Town : CustomUI
{
	//设置城镇
	public void Set(Town _town)
	{

	}

	//退出
    public override void Quit()
	{
		//隐藏这个界面
		base.Quit();

		//进入冒险界面
		UIManager.instance.Enter("travel");
	}
}
