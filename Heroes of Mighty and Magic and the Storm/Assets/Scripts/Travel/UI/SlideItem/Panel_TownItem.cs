using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_TownItem : SliderItem
{
    public Image image_bg;
    public GameObject icon_finishBuilding;

    public void Set(Town _town)
    {
        image_bg.enabled = true;
        image_bg.sprite = _town.race.sprite_bg;
    }

    //清空
    public void Clear()
    {
        image_bg.enabled = false;

        icon_finishBuilding.SetActive(false);
    }
}
