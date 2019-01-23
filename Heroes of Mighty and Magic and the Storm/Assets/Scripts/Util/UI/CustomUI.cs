using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomUI : MonoBehaviour
{
	public GameObject ui;

	//每个界面的底部信息文本
	public Text text_bottomInfo;

	protected virtual void Start()
	{
		ui.SetActive(false);
	}

	public virtual void Enter()
	{
		ui.SetActive(true);

		//设置底部文本
		UIManager.text_bottomInfo = text_bottomInfo;
	}

	public virtual void Quit()
	{
		ui.SetActive(false);
	}
}
