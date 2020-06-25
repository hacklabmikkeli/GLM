using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script used for scene information.
/// Only function for this script is to provide information for other classes.
/// </summary>
public class s_SceneController : MonoBehaviour 
{
	public string nextLevel;
	public int chapterNumber;
	public int levelNumber;
	public bool isLastLevelInChapter;
	public float scrapResetTime;

	private List<s_TaskGeneral> tasks;

	void Start()
	{
		tasks = new List<s_TaskGeneral>();
		List<GameObject> temp = GameObject.FindGameObjectsWithTag("Task").ToList();

		for (int i = 0; i < temp.Count; i++)
		{
			tasks.Add(temp[i].GetComponent<s_TaskGeneral>());
		}

		AddLevelToSaveFile();
	}

	void AddLevelToSaveFile()
	{
		int sceneID = SceneManager.GetActiveScene().buildIndex;
		string sceneName = SceneManager.GetActiveScene().name;

		if (sceneID != 0)
		{
			PlayerData.current.AddVisitedLevel(sceneName, sceneID, tasks);
		}
	}
}
