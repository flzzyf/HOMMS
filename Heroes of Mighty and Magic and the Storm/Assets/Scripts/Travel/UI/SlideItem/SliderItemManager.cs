using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//旅行界面的英雄和城镇项管理器
[System.Serializable]
public class SliderItemManager<T> : MonoBehaviour
{
    public T[] items;
    public Button button_pageUp;
    public Button button_pageDown;

    [HideInInspector]
    public static int currentPages;

    //当前高亮项的序号
    [HideInInspector]
    public static int highlightedItemIndex = -1;

	public int itemCount { get { return items.Length; } }

    //翻到某页
    public virtual void MoveToPage(int _page)
    {
        currentPages = _page;

        int num;
        if (GetType().Equals(typeof(SliderItemManager_Hero)))
        {
            num = GameManager.currentPlayer.heroes.Count;
        }
        else
        {
            num = GameManager.currentPlayer.towns.Count;
        }

        //显示/隐藏翻页按钮
        if (currentPages > 0)
            button_pageUp.interactable = true;
        else
            button_pageUp.interactable = false;

        if (currentPages + items.Length < num)
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
