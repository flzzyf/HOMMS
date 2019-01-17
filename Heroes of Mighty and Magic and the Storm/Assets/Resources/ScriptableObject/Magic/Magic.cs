using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicSchool { All, Earth, Fire, Air, Water }
public enum MagicType { Battle, Travel, All }
public enum MagicTargetType { Null, Unit, Node }
public enum MagicTargetFliter { All, Ally, Enemy }

[CreateAssetMenu(menuName = "Magic")]
public class Magic : ScriptableObject
{
	public Sprite icon;
	public MagicSchool school;
	public MagicType type;
	public int level = 1;
	public int[] mana = new int[1];

	//目标类型
	public MagicTargetType[] targetType = new MagicTargetType[1];
	public MagicTargetFliter[] targetFliter = new MagicTargetFliter[1];

	public Effect[] effects;

	public string magicName { get { return LocalizationMgr.instance.GetText(base.name); } }
	public string magicInfo { get { return LocalizationMgr.instance.GetText(base.name + "_Info"); } }
	public string magicEffect { get { return LocalizationMgr.instance.GetText(base.name + "_Effect"); } }

	//魔法学派对应技能
	Skill SchoolToSkill(MagicSchool _school)
	{
		return SkillManager.GetSkill("Magic_" + _school.ToString());
	}
	//获取英雄该魔法等级
	public int GetMagicLevel(Hero _hero)
	{
		//如果是全派系通用魔法，则遍历所有派系选出等级最高的
		if(school == MagicSchool.All)
		{
			int maxLevel = 0;
			//遍历除了所有（0）以外的魔法派系，选出等级最高的
			for (int i = 1; i < System.Enum.GetValues(typeof(MagicSchool)).Length; i++)
			{
				if(SkillManager.LevelOfSkill(_hero, SchoolToSkill((MagicSchool)i)) > maxLevel)
				{
					maxLevel = SkillManager.LevelOfSkill(_hero, SchoolToSkill((MagicSchool)i));
				}
			}

			return maxLevel;
		}
		else
			return SkillManager.LevelOfSkill(_hero, SchoolToSkill(school));
	}
	//获取该等级魔法值消耗
	public int GetManaCost(Hero _hero)
	{
		return mana[Mathf.Min(mana.Length - 1, GetMagicLevel(_hero))];
	}
}
