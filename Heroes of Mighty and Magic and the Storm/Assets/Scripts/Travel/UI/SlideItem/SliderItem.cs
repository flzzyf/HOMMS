using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderItem : MonoBehaviour, IPointerClickHandler
{
    public GameObject border_highlight;

    //序号
    public int index;

    //被点击事件
    public delegate void ItemClick(int _index);
    public ItemClick onClick;

	public bool isHighlighted { get { return border_highlight.activeSelf; } }

    //鼠标点击
    public void OnPointerClick(PointerEventData data)
    {
        onClick.Invoke(index);
    }

    //高亮
    public void Highlight(bool _highlight = true)
    {
        //显示高亮边框
        border_highlight.SetActive(_highlight);
    }
}
