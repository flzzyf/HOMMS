﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleNodeMgr : Singleton<BattleNodeMgr>
{
    List<NodeItem> path;

    [HideInInspector]
    public NodeItem playerHovered;

    Vector3 lastMousePos;
    float mouseMoveSensitivity = 3;

    NodeItem targetNode;

	//鼠标进入节点
    public void OnNodeHovered(NodeItem_Battle _node)
    {
        _node.ChangeBackgoundColor("hover");

        playerHovered = _node;

        //是可到达节点，则显示路径
        if (_node.battleNodeType == BattleNodeType.reachable)
        {
            CursorManager.instance.ChangeCursor("reachable");

            //是地面移动单位，则计算路径
            if (BattleManager.currentActionUnit.isWalker)
            {
                //print("hover");
                FindPath(BattleManager.currentActionUnit, _node);
            }
        }
        else if (_node.battleNodeType == BattleNodeType.attackable)
        {
            if (BattleManager.currentActionUnit.IsRangeAttack)
            {
                //(有远程伤害不减的特质)，或者距离10以内
                //TraitManager.PossessTrait(BattleManager.currentActionUnit, "No melee penalty") ||
                if (AStarManager.GetNodeItemDistance(BattleManager.currentActionUnit.nodeItem,
                    _node, true) <= BattleManager.instance.rangeAttackRange)
                    CursorManager.instance.ChangeCursor("arrow");
                else
                    CursorManager.instance.ChangeCursor("arrow_penalty");
            }

            //显示文本
            BattleInfoMgr.instance.SetText_Attack(BattleManager.currentActionUnit, _node.unit);

        }
		else if (_node.battleNodeType == BattleNodeType.spellable)
		{
			CursorManager.instance.ChangeCursor("spell");

		}
		else
		{
			//如果不是可行动节点
			//如果是单位
			if (_node.nodeObject != null &&
				_node.nodeObject.nodeObjectType == NodeObjectType.unit)
			{
				//显示并更新单位属性UI
				//BattleManager.instance.ShowUnitStatUI(true, _node.unit);

				//如果不是当前行动单位，开始闪烁
				if (_node.nodeObject != BattleManager.currentActionUnit)
				{
					if (BattleManager.instance.isSamePlayer(_node.unit,
						BattleManager.currentActionUnit))
						UnitHaloMgr.instance.HaloFlashStart(_node.unit, "friend");
					else
						UnitHaloMgr.instance.HaloFlashStart(_node.unit, "enemy");
				}

				//根据敌友改变指针
				if (BattleManager.instance.isSamePlayer(_node.unit,
					BattleManager.currentActionUnit))
				{
					CursorManager.instance.ChangeCursor("friend");
				}
				else
				{
					CursorManager.instance.ChangeCursor("enemy");
				}
			}
		}
		//不可到达点
		// else if (_node.battleNodeType == BattleNodeType.empty)
		// {
		//     CursorManager.instance.ChangeCursor("stop");
		// }
	}

	//鼠标移出
	public void OnNodeUnhovered(NodeItem_Battle _node)
    {
        _node.RestoreBackgroundColor();

        CursorManager.instance.ChangeCursor();
        CursorManager.instance.ChangeCursorAngle();

        //显示并更新单位属性UI
        //BattleManager.instance.ShowUnitStatUI(false);

        if (_node.unit != null &&
            _node.unit != BattleManager.currentActionUnit)
        {
            UnitHaloMgr.instance.HaloFlashStop(_node.unit);
        }

        //有则清除之前路径
        if (path != null)
        {
            ClearPath();
        }

        playerHovered = null;
    }

	//鼠标移动
    public void OnMouseMoved(NodeItem_Battle _node)
    {
        //右键点击
        if (Input.GetMouseButtonDown(1))
        {
            if (_node.nodeObject != null &&
                _node.nodeObject.nodeObjectType == NodeObjectType.unit)
            {
				//显示并更新单位信息面板
				BattleManager.instance.panel_unitInfo.Set(_node.unit);
				//移动单位面板到鼠标位置
                Vector3 pos = BattleManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
				BattleManager.instance.panel_unitInfo.transform.position = pos;
            }

            return;
        }
		//右键起
        if (Input.GetMouseButtonUp(1))
        {
			//隐藏单位信息面板
			BattleManager.instance.panel_unitInfo.Quit();

			return;
        }

        //判断玩家是当前行动者，可能有问题
        //if (BattleManager.players[BattleManager.currentActionUnit.side] != GameManager.player)
            //return;

        //不响应鼠标小范围移动
        if (Vector3.Distance(Input.mousePosition, lastMousePos) < mouseMoveSensitivity)
        {
            return;
        }
        else
        {
            lastMousePos = Input.mousePosition;
        }

        //if可攻击
        if (_node.battleNodeType == BattleNodeType.attackable)
        {
            //如果是远程攻击，直接跳过
            if (BattleManager.currentActionUnit.IsRangeAttack)
                return;

            Vector2 mousePoint = BattleManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = mousePoint - (Vector2)_node.transform.position;
            dir.y -= 0.9f;
            //计算鼠标和节点角度
            float angle;
            if (dir.x > 0)
                angle = Vector3.Angle(dir, Vector3.up);
            else
                angle = 360 - Vector3.Angle(dir, Vector3.up);
            //计算箭头角度
            int arrowIndex = (int)angle / 60;

            //攻击方向上的格子存在，且可到达便可发起攻击。或者直接就是当前单位所在格子
            targetNode = BattleManager.instance.map.GetNearbyNodeItem(_node, arrowIndex);

            if (targetNode != null &&
               (targetNode.GetComponent<NodeItem_Battle>().battleNodeType == BattleNodeType.reachable ||
                targetNode.nodeObject == BattleManager.currentActionUnit))
            {
                //根据角度显示攻击箭头
                int arrowAngle = (arrowIndex * 60 + 210) % 360;
                int arrowAngleFixed = 360 - arrowAngle;

                CursorManager.instance.ChangeCursor("sword");
                CursorManager.instance.ChangeCursorAngle(arrowAngleFixed);

				//攻击方向上的节点不是当前行动单位，否则直接攻击无需寻路 
                if (targetNode.nodeObject != BattleManager.currentActionUnit)
                {
                    //是近战单位则显示路径
                    if (BattleManager.currentActionUnit.isWalker)
                    {
						FindPath(BattleManager.currentActionUnit, targetNode);
					}
				}
            }
            else
            {
                CursorManager.instance.ChangeCursor("enemy");
                CursorManager.instance.ChangeCursorAngle();
            }
        }
    }

	//节点被点击
    public void OnNodePressed(NodeItem_Battle _node)
    {
        //设定指令
        if (_node.battleNodeType == BattleNodeType.reachable)
        {
            //多种移动方式
            if (BattleManager.currentActionUnit.isWalker)
            {
                UnitActionMgr.order = new Order(OrderType.move,
                                            BattleManager.currentActionUnit, path);
            }
            else if (TraitManager.PossessTrait(BattleManager.currentActionUnit, "Flying"))
            {
                UnitActionMgr.order = new Order(OrderType.move,
                                                            BattleManager.currentActionUnit, _node);
            }
        }
        else if (_node.battleNodeType == BattleNodeType.attackable)
        {
            Unit target = _node.unit;

            if (BattleManager.currentActionUnit.IsRangeAttack)
            {
                UnitActionMgr.order = new Order(OrderType.rangeAttack,
                                        BattleManager.currentActionUnit, target);
            }
            else
            {
                //如果targetNode就是攻击者所在节点，直接攻击
                if (targetNode == BattleManager.currentActionUnit.nodeItem)
                {
                    UnitActionMgr.order = new Order(OrderType.attack,
                                            BattleManager.currentActionUnit, target);
                }
                else
                {
                    if (BattleManager.currentActionUnit.isWalker)
                    {
                        if (path != null)
                            UnitActionMgr.order = new Order(OrderType.attack,
                                                        BattleManager.currentActionUnit, path, target);
                        else
                            Debug.LogError("攻击无路径BUG");
                    }
                    else
                    {
                        UnitActionMgr.order = new Order(OrderType.attack,
                                                        BattleManager.currentActionUnit, targetNode, target);
                    }
                }
            }
		}
		else if (_node.battleNodeType == BattleNodeType.spellable)
		{
			//隐藏可施法节点
			NodeSelector.HideSpellableNodes();
			//施法
			MagicManager.instance.CastMagic(_node);
		}

		if (_node.battleNodeType != BattleNodeType.empty)
        {
            if (path != null)
                ClearPath();
            CursorManager.instance.ChangeCursor();
            CursorManager.instance.ChangeCursorAngle();
        }
    }

    //清除之前路径
    void ClearPath()
    {
        foreach (var item in path)
        {
            item.GetComponent<NodeItem_Battle>().RestoreBackgroundColor();
        }

        path = null;
    }

    //寻找路径
    bool FindPath(Unit _unit, NodeItem _target)
    {
        if (path != null)
            ClearPath();

		NodeItem unitNode = _unit.nodeItem;

		/*
		//判断是双格单位，而且前方点更近，则使用前方点进行寻路
		if(_unit.type.isTwoHexsUnit && (_target.pos - _unit.nodeAhead.pos).magnitude < (_target.pos - _unit.nodeItem.pos).magnitude)
		{
			unitNode = _unit.nodeAhead;
		}

		if(_unit.type.isTwoHexsUnit)
		{
			//前方点不存在或被占用，则使用上个点为目的地
			Vector2Int pos = _target.pos;
			pos.x += _unit.sideFacing;
			if(!BattleManager.instance.map.isNodeAvailable(pos) || !BattleManager.instance.map.GetNode(pos).walkable)
			{
				pos.x -= 2 * _unit.sideFacing;
				_target = BattleManager.instance.map.GetNodeItem(pos);
			}
		}
		*/

		//根据是否是双格单位，选中寻路方法
		if (!_unit.type.isTwoHexsUnit)
        {
            path = AStarManager.FindPath(BattleManager.instance.map, unitNode, _target, !_unit.isWalker);

        }
        else
		{
			path = new List<NodeItem>();
			MapManager map = BattleManager.instance.map;

			//如果目标点的前方点不存在，或者不在可到达节点，则目标点向后移动
			Vector2Int ahead = new Vector2Int(_target.pos.x + _unit.sideFacing, _target.pos.y);
			if (!map.isNodeAvailable(ahead) || !NodeSelector.reachableNodes.Contains(map.GetNodeItem(ahead)))
			{
				_target = map.GetNodeItem(new Vector2Int(_target.pos.x - _unit.sideFacing, _target.pos.y));
			}

			foreach (var item in AStarManager.FindPath_TwoHex(map, map.GetNode(unitNode.pos), map.GetNode(_unit.nodeAhead.pos), map.GetNode(_target.pos), !_unit.isWalker))
			{
				path.Add(map.GetNodeItem(item.pos));
			}
		}

		//print(path.Count);
		if (path == null)
        {
            //print("未能找到路径");
            return false;
        }

		path.RemoveAt(0);

        foreach (var item in path)
        {
            item.GetComponent<NodeItem_Battle>().ChangeBackgoundColor("path");
        }
        return true;
    }
}
