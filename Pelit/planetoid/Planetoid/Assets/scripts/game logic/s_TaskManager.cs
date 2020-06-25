using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class s_TaskManager : MonoBehaviour
{
	private s_GameUIController gameUIScript;
	private string levelName;
	private int levelID;

	public List<GameObject> tasks;

	void Start()
	{
		levelName = Application.loadedLevelName;
		levelID = Application.loadedLevel;

		tasks = GameObject.FindGameObjectsWithTag("Task").ToList();

		//This ensures that the game won't crash if more then three tasks are found in the scene.
		if (tasks.Count > 3)
		{
			Debug.Log("ONLY THREE TASKS PER SCENE!!! PLEASE DISABLE TASKS TO MATCH CORRECT AMOUNT!");
			int amountToRemove = tasks.Count - 3;
			tasks.RemoveRange(2, amountToRemove);
		}

		if (GameObject.FindGameObjectWithTag("GameUI") != null)
		{
			gameUIScript = GameObject.FindGameObjectWithTag("GameUI").GetComponent<s_GameUIController>();
			gameUIScript.SetUpTaskTitles(tasks);
		}

	}

	void OnEnable()
	{
		s_CountingTask.OnTaskComplete += OnTaskComplete;
		s_ConditionTask.OnTaskComplete += OnTaskComplete;
	}

	void OnDisable()
	{
		s_CountingTask.OnTaskComplete -= OnTaskComplete;
		s_ConditionTask.OnTaskComplete -= OnTaskComplete;
	}

	void OnTaskComplete (GameObject completedTask)
	{
		s_TaskGeneral generalTaskScript = completedTask.GetComponent<s_TaskGeneral>();
		int taskID = generalTaskScript.taskID;

		if (PlayerData.current.IsTaskCompleted(levelID, taskID) == false)
		{
			gameUIScript.SetAsCompleted(completedTask);
		}

		//TODO: Save completed task to playerdata
		generalTaskScript.AddScrapFromTask(completedTask);
		//PlayerData.current.AddCompletedTask(levelName, levelID, taskID);
		PlayerData.current.SetTaskAsCompleted(levelID, taskID);
		//Debug.Log("TASK COMPLETED");


	}
}
