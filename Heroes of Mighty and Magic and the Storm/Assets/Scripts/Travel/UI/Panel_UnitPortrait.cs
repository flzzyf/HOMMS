using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Panel_UnitPortrait : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public Image portrait;
    public Text text_num;

    public GameObject border_selected;

    [HideInInspector]
    public UnitType unitType;
    [HideInInspector]
    public int unitNum;

    public static Panel_UnitPortrait selectedPanel;

    //该单位栏的序号
    [HideInInspector]
    public int index;

    //初始设置
    public void Init(PocketUnit _unit)
    {
        Init(_unit.type, _unit.num);
    }
    public void Init(UnitType _type, int _num)
    {
        unitType = _type;
        unitNum = _num;

        //显示头像
        if (!portrait.enabled)
            portrait.enabled = true;
        //设置头像
        portrait.sprite = _type.icon;
        //设置数量
        text_num.text = _num + "";
    }

    //设置，同时设置玩家携带单位
    public void Set(UnitType _type = null, int _num = 0)
    {
        //如果type为空则清除
        if (_type == null)
        {
            Clear();
            TravelManager.currentHero.pocketUnits[index] = null;

            return;
        }

        //修改英雄携带单位
        TravelManager.currentHero.pocketUnits[index] = new PocketUnit(_type, _num);
        //设置框内单位
        Init(_type, _num);
    }

    //清空
    public void Clear()
    {
        portrait.enabled = false;
        text_num.text = "";

        unitType = null;
    }

    //选中
    public void Select()
    {
        Highlight(true);

        selectedPanel = this;
    }
    public void Deselect()
    {
        Highlight(false);

        selectedPanel = null;
    }

    public void Highlight(bool _highlight)
    {
        border_selected.SetActive(_highlight);
    }

    //鼠标进入
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (unitType != null)
            UIManager.text_bottomInfo.text = LocalizationMgr.instance.GetText("Select") + unitType.unitName;
    }
    //鼠标点击
    public void OnPointerClick(PointerEventData eventData)
    {
        //未选中物体
        if (selectedPanel == null)
        {
            //且不为空，则选中
            if (unitType != null)
                Select();
        }
        else
        {
            //已经选中物体
            if (unitType == null)
            {
                //点击的为空，移动
                Set(selectedPanel.unitType, selectedPanel.unitNum);
                selectedPanel.Set();
            }
            else
            {
                if (unitType != selectedPanel.unitType)
                {
                    //是不同种单位，交换
                    UnitType tempType = unitType;
                    int tempNum = unitNum;

                    Set(selectedPanel.unitType, selectedPanel.unitNum);
                    selectedPanel.Set(tempType, tempNum);
                }
                else
                {
                    //同种单位，叠加
                    Set(unitType, unitNum + selectedPanel.unitNum);
                    selectedPanel.Set();
                }
            }

            selectedPanel.Deselect();
        }
    }
}
