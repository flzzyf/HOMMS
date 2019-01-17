using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicItem : MonoBehaviour, IPointerEnterHandler
{
    public Image icon;
    [HideInInspector]
    public int bookIndex;

    public Text text_name;
    public Text text_level;
    public Text text_mana;

    public Magic magic;

    public string key_level;
    public string key_mana;

	public bool noEnoughMana;

    public void Init()
    {
        icon.sprite = magic.icon;
        text_name.text = magic.magicName;
        text_level.text = string.Format(LocalizationMgr.instance.GetText(key_level), magic.level.ToString());
        text_mana.text = string.Format(LocalizationMgr.instance.GetText(key_mana), magic.GetManaCost(BattleManager.currentHero).ToString());

		//判定魔法量，不足以释放魔法
		if(BattleManager.currentHero.mana < magic.GetManaCost(BattleManager.currentHero))
		{
			noEnoughMana = true;

			//文本改为魔法量不足的颜色
			text_level.color = MagicBookMgr.instance.textColors.GetColor("NoEnoughMana");
			text_mana.color = MagicBookMgr.instance.textColors.GetColor("NoEnoughMana");
		}
		else
		{
			//文本改回白色
			text_level.color = Color.white;
			text_mana.color = Color.white;
		}
	}

    public void Init(Magic _magic)
    {
        magic = _magic;
        Init();
    }

    public void OnClick()
    {
        MagicBookMgr.instance.CastMagic(bookIndex);
    }

    public void OnRightMouseDown()
    {
        MagicBookMgr.instance.ShowMagicInfo(bookIndex);
    }

    //鼠标进入
    public void OnPointerEnter(PointerEventData eventData)
    {
        MagicBookMgr.instance.text_info.text = LocalizationMgr.instance.GetText(magic.name) + " (" + magic.level + "" + LocalizationMgr.instance.GetText("Lev") + ")";
    }

}
