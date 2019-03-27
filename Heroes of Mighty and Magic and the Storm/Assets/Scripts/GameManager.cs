using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameScene { Travel, Battle }

public enum GameState { playerControl, canNotControl }

public class GameManager : Singleton<GameManager>
{
    public static GameState gameState;

	//本地玩家
	public int localPlayerIndex;
	public Player localPlayer { get { return players[localPlayerIndex]; } }

	//当前回合行动的玩家
	public static int actionPlayer;

    public GameScene scene;

	public Player[] players;


	void Start()
    {
        //之前有保存语言则直接设置，否则根据本地语言设置
        // if (PlayerPrefs.HasKey("Language"))
        //     ChangeLanguage(PlayerPrefs.GetString("Language"));
        // else
        //     ChangeToLocalLanguage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SoundManager.instance.PlaySound("Combat02");
            //TravelManager.instance.EnterTravelMode();
            //LocalizationMgr.instance.ChangeToLanguage(Language.English);

            Time.timeScale = 2f;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SoundManager.instance.StopPlay("Combat02");
            //LocalizationMgr.instance.ChangeToLanguage(Language.Chinese_Simplified);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            //SoundManager.instance.PlaySound("PickUp");
            //SkillManager.AddSkill(testHeroes[0], "Magic_Air", 3);
            //MagicBookMgr.instance.SetMagics(testHeroes[0]);
            //MagicBookMgr.instance.ShowMagics(MagicSchool.All, MagicType.Battle);
            BattleResultMgr.instance.ShowResultUI(0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SoundManager.instance.StopPlay("PickUp");
            //MagicBookMgr.instance.ShowMagics(MagicSchool.All, MagicType.All, 1);

            //MagicBookMgr.instance.Show();
            //BattleResultMgr.instance.ShowResultUI(1);
            //Resources.LoadAll<Sprite>("Textures");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
			Hero hero = GameManager.instance.players[0].heroes[0];

			SkillManager.AddSkill(hero, "Sorcery", 1);
            SkillManager.AddSkill(hero, "Wisdom", 2);
            SkillManager.AddSkill(hero, "Magic_Air", 2);
            SkillManager.AddSkill(hero, "Magic_Fire", 2);
            SkillManager.AddSkill(hero, "Magic_Water", 2);
            SkillManager.AddSkill(hero, "Magic_Earth", 2);

            //SkillManager.AddSkill(testHeroes[0], "Magic_Earth", 3);
            //MagicManager.instance.CastMagic(testHeroes[0], testHeroes[0].magics[1]);

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SoundManager.instance.PlaySound("Combat02");
        }
    }

    //变为本地语言
    void ChangeToLocalLanguage()
    {
        string localLang = Application.systemLanguage.ToString();
        //print("本地语言：" + localLang);
        ChangeLanguage(localLang);
    }

    void ChangeLanguage(string _lang)
    {
        if (_lang == "Chinese")
            LocalizationMgr.instance.ChangeToLanguage(Language.Chinese_Simplified);
        else
            LocalizationMgr.instance.ChangeToLanguage(Language.English);
    }
}
