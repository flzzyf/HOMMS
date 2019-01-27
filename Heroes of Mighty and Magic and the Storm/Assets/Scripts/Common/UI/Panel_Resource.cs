using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Resource : MonoBehaviour
{
	public Image icon;
	public Text text_num;

	public void Set(int _num)
	{
		text_num.text = _num.ToString();
	}
}
