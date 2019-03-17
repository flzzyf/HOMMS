using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void ItemClick(int _index);

public class SliderItem : MonoBehaviour, IPointerClickHandler
{
    public GameObject border_highlight;
	public bool isHighlighted { get { return border_highlight.activeSelf; } }

	//序号，即在父级中的位置
	int index { get { return transform.GetSiblingIndex(); } }

	public virtual bool IsEnabled() { return true; }

	//被点击事件
	public ItemClick onClick;

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
