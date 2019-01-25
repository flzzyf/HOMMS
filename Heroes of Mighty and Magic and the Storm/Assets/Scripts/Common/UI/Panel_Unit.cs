using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Unit : MonoBehaviour
{
	public Image image_bg;
	public Animator animator;
	public Text text_num;

	//随机播放的动画列表
	string[] baseAnim = { "Walk", "Attack", "Defend", "Hit" };
	//当前播放的动画序号
	int animIndex = -1;

	//设置为所选单位
	public void Set(UnitType _type, int _num = 0)
	{
		//设置背景图
		image_bg.sprite = _type.race.sprite_bg;
		//设置单位动画
		animator.runtimeAnimatorController = _type.animControl;

		//设置单位数量（如果需要的话）
		if(text_num != null && _num != 0)
		{
			text_num.text = _num.ToString();
		}
		else
		{
			text_num.text = "";
		}

		//播放随机动画
		StopAllCoroutines();
		StartCoroutine(KeepPlayingRandomAnim());
	}

	//播放随机动画
	void PlayRandomAnim()
	{
		//随机一个数
		int random = Random.Range(0, baseAnim.Length);
		//如果是当前播放动画序号则设为random加1，否则赋值
		animIndex = random != animIndex ? random : (random + 1) % baseAnim.Length;

		//移动比较特殊
		if (baseAnim[animIndex] == "Walk")
		{
			animator.SetBool("walking", true);
			return;
		}

		//播放动画
		animator.SetBool("walking", false);
		animator.Play(baseAnim[animIndex]);
	}
	//持续播放随机动画
	IEnumerator KeepPlayingRandomAnim()
	{
		while (gameObject.activeSelf)
		{
			yield return new WaitForSeconds(Random.Range(2f, 2.5f));

			if (gameObject.activeSelf)
				PlayRandomAnim();
		}
	}
}
