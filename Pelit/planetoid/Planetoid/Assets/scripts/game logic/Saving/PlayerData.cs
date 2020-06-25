using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// This class is used to store all the information about the game that
/// we want to save for when player starts the game next time.
/// </summary>

[Serializable]
public class PlayerData
{
	public static PlayerData current = new PlayerData();

	private int jumpCount;
	private int scrapCollected;
	private int nextLevelID;
	private List<Level> levels;


	//Character skins
	public int equippedSkin;
	public int equippedHead;
	public int equippedBody;
	public int equippedLegs;
	public bool[] boughtSkins;
	//public List<int> boughtSkins;
	//public List<int> availableSkins;

	//Jetpack
	//General
	public bool jetpackUnlocked;
	public bool jetpackEquipped;
	//Power
	public int currentPowerPrice;
	public int powerUpgradeLevel;
	public float jetpackPower;
	//Fuel
	public int currentFuelPrice;
	public int fuelUpgradeLevel;
	public float jetpackDuration;

	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerData"/> class.
	/// </summary>
	public PlayerData()
	{
		jumpCount = 0;

		//Scrap
		scrapCollected = 100;

		//Levels
		levels = new List<Level>();
		nextLevelID = 1;

		//Character skins
		equippedSkin = 0;
		equippedHead = 0;
		equippedBody = 0;
		equippedLegs = 0; 
		boughtSkins = new bool[8];
		//boughtSkins = new List<int>();
		//availableSkins = new List<int>();

		for (int i = 0; i < boughtSkins.Length; i++)
		{
			if (i == 0)
			{
				boughtSkins[i] = true;
			}
			else
			{
				boughtSkins[i] = false;
			}
		}

		//Jetpack
		//General
		jetpackUnlocked = false;
		jetpackEquipped = false;
		//Power
		currentPowerPrice = 50;
		powerUpgradeLevel = 0;
		jetpackPower = 5;
		//Fuel
		currentFuelPrice = 50;
		fuelUpgradeLevel = 0;
		jetpackDuration = 1;
	}

	//LEVEL FUNCTIONS

	/// <summary>
	/// Adds the visited level to save file.
	/// </summary>
	/// <param name="levelName">Level name to add.</param>
	/// <param name="levelID">ID number for the level to add.</param>
	public void AddVisitedLevel(string levelName, int levelID, List<s_TaskGeneral> tasks)
	{
		Level lvl = ReturnLevelByID(levelID);

		if (lvl == null)
		{
			levels.Add(new Level(levelID, tasks));
		}
	}

	/// <summary>
	/// Marks level as cleared.
	/// </summary>
	/// <param name="LevelName">Level name to be marked as cleared.</param>
	public void SetLevelCompleted(int levelID)
	{
		Level lvl = ReturnLevelByID(levelID);

		if (lvl != null)
		{
			lvl.SetAsCompleted();
			nextLevelID = levelID + 1;
		}
	}

	/// <summary>
	/// Returns the highests level passed.
	/// </summary>
	/// <returns>Index of the highest passed level.</returns>
	public int HighestLevelPassed()
	{
		return nextLevelID - 1;
	}

	/// <summary>
	/// This function returns the ID for the next available level.
	/// </summary>
	/// <returns>ID of the next level</returns>
	public int GetNextLevelID()
	{
		return nextLevelID;
	}

	/// <summary>
	/// This function sets the ID of the available level.
	/// </summary>
	/// <param name="id">New ID for the next level.</param>
	public void SetNextLevelID(int id)
	{
		nextLevelID = id;
	}

	//TASK FUNCTIONS

