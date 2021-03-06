﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    public string key;
    public string[] args;
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        LocalizationMgr.instance.localizationTexts.Add(this);

        Init();
    }

    public void ChangeToLanguage(Language _language)
    {
        LocalizationMgr.instance.LoadLanguage(_language);

        Init();
    }

    public void Init()
    {
        if (key == "")
            return;

		if (text == null)
			text = GetComponent<Text>();
		//
		text.font = LocalizationMgr.instance.font;

        if (args.Length == 0)
        {
            text.text = LocalizationMgr.instance.GetText(key);
        }
        else
        {
            text.text = string.Format(LocalizationMgr.instance.GetText(key), args);
        }
    }

    public void SetKey(string _key, params string[] _text)
    {
        args = _text;

		SetKey(_key);
    }

    public void SetKey(string _key)
    {
        key = _key;

        Init();
    }

	public void SetText(string _text)
	{
		if (text == null)
			text = GetComponent<Text>();
		//
		text.text = _text;
	}

    public void ClearText()
    {
		if(text == null)
			text = GetComponent<Text>();
		//
		text.text = "";
    }
}
