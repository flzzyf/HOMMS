using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitActionMgr : Singleton<UnitActionMgr>
{
    List<NodeItem> reachableNodes;

    public static Order order;

    public void ActionStart(Unit _unit)
    {
        StartCoroutine(ActionStartCor(_unit));
    }

    IEnumerator ActionStartCor(Unit _unit)
    {
        UnitHaloMgr.instance.HaloFlashStart(_unit, "action");

		//测试：永远是玩家控制
		PlayerActionStart(_unit);

		/*
		if (!PlayerManager.instance.players[BattleManager.players[_unit.side]].isAI)
        {
            PlayerActionStart(_unit);
        }
        else
        {
            AIActionMgr.instance.AIActionStart(_unit);
        }*/

        //在下令前暂停
        while (order == null)
            yield return null;

        //发出指令后，开始执行命令
        UnitHaloMgr.instance.HaloFlashStop(_unit);
		//重置所有可行动节点
		NodeSelector.HideActionNodes();
		//执行指令
        InvokeOrder();

        GameManager.gameState = GameState.canNotControl;
        //在指令完成前暂停
        while (order != null)
            yield return null;

        //命令执行完毕

        ActionEnd();
    }

	//单位行动开始，显示移动范围，可攻击目标
    public void PlayerActionStart(Unit _unit)
    {
        GameManager.gameState = GameState.playerControl;

		//获取并高亮单位可行动的节点
		NodeSelector.GetUnitActionNodes(_unit);
		NodeSelector.HighlightUnitActionNodes();


		//将当前鼠标高亮节点，触发高亮事件
		MapManager_Battle map = BattleManager.instance.map;
        if (BattleNodeMgr.instance.playerHovered != null)
        {
            map.OnNodeHovered(BattleNodeMgr.instance.playerHovered);
        }
    }

    //执行指令
    public void InvokeOrder()
    {
        StartCoroutine(InvokeOrderCor());
    }
    IEnumerator InvokeOrderCor()
    {
        if (order.type == OrderType.move)
        {
            NodeMovingMgr.instance.Event_StartMoving += StartMoving;
            NodeMovingMgr.instance.Event_ReachTarget += ReachTarget;
            NodeMovingMgr.instance.Event_MovingToNode += MoveToNode;

            if (order.origin.isWalker)
            {
				//行走
                NodeMovingMgr.instance.MoveObject(order.origin.gameObject, order.path,
                    order.origin.UnitActualSpeed, MapCoord.xy);
            }
            else if (TraitManager.PossessTrait(order.origin, "Flying"))
            {
				//飞行
                NodeMovingMgr.instance.MoveObjectFlying(order.origin.gameObject, order.targetNode,
                    order.origin.UnitActualSpeed, MapCoord.xy);
            }
            //else if 瞬移

            if (order.origin.RestoreFacing())
            {
                //需要转身
                yield return new WaitForSeconds(UnitAttackMgr.instance.animTurnbackTime);
            }

            while (NodeMovingMgr.instance.moving)
                yield return null;

            if (order.origin.RestoreFacing())
            {
                //需要转身
                yield return new WaitForSeconds(UnitAttackMgr.instance.animTurnbackTime);
            }
        }
        else if (order.type == OrderType.attack)
        {
            //移动后攻击
            if (order.path != null || order.targetNode != null)
            {
                NodeMovingMgr.instance.Event_StartMoving += StartMoving;
                NodeMovingMgr.instance.Event_ReachTarget += ReachTarget;
                NodeMovingMgr.instance.Event_MovingToNode += MoveToNode;

                if (order.path != null)
                {
                    NodeMovingMgr.instance.MoveObject(order.origin.gameObject, order.path,
                        order.origin.UnitActualSpeed, MapCoord.xy);
                }
                else
                    NodeMovingMgr.instance.MoveObjectFlying(order.origin.gameObject, order.targetNode,
                order.origin.UnitActualSpeed, MapCoord.xy);

                while (NodeMovingMgr.instance.moving)
                    yield return null;
            }

            //攻击
            UnitAttackMgr.instance.Attack(order.origin, order.target);

            while (UnitAttackMgr.operating)
                yield return null;
        }
        else if (order.type == OrderType.rangeAttack)
        {
            UnitAttackMgr.instance.Attack(order.origin, order.target, true);

            while (UnitAttackMgr.operating)
                yield return null;
        }
        else if (order.type == OrderType.wait)
        {
            BattleManager bm = BattleManager.instance;
            bm.AddUnitToActionList(ref bm.waitingUnitList, BattleManager.currentActionUnit, false);
        }
        else if (order.type == OrderType.defend)
        {
            //获得+1/5防御力buff一回合
            BehaviorMgr.AddBehavior(order.origin, BehaviorMgr.GetBehavior("Defend"));
        }

        order = null;
    }

    void StartMoving()
    {
		//GameManager.instance.gamePaused = true;


        //播放移动动画和音效
        UnitAnimMgr.instance.PlayAnimation(BattleManager.currentActionUnit, Anim.Walk);

        if (BattleManager.currentActionUnit.type.sound_walk != null)
            StartCoroutine(PlayMoveSound(BattleManager.currentActionUnit));
    }
	//播放单位行走音效
    IEnumerator PlayMoveSound(Unit _unit)
    {
		//如果是循环音效则播放一次
		if(_unit.type.sound_walk.loop)
		{
			SoundManager.instance.PlaySound(_unit.type.sound_walk);

			while (NodeMovingMgr.instance.moving)
				yield return null;

			SoundManager.instance.StopCategory(SoundCategory.Walk);
		}
		else
		{
			//否则持续播放（也应该根据单位速度播放
			int speed = _unit.speed;
			float waitTime = 0.2f + speed * 0.05f;

			while (NodeMovingMgr.instance.moving)
			{
				SoundManager.instance.PlaySound(_unit.type.sound_walk);

				yield return new WaitForSeconds(waitTime);
			}

			SoundManager.instance.StopCategory(SoundCategory.Walk);
		}
	}

	void MoveToNode(NodeItem _node)
    {
        //改变单位朝向
        BattleManager.currentActionUnit.FaceTarget(_node.transform.position);

        BattleManager.instance.map.LinkNodeWithUnit(BattleManager.currentActionUnit, _node);
    }

    void ReachTarget(NodeItem _node)
    {
        //GameManager.instance.gamePaused = false;

        UnitAnimMgr.instance.PlayAnimation(BattleManager.currentActionUnit, Anim.Walk, false);

        //BattleManager.currentActionUnit.RestoreFacing();
    }

    public void ActionEnd()
    {
        RoundManager.instance.TurnEnd();
    }

}

public enum OrderType { move, attack, rangeAttack, wait, defend, cast }

public class Order
{
    public OrderType type;
    public Unit origin, target;
    public NodeItem targetNode;
    public List<NodeItem> path;

    public Order(OrderType _type, Unit _origin)
    {
        type = _type;
        origin = _origin;
    }
    public Order(OrderType _type, Unit _origin, List<NodeItem> _path)
    {
        type = _type;
        origin = _origin;
        path = _path;
    }
    public Order(OrderType _type, Unit _origin, Unit _target)
    {
        type = _type;
        origin = _origin;
        target = _target;
    }
    public Order(OrderType _type, Unit _origin, List<NodeItem> _path, Unit _target)
    {
        type = _type;
        origin = _origin;
        path = _path;
        target = _target;
    }
    public Order(OrderType _type, Unit _origin, NodeItem _targetNode)
    {
        type = _type;
        origin = _origin;
        targetNode = _targetNode;
    }
    public Order(OrderType _type, Unit _origin, NodeItem _targetNode, Unit _target)
    {
        type = _type;
        origin = _origin;
        targetNode = _targetNode;
        target = _target;
    }
}
