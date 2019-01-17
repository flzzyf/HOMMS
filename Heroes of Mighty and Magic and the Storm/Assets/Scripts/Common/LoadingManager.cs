using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : Singleton<LoadingManager>
{
	public GameObject panel_loadingScreen;
	public Transform loadingBarParent;

	void Start()
	{
		panel_loadingScreen.SetActive(false);

		//隐藏所有进度条块
		for (int i = 0; i < loadingBarParent.childCount; i++)
		{
			loadingBarParent.GetChild(i).gameObject.SetActive(false);
		}
	}

	public void LoadScene()
	{
		panel_loadingScreen.SetActive(true);

		StartCoroutine(LoadLevelAsync());
	}

	IEnumerator LoadLevelAsync()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync("Travel");

		int currentProgress = 0;

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);

			for (int i = currentProgress; i < Mathf.FloorToInt(loadingBarParent.childCount * progress); i++)
			{
				loadingBarParent.GetChild(i).gameObject.SetActive(true);
			}

			currentProgress = Mathf.FloorToInt(loadingBarParent.childCount * progress);

			yield return null;
		}
	}
}
