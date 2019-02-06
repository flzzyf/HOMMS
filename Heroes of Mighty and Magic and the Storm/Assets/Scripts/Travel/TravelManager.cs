using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HighlightedItemType { Hero, Town }

public class TravelManager : Singleton<TravelManager>
{
    [HideInInspector]
    public GameObject lastHighlightNode;
    List<GameObject> lastPath;

    //当前英雄
    public static Hero currentHero;

    public Transform[] spawnPoints;
    public GameObject prefab_town;
    public GameObject prefab_hero;

    public float heroSpeed = 1;

    public MapManager_Travel map;

    public SliderItemManager_Hero sliderItemManager_hero;
    public SliderItemManager_Town sliderItemManager_town;

    //高亮滑动项的类型
    public static HighlightedItemType highlightedItemType;

    public void Init()
    {
        map.GenerateMap();

        //玩家初始设置
        for (int i = 0; i < PlayerManager.instance.players.Length; i++)
        {
            InitPlayer(PlayerManager.instance.players[i]);
        }
    }

    //玩家初始化，生成城镇和英雄
    void InitPlayer(Player _player)
    {
        GameObject town = CreateObjectOnNode(prefab_town, _player.startingPoint);
        Vector2Int offset = town.GetComponent<Town>().interactPoint;
        Hero hero = CreateObjectOnNode(prefab_hero, _player.startingPoint + offset).GetComponent<Hero>();
        //英雄类型
        hero.heroType = HeroType.GetHeroType("Jaina");
        hero.Init();
        _player.heroes.Add(hero);

        //添加英雄初始兵力
        hero.pocketUnits[0] = new PocketUnit(UnitType.GetUnit("Crusader_Upgrade"), 40);
        hero.pocketUnits[1] = new PocketUnit(UnitType.GetUnit("Tyrael"), 10);

        //非AI
        if (!_player.isAI)
        {
            //移动镜头到英雄
            //HighlightHero(hero);
        }
    }
    //在节点上创建物体
    GameObject CreateObjectOnNode(GameObject _prefab, Vector2Int _pos)
    {
        NodeItem node = map.GetNodeItem(_pos);

        GameObject go = Instantiate(_prefab, node.transform.position, Quaternion.identity);
        go.GetComponent<NodeObject>().nodeItem = map.GetNodeItem(_pos);
        node.nodeObject = go.GetComponent<NodeObject>();

        return go;
    }

    //回合开始
    public void TurnStart(int _index)
    {
        GameManager.actionPlayer = _index;

        //更新英雄项，选中第一个
        if (GameManager.currentPlayer.heroes.Count > 0)
        {
            sliderItemManager_hero.Highlight(0);
            sliderItemManager_hero.MoveToPage(0);
        }
        // if (GameManager.currentPlayer.towns.Count > 0)
        // {
        //     sliderItemManager_town.Highlight(0);
        //     sliderItemManager_town.MoveToPage(0);
        // }

        //高亮玩家的第一个英雄
        sliderItemManager_hero.Highlight(0);
    }

    public void BattleBegin(Hero _attacker, Hero _defender)
    {
        UIManager.instance.Enter("battle", true);

        BattleManager.instance.BattleStart(_attacker, _defender);
    }
}
