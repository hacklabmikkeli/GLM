using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class Level that is used to store information about level like
/// the amount of scrap collected from the level or information about
/// tasks in level.
/// </summary>

[Serializable]
public class Level
{
	private int ID;
	private DateTime scrapCollectionTime;
	private int jumpCount;
	private bool isCompleted;
	private List<Task> tasks;
	private List<int> collectedScraps;

	/// <summary>
	/// Constructor that gives level ID and tasks.
	/// </summary>
	/// <param name="levelID">LevelID to set for the level.</param>
	/// <param name="levelTasks">Level tasks to set for the level.</param>
	public Level(int levelID, List<s_TaskGeneral> levelTasks)
	{
		ID = levelID;
		tasks = new List<Task>();
		AddLevelTasks(levelTasks);
		collectedScraps = new List<int>();
	}

	/// <summary>
	/// Returns the name of the level as a string.
	/// </summary>
	public int ReturnLevelID()
	{
		return ID;
    }

	/// <summary>
	/// Sets level as completed.
	/// </summary>
	public void SetAsCompleted()
	{
		isCompleted = true;
	}

	/// <summary>
	/// Checks if level is completed.
	/// </summary>
	/// <returns><c>true</c> if this instance is completed; otherwise, <c>false</c>.</returns>
	public bool IsCompleted()
	{
		return isCompleted;
	}

	/// <summary>
	/// Adds tasks to level.
	/// </summary>
	/// <param name="levelTasks">Tasks to add to level.</param>
	public void AddLevelTasks(List<s_TaskGeneral> levelTasks)
	{
		if (tasks.Count == 0)
		{
			for (int i = 0; i < levelTasks.Count; i++)
			{
				tasks.Add(new Task(levelTasks[i].taskID, levelTasks[i].taskName, levelTasks[i].scrapFromTask));
			}
		}
	}

	/// <summary>
	/// Returns the tasks in level.
	/// </summary>
	/// <returns>The tasks in level.</returns>
	public List<Task> ReturnTasksInLevel()
	{
		return tasks;
	}

	/// <summary>
	/// Sets specific task as completed.
	/// </summary>
	/// <param name="taskID">Task ID to set as completed.</param>
	public void SetTaskAsCompleted(int taskID)
	{
		Task task = ReturnTaskByID(taskID);
		task.SetComplete();
	}

	/// <summary>
	/// Determines whether this level has a certain task completed with the specified taskID.
	/// </summary>
	/// <returns><c>true</c> if this level has the task completed with the specified taskID; otherwise, <c>false</c>.</returns>
	/// <param name="scrapID">Scrap I.</param>
	public bool IsTaskCompleted(int taskID)
	{
		Task task = ReturnTaskByID(taskID);
		return task.IsComplete();
	}

	/// <summary>
	/// Returns the with specific ID.
	/// </summary>
	/// <returns>Task with specific ID.</returns>
	/// <param name="taskID">TaskID to find.</param>
	Task ReturnTaskByID(int taskID)
	{
		Task task = null;

		for (int i = 0; i < tasks.Count; i++)
		{
			if (tasks[i].GetID() == taskID)
			{
				task = tasks[i];
			}
		}

		return task;
	}

	/// <summary>
	/// Adds the scrap to the list of collected scraps in certain level.
	/// </summary>
	/// <param name="scrapID">Unique index number of the scrap to be added to the collected list.</param>
	public void AddScrap(int scrapID)
	{
		//We first check if scrap is already in the list. (This should not happen ever, just a precaution.)
		if (collectedScraps.Contains(scrapID) == false)
		{
			//Checks if collectedScraps list is empty for this list and if it is sets the levels scrap reset timer to current time.
			if (collectedScraps.Count == 0)
			{
				scrapCollectionTime = DateTime.Now;
			}

			collectedScraps.Add(scrapID);
		}
	}

	/// <summary>
	/// Empties the collectedScraps list.
	/// </summary>
	public void ClearScraps()
	{
		collectedScraps.Clear();
	}

	/// <summary>
	/// Checks if scrap in level has been collected and if it should be deleted or not.
	/// </summary>
	/// <returns><c>true</c>, if collected scrap was deleted, <c>false</c> otherwise.</returns>
	/// <param name="scrapID">Scrap I.</param>
	/// <param name="resetTime">Reset time.</param>
	public bool DeleteCollectedScrap(int scrapID, float resetTime)
	{
		bool shouldDelete = false;
		System.TimeSpan timeDifference;

		if (collectedScraps.Count > 0)
		{
				timeDifference = System.DateTime.Now - CheckCollectionTime();

				if (timeDifference.TotalMinutes <= resetTime)
				{
					if (IsScrapCollected(scrapID) == true)
					{
						shouldDelete = true;
					}
				}
				else
				{
					ClearScraps();
				}
		}

		return shouldDelete;
	}

	/// <summary>
	/// Determines whether this level has a certain scrap collected with the specified scrapID.
	/// </summary>
	/// <returns><c>true</c> if this level has scrap collected with the specified scrapID; otherwise, <c>false</c>.</returns>
	/// <param name="scrapID">Scrap I.</param>
	public bool IsScrapCollected(int scrapID)
	{
		bool scrapFound = false;

		if (collectedScraps.Contains(scrapID) == true)
		{
			scrapFound = true;
		}

		return scrapFound;
	}

	/// <summary>
	/// Returns the amount of scraps collected in level.
	/// </summary>
	/// <returns>The scrap amount collected.</returns>
	public int ScrapAmountCollected()
	{
		return collectedScraps.Count;
	}

	/// <summary>
	/// Checks the collection time of the first collected scrap in level.
	/// </summary>
	/// <returns>The collection time for the first scrap.</returns>
	public DateTime CheckCollectionTime()
	{
		return scrapCollectionTime;
	}

	/// <summary>
	/// Adds to jump count.
	/// </summary>
	public void AddToJumpCount()
	{
		jumpCount++;
	}

	/// <summary>
	/// Returns the jump count in level.
	/// </summary>
	/// <returns>The jump count in level.</returns>
	public int JumpCountInLevel()
	{
		return jumpCount;
	}

	/// <summary>
	/// Clears the jump count.
	/// </summary>
	public void ClearJumpCount()
	{
		jumpCount = 0;
	}
}
