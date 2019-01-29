using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Battle : CustomUI
{
	public GameObject battleObjectParent;

	public override void Enter(bool _quitCurrentUI = false)
	{
		base.Enter(_quitCurrentUI);

		battleObjectParent.SetActive(true);
		//显示节点
		ParentManager.instance.GetParent("MapManager_Battle").gameObject.SetActive(true);
	}

	public override void Quit()
	{
		base.Quit();

		battleObjectParent.SetActive(false);
	}
}
