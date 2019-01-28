using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Build : CustomUI
{
	public Panel_Resources panel_Resources;

	public override void Enter(bool _quitCurrentUI = false)
	{
		base.Enter(_quitCurrentUI);

		//设置资源
		panel_Resources.Set(GameManager.currentPlayer.resources);
	}
}
