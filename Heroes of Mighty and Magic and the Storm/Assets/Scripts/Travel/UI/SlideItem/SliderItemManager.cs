using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightedItemType { Hero, Town }

//旅行界面的英雄和城镇项管理器
[System.Serializable]
public class SliderItemManager : MonoBehaviour
{
    public Button button_pageUp;
    public Button button_pageDown;

    [HideInInspector]
    public int currentPages;

	[HideInInspector]
	public List<SliderItem> items;

    //当前高亮项的序号
    public static int highlightedItemIndex = -1;

	//所有项直接放在transform下，子物体数就是项数
	public int itemCount { get { return transform.childCount; } }

	//点击高亮项
	public virtual void ClickHighlightedItem(int _index) { }

	//当前高亮的项类型
	public static HighlightedItemType highlightedItemType;

	void Start()
	{
		//初始化序号
		for (int i = 0; i < itemCount; i++)
		{
			items[i] = transform.GetChild(i).GetComponent<SliderItem>();
			items[i].onClick += ItemClicked;
		}

		button_pageDown.onClick.AddListener(PageDown);
		button_pageUp.onClick.AddListener(PageUp);
	}

	//项被点击
	public void ItemClicked(int _index)
	{
		//该项真正的序号
		int index = currentPages + _index;

		if (highlightedItemIndex == index && IsHightlightedItemType()) //如果当前高亮项是这个，而且高亮类型是这个，则直接进入界面
		{
			ClickHighlightedItem(index);
		}
		else //否则将其设为高亮项，然后更新所有项
		{
			//高亮项
			Highlight(index);
		}
	}

	//高亮，并更新所有项
	public virtual void Highlight(int _index)
	{
		highlightedItemIndex = _index;

		UpdateItems();
	}

	public virtual int ObjectCount() { return 0; }

	public virtual bool IsHightlightedItemType() { return true; }

	//更新本页的项的情况
	public virtual void UpdateItems()
	{
		//更新高亮项
		for (int i = 0; i < itemCount; i++)
		{
			if(ObjectCount() - currentPages > i)
			{
				items[i].enabled = true;
				SetItem(i);
			}
			else
			{
				ClearItem(i);
			}

			//是高亮的类型才显示高亮项
			if(IsHightlightedItemType())
			{
				bool isHightlightItem = currentPages + i == highlightedItemIndex;
				items[i].Highlight(isHightlightItem);
			}
		}
	}

	public void DishighlightAll()
	{
		for (int i = 0; i < itemCount; i++)
		{
			items[i].Highlight(false);
		}
	}

	public virtual void SetItem(int _index) { }
	public virtual void ClearItem(int _index) { }

	//翻到某页
	public void MoveToPage(int _page)
    {
        currentPages = _page;

		UpdateItems();

        //显示/隐藏翻页按钮
        if (currentPages > 0)
            button_pageUp.interactable = true;
        else
            button_pageUp.interactable = false;

		if (currentPages + itemCount < ObjectCount())
			button_pageDown.interactable = true;
		else
			button_pageDown.interactable = false;
	}

    //翻页
    public void PageDown()
    {
        MoveToPage(currentPages + 1);
    }
    public void PageUp()
    {
        MoveToPage(currentPages - 1);
    }
}
