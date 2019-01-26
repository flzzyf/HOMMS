using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//旅行界面的英雄和城镇项管理器
public class SliderItemManager : Singleton<SliderItemManager>
{
	public SliderItem_Hero sliderItem_hero;
	public SliderItem_Town sliderItem_town;
}

[System.Serializable]
public class SliderItem<T> : MonoBehaviour
{
	public T[] items;
	public Button button_pageUp;
	public Button button_pageDown;

	[HideInInspector]
	public int currentPages;

	//更新按钮项
	public virtual void UpdateItems(int _page)
	{

	}

	//翻页
	public void PageDown()
	{
		currentPages++;

		UpdateItems(currentPages);
	}
	public void PageUp()
	{
		currentPages--;

		UpdateItems(currentPages);
	}
}
