using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// GameController is used throughout the game and handles, as the name states, controlling
/// of the game. For example changing the state of the game from paused to playing etc.
/// </summary>

public enum GameState { Menu, PausedLevel, PreparingGame, PlayingLevel, RestartLevel, FailedLevel, PassedLevel };

public class s_GameController : MonoBehaviour
{
	// Enable debug mode for the game. All levels are unlocked etc. when this is on.
	public bool debugMode;

	//This could be used to raise and event every time gamestate is changed.
	public delegate void StateAction(GameState newState);
	public static event StateAction OnStateChange;

	public static s_GameController gameControllerSingleton;
	public GameState currentGameState;

	//Used to decide if start zoom happens at the start of the level
	public static bool allowStartZoom = false;

	void Awake()
	{
		// Make this a singleton
		DontDestroyOnLoad(gameObject);
		
		// If we find another gamecontroller object, destroy it.
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		} 
		else 
		{
			// This is the only gamecontroller. Assign the script.
			gameControllerSingleton = this;
		}

		SaveLoadData.Load();

		ChangeGameState(GameState.PreparingGame);
	}

	public void ChangeGameState(GameState newState)
	{
		this.currentGameState = newState;

		if (OnStateChange != null)
		{
			OnStateChange(newState);
		}

		switch (currentGameState)
		{
		case GameState.PlayingLevel:
			ChangePauseState(false);
			break;
		case GameState.PassedLevel:
			SetLastClearedLevel();
			SaveLoadData.Save();
			ChangePauseState(true);
			break;
		case GameState.FailedLevel:
			SaveLoadData.Save();
			ChangePauseState(true);
			break;
		case GameState.PausedLevel:
			ChangePauseState(true);
			break;
		case GameState.Menu:
			SaveLoadData.Save();
			break;
		case GameState.PreparingGame:
			ChangePauseState(false);
			break;
		case GameState.RestartLevel:
			ChangePauseState(false);
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// Returns the state of the game.
	/// </summary>
	/// <returns>The game state.</returns>
	public GameState CurrentGameState()
	{
		return this.currentGameState;
	}

	/// <summary>
	/// Sets the last cleared level.
	/// </summary>
	void SetLastClearedLevel()
	{
		//PlayerData.current.SetLevelCompleted(Application.loadedLevelName);
		PlayerData.current.SetLevelCompleted(SceneManager.GetActiveScene().buildIndex);
		//PlayerData.current.nextLevelID = Application.loadedLevel + 1;
	}

	/// <summary>
	/// Loads the next level specified by the nextLevel string.
	/// TODO: Add chapter change and check so we don't try to load a level that doesn't exist.
	/// </summary>
	public void LoadNextLevel ()
	{
		ChangeGameState(GameState.PreparingGame);
		int i = Application.loadedLevel; //Get the scene id in buildsettings
		int nextLevelID = i + 1;
		if (nextLevelID > 20)
		{
			Application.LoadLevel(0);
			PlayerData.current.SetNextLevelID(1);
		}
		else
		{
			Application.LoadLevel(nextLevelID);
			allowStartZoom = true;
		}
		SaveLoadData.Save();
	}

	public void LoadMainMenu()
	{
		ChangeGameState(GameState.Menu);
		Application.LoadLevel(0);
	}

	public void ResetLevel ()
	{
		ChangeGameState(GameState.RestartLevel);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ChangePauseState(bool pause)
	{

		print("Changing state to " + pause + " " + currentGameState);
		if (pause == true)
		{
			Time.timeScale = 0;
		} 
		else  
		{
			Time.timeScale = 1;
		}

	}
}
