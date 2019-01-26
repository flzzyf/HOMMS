using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BottomTextTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public string key;

	//鼠标进入
	public void OnPointerEnter(PointerEventData eventData)
	{
		//设置底部文本
		UIManager.text_bottomInfo.text = LocalizationMgr.instance.GetText(key);
	}
	//鼠标离开
	public void OnPointerExit(PointerEventData data)
	{
		//设置底部文本
		UIManager.text_bottomInfo.text = "";
	}
}