	/// <summary>
	/// Returns all tasks in specific level.
	/// </summary>
	/// <returns>All tasks in level.</returns>
	/// <param name="levelID">Index of the level to return tasks from.</param>
	public List<Task> GetLevelTasks(int levelID)
	{
		Level lvl = ReturnLevelByID(levelID);

		if (lvl != null)
		{
			return lvl.ReturnTasksInLevel();
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// Sets the task as completed.
	/// </summary>
	/// <param name="nextLevelID">Next level I.</param>
	/// <param name="taskID">Task I.</param>
	public void SetTaskAsCompleted(int levelID, int taskID)
	{
		Level lvl = ReturnLevelByID(levelID);

		if (lvl != null)
		{
			lvl.SetTaskAsCompleted(taskID);
		}
	}

	/// <summary>
	/// Checks if a task is completed in certain level.
	/// </summary>
	/// <returns><c>true</c> if task is completed with the taskID; otherwise, <c>false</c>.</returns>
	/// <param name="levelID">LevelID if the level from where the task is checked.</param>
	/// <param name="taskID">ID of the task to check.</param>
	public bool IsTaskCompleted(int levelID, int taskID)
	{
		Level lvl = ReturnLevelByID(levelID);
		bool isCompleted = false;

		if (lvl != null)
		{
			isCompleted = lvl.IsTaskCompleted(taskID);
		}

		return isCompleted;
	}

	//SCRAP FUNCTIONS

	/// <summary>
	/// Use this to add scrap to player.
	/// </summary>
	/// <param name="scrapAmount">Scrap amount to add.</param>
	public void AddScrapPoints(int scrapAmount)
	{
		scrapCollected += scrapAmount;
	}

	/// <summary>
	/// Use this to remove scrap from player.
	/// </summary>
	/// <param name="scrapAmount">Scrap amount to remove</param>
	public void RemoveScrapPoints(int scrapAmount)
	{
		scrapCollected -= scrapAmount;
	}

	/// <summary>
	/// Adds the collected scrap.
	/// </summary>
	/// <param name="levelName">Level name.</param>
	/// <param name="scrapID">Scrap I.</param>
	public void AddCollectedScrap(int levelID, int scrapID)
	{	
		Level lvl = ReturnLevelByID(levelID);

		if (lvl != null)
		{
			lvl.AddScrap(scrapID);
		}
	}

	/// <summary>
	/// Deletes the collected.
	/// </summary>
	/// <returns><c>true</c>, if collected was deleted, <c>false</c> otherwise.</returns>
	/// <param name="levelName">Level name.</param>
	/// <param name="scrapID">Scrap I.</param>
	/// <param name="resetTime">Reset time.</param>
	public bool DeleteCollected(int levelID, int scrapID, float resetTime)
	{
		Level lvl = ReturnLevelByID(levelID);
		bool shouldDelete = false;

		if (lvl != null)
		{
			shouldDelete = lvl.DeleteCollectedScrap(scrapID, resetTime);
		}

		return shouldDelete;
	}

	/// <summary>
	/// Returns scrap amount collected from specific level.
	/// </summary>
	/// <returns>The amount collected.</returns>
	/// <param name="levelID">Level I.</param>
	public int ScrapAmountCollected(int levelID)
	{
		Level lvl = ReturnLevelByID(levelID);
		int scrapAmount = 0;

		if (lvl != null)
		{
			scrapAmount = lvl.ScrapAmountCollected();
		}

		return scrapAmount;
	}

	/// <summary>
	/// Returns total scrap amount collected.
	/// </summary>
	/// <returns>Amount of total scrap collected.</returns>
	public int GetTotalScrapCollected()
	{
		return scrapCollected;
	}

	/// <summary>
	/// Contains level with certain ID
	/// </summary>
	/// <returns>Index of the level found. If not found, returns -1</returns>
	/// <param name="levelID">Level ID to search for.</param>
	public Level ReturnLevelByID(int levelID)
	{
		int levelIndex = -1;
		
		for (int i = 0; i < levels.Count; i++)
		{
			if (levels[i].ReturnLevelID() == levelID)
			{
                levelIndex = i;
                break;
            }
        }
        
		if (levelIndex == -1)
		{
			return null;
		}
		else
		{
			return levels[levelIndex];
		}
        
	}

	/// <summary>
	/// Returns the jumpcount in certain level.
	/// </summary>
	/// <returns>The jump count in level. If level is not found in levels list, returns 0.</returns>
	/// <param name="levelName">Level name to check the jump count from.</param>
	public int JumpCountInLevel(int levelID)
	{
		Level lvl = ReturnLevelByID(levelID);
		int jumpCountInLevel = 0;

		if (lvl != null)
		{
			jumpCountInLevel = lvl.JumpCountInLevel();
		}

		return jumpCountInLevel;
	}

	/// <summary>
	/// Adds to level jump count.
	/// </summary>
	/// <param name="levelName">Level name to add the jumps.</param>
	public void AddToJumpCount(int levelID)
	{
		jumpCount++;

		Level lvl = ReturnLevelByID(levelID);

		if (lvl != null)
		{
			lvl.AddToJumpCount();
		}
	}

	/// <summary>
	/// Clears the jump count of a specific level.
	/// </summary>
	/// <param name="levelName">Level name to to clear the jumps from.</param>
	public void ClearLevelJumpCount(int levelID)
	{	
		Level lvl = ReturnLevelByID(levelID);

		if (lvl != null)
		{
			lvl.ClearJumpCount();
		}
	}

	public void ChangeEquippedSkin(int head, int body, int legs)
	{
		equippedHead = head;
		equippedBody = body;
		equippedLegs = legs;
	}
}
