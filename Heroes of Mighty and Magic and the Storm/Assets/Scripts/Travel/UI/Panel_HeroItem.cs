using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Panel_HeroItem : MonoBehaviour, IPointerClickHandler
{
    public Image image_portrait;
    public GameObject border_highlight;

    public Slider slider_movementRate;
    public Slider slider_mana;

    [HideInInspector]
    public int index;

	//被点击事件
	public delegate void ItemClick(int _index);
	public ItemClick onClick;

	//鼠标点击
	public void OnPointerClick(PointerEventData data)
    {
		onClick.Invoke(index);
	}

	public void Highlight(bool _highlight)
    {
        border_highlight.SetActive(_highlight);
    }

    //更新图像
    public void Set(Hero _hero)
    {
        image_portrait.enabled = true;
        image_portrait.sprite = _hero.heroType.icon;

        //更新移动力、魔法
        slider_movementRate.value = _hero.movementRate / 2000f;
        slider_mana.value = _hero.mana / 50f;
    }
    //重置
    public void Clear()
    {
        image_portrait.enabled = false;

        slider_movementRate.value = 0;
        slider_mana.value = 0;
    }
}
