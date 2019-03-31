using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundCategory { Effect, Music, Vox, Walk, UI, Impact }

public class SoundManager : Singleton<SoundManager>
{
    //同时播放的最大同种音效数量
    public int maxSameAudioPlayCountAtOneTime = 5;

    public AudioMixerGroup audioGroup_music;
    public AudioMixerGroup audioGroup_effect;

	//各声音类别的声道数量
	[HideInInspector]
	public int[] soundCategoryTrackNum = new int[System.Enum.GetValues(typeof(SoundCategory)).Length];
	//音效类别和播放器的字典
	Dictionary<SoundCategory, List<AudioSource>> soundCategoryDic;

	//初始化
	void Awake()
    {
		soundCategoryDic = new Dictionary<SoundCategory, List<AudioSource>>();

		//为每种声音创造其播放器组
		for (int i = 0; i < System.Enum.GetValues(typeof(SoundCategory)).Length; i++)
		{
			List<AudioSource> sources = new List<AudioSource>();

			for (int j = 0; j < soundCategoryTrackNum[i]; j++)
			{
				sources.Add(gameObject.AddComponent<AudioSource>());
			}

			soundCategoryDic.Add((SoundCategory)i, sources);
		}
    }

    //播放音效
    public void PlaySound(Sound _sound)
    {
        if (_sound.clips == null || _sound.clips.Length == 0 || _sound.clips[0] == null)
            Debug.LogError("空的声音：" + _sound.name);

        //获取闲置的声音组件并初始化
        AudioSource source = GetAvailableSource(_sound.category);
        if (source == null)
            return;

        InitAudioSource(source, _sound);

        //如果有多个声音片段则随机选择
        if (_sound.clips.Length > 1)
        {
            int random = Random.Range(0, _sound.clips.Length);
            source.clip = _sound.clips[random];
        }
        else
        {
            source.clip = _sound.clips[0];
        }

        //播放
        source.Play();

        StartCoroutine(FinishPlaying(source));
    }
    public void PlaySound(string _name)
    {
        PlaySound(GetSound(_name));
    }

    //初始化声音组件
    void InitAudioSource(AudioSource _source, Sound _sound)
    {
        _source.volume = _sound.volume;
        _source.pitch = _sound.pitch;
        _source.loop = _sound.loop;

        //设置声音组
        if (_sound.category == SoundCategory.Music)
            _source.outputAudioMixerGroup = audioGroup_music;
        else
            _source.outputAudioMixerGroup = audioGroup_effect;

        //开始时间
        _source.time = _sound.startingTime;
    }

    //通过名字获取声音
    Sound GetSound(string _name)
    {
        Sound[] sounds = Resources.LoadAll<Sound>("ScriptableObject/Sound");

        foreach (Sound item in sounds)
        {
            if (item.name == _name)
                return item;
        }
        Debug.LogWarning("未能找到：" + _name);
        return null;
    }

	//停止某个类别的声音
	public void StopCategory(SoundCategory _Cat)
	{
		foreach (var item in soundCategoryDic[_Cat])
		{
			item.Stop();
		}
	}

    //停止播放某种声音
    //public void StopPlay(Sound _sound)
    //{
    //    List<AudioSource> sourceList = new List<AudioSource>();
    //    foreach (var item in soundDic)
    //    {
    //        if (item.Value == _sound)
    //        {
    //            sourceList.Add(item.Key);
    //        }
    //    }

    //    foreach (var item in sourceList)
    //    {
    //        item.Stop();

    //        RemoveSound(item);
    //    }
    //}
    //public void StopPlay(string _name)
    //{
    //    StopPlay(GetSound(_name));
    //}

    //获取闲置的音效播放器
    AudioSource GetAvailableSource(SoundCategory _Cat)
    {
		//找到音效类别里没在播放的播放器
		List<AudioSource> sources = soundCategoryDic[_Cat];
		for (int i = 0; i < sources.Count; i++)
		{
			if (!sources[i].isPlaying)
				return sources[i];
		}

		//如果没有闲置的，返回第一个
        return sources[0];
    }

    //等待音效播放完毕
    IEnumerator FinishPlaying(AudioSource _source)
    {
        yield return new WaitForSeconds(_source.clip.length - _source.time);

        //RemoveSound(_source);
    }

}
