using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderItemManager_Hero : SliderItemManager<Panel_HeroItem>
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
            //if (highlightedItemIndex != -1)
                //Highlight(highlightedItemIndex - currentPages, false);


            //如果当前高亮滑动项类型不同，取消另一种项的高亮
            if (TravelManager.highlightedItemType != HighlightedItemType.Hero)
            {
                TravelManager.instance.sliderItemManager_town.Highlight(
                    SliderItemManager_Town.highlightedItemIndex - SliderItemManager_Town.currentPages, false);
            }

            //高亮项
            Highlight(index);

			UpdateHightlight();
		}
		else
        {
			//当前高亮的是这个，则设置英雄，进入界面
            UIManager.instance.Get("hero").GetComponent<Panel_HeroUI>().Set(GameManager.currentPlayer.heroes[index]);
            UIManager.instance.Enter("hero");
        }
    }

    //高亮项
    public void Highlight(int _index, bool _highlight = true)
    {
        //items[_index].Highlight(_highlight);

        highlightedItemIndex = _highlight ? _index : -1;

        if (_highlight)
        {
            //设置当前高亮滑动项的类型
            TravelManager.highlightedItemType = HighlightedItemType.Hero;

            //设置当前英雄
            TravelManager.currentHero = GameManager.currentPlayer.heroes[_index];

			//移动镜头
			TravelCamMgr.instance.MoveCamera(TravelManager.currentHero.transform.position);
            //更新右下角英雄信息
            Panel_HeroInfo.instance.Set(TravelManager.currentHero);
        }
    }

    //翻到某页
    public override void MoveToPage(int _page)
    {
        base.MoveToPage(_page);

        int heroNumber = GameManager.currentPlayer.heroes.Count;

        //显示按钮并更新图像
        int numberToShow = Mathf.Min(heroNumber - currentPages, 5);
        for (int i = 0; i < numberToShow; i++)
        {
            items[i].enabled = true;
            items[i].Set(GameManager.currentPlayer.heroes[currentPages + i]);
        }
        for (int i = numberToShow; i < 5; i++)
        {
            items[i].Clear();
            items[i].enabled = false;
        }

		UpdateHightlight();
	}

	//遍历，如果是当前高亮项则高亮，否则取消高亮
	void UpdateHightlight()
	{
		for (int i = 0; i < itemCount; i++)
		{
			if (!items[i].enable)
				continue;

			bool isHightlightItem = currentPages + i == highlightedItemIndex;
			items[i].Highlight(isHightlightItem);
		}
	}
}
