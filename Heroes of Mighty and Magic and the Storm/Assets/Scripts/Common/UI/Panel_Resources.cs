using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Resources : MonoBehaviour
{
	public Panel_Resource panel_Resources_Gold;
	public Panel_Resource panel_Resources_Wood;
	public Panel_Resource panel_Resources_Ore;
	public Panel_Resource panel_Resources_Sulfur;
	public Panel_Resource panel_Resources_Crystals;
	public Panel_Resource panel_Resources_Mercury;
	public Panel_Resource panel_Resources_Gems;

	public void Set(HOMMResource _resources)
	{
		panel_Resources_Gold.Set(_resources.gold);
		panel_Resources_Wood.Set(_resources.wood);
		panel_Resources_Ore.Set(_resources.ore);
		panel_Resources_Sulfur.Set(_resources.sulfur);
		panel_Resources_Crystals.Set(_resources.crystal);
		panel_Resources_Mercury.Set(_resources.mercury);
		panel_Resources_Gems.Set(_resources.gem);
	}
}
