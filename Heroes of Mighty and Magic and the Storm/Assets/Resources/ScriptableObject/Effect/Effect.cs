using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTarget { origin, target }
public enum EffectTargetType { Node, Unit }

public class Effect : ScriptableObject
{
    [HideInInspector]
    public Unit originUnit, targetUnit;
    [HideInInspector]
    public NodeItem targetNode;
    [HideInInspector]
    public int originPlayer;

    public EffectTarget target = EffectTarget.target;
	public EffectTargetType targetType;

    public AnimationClip fx;
    public Sound sound;

    public virtual void Init(Effect _parent)
    {
        originPlayer = _parent.originPlayer;
        originUnit = _parent.originUnit;
        targetUnit = _parent.targetUnit;
        targetNode = _parent.targetNode;
    }
    public virtual void Init(Unit _originUnit, Unit _targetUnit, NodeItem _targetNode)
    {
        originUnit = _originUnit;
        targetUnit = _targetUnit;
        targetNode = _targetNode;
    }
    public virtual void Init(Unit _originUnit)
    {
        originUnit = _originUnit;
    }

    public virtual void Invoke()
    {
        if (fx != null)
        {
			if(targetType == EffectTargetType.Node)
				OneShotFXMgr.instance.Play(fx, targetNode.transform.position);
			else
				OneShotFXMgr.instance.Play(fx, targetUnit.transform.position);
		}

		if (sound != null)
        {
            SoundManager.instance.PlaySound(sound);
        }
    }

}
