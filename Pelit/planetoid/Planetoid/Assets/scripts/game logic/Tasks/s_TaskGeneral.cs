using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class s_TaskGeneral : MonoBehaviour
{
	public int taskID;
	public string taskName;
	public int scrapFromTask;

	void Awake()
	{
		taskName = gameObject.name;
	}

	public void AddScrapFromTask(GameObject taskTitle)
	{
		int levelID = SceneManager.GetActiveScene().buildIndex;

		if (taskTitle == gameObject && PlayerData.current.IsTaskCompleted(levelID, taskID) == false)
		{
			//Debug.Log(taskTitle.name);
			PlayerData.current.AddScrapPoints(scrapFromTask);
		}
	}
}
