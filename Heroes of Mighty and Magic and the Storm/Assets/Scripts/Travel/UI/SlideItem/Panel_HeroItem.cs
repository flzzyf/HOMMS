using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_HeroItem : SliderItem
{
    public Image image_portrait;

    public Slider slider_movementRate;
    public Slider slider_mana;

	public override bool IsEnabled() { return image_portrait.enabled; }

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
