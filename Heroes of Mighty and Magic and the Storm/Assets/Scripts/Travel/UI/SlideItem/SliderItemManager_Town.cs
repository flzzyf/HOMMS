using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItemManager_Town : SliderItemManager<Panel_TownItem>
{
    void Start()
    {
        //初始化序号
        for (int i = 0; i < items.Length; i++)
        {
            items[i].index = i;
            items[i].onClick += ItemClicked;
        }
    }

    //项被点击
    void ItemClicked(int _index)
    {
        int index = currentPages + _index;

        //如果当前高亮项不是这个
        if (highlightedItemIndex != index)
        {
            //如果当前有高亮项，则取消高亮
            if (highlightedItemIndex != -1)
                Highlight(highlightedItemIndex - currentPages, false);

            //如果当前高亮滑动项类型不同，取消另一种项的高亮
            if (TravelManager.highlightedItemType != HighlightedItemType.Town)
            {
                TravelManager.instance.sliderItemManager_hero.Highlight(
                    SliderItemManager_Hero.highlightedItemIndex - SliderItemManager_Hero.currentPages, false);
            }

            Highlight(index);
        }
        else
        {
            //当前高亮的是这个，则进入城镇界面
            UIManager.instance.Get("town").GetComponent<Panel_Town>().Set(GameManager.currentPlayer.towns[index]);
            UIManager.instance.Enter("town");
        }
    }

    //高亮项
    public void Highlight(int _index, bool _highlight = true)
    {
        items[_index].Highlight(_highlight);

        highlightedItemIndex = _highlight ? _index : -1;

        //设置当前高亮滑动项的类型
        if (_highlight)
            TravelManager.highlightedItemType = HighlightedItemType.Town;
    }

    //翻到某页
    public override void MoveToPage(int _page)
    {
        base.MoveToPage(_page);

        int num = GameManager.currentPlayer.towns.Count;

        //显示按钮并更新图像
        int numberToShow = Mathf.Min(num - currentPages, items.Length);
        for (int i = 0; i < numberToShow; i++)
        {
            items[i].enabled = true;
            items[i].Set(GameManager.currentPlayer.towns[currentPages + i]);
        }
        for (int i = numberToShow; i < items.Length; i++)
        {
            items[i].Clear();
            items[i].enabled = false;
        }
    }
}