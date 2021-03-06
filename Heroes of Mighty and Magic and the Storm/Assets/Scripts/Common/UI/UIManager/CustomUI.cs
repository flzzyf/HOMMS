﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomUI : MonoBehaviour
{
	public GameObject ui;

	//每个界面的底部信息文本
	public Text text_bottomInfo;

	//当前UI
	public static CustomUI currentUI;

	protected virtual void Awake()
	{
		ui.SetActive(false);
	}

	//进入
	public virtual void Enter(bool _quitCurrentUI = false)
	{
		//退出当前UI
		if (_quitCurrentUI && currentUI != null)
		{
			currentUI.Quit();
		}

		currentUI = this;

		ui.SetActive(true);

		//设置底部文本
		if (text_bottomInfo != null)
			UIManager.text_bottomInfo = text_bottomInfo;
	}

	//退出
	public virtual void Quit()
	{
		ui.SetActive(false);
	}
}
