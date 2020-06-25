using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using GameAnalyticsSDK;

public class s_AnalyticsManager : MonoBehaviour
{
	private s_PlayerControls playerScript;
	private string causeOfDeath;
	private float jumpCount;

	void OnEnable()
	{
		s_GameController.OnStateChange += LevelStatus;
	}

	void OnDisable()
	{
		s_GameController.OnStateChange -= LevelStatus;
	}

	void Start()
	{
		if (GameObject.FindGameObjectWithTag ("Player"))
		{
			playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<s_PlayerControls> ();
		}
	}

	public void LevelStatus (GameState newState)
	{
		// Get current scene name through SceneManager
		string sceneName = SceneManager.GetActiveScene().name;
		int levelID = SceneManager.GetActiveScene().buildIndex;

		switch (newState)
		{
		case GameState.PreparingGame:
			GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusStart, sceneName);
			break;
		case GameState.PassedLevel:
			jumpCount = (float)PlayerData.current.JumpCountInLevel(levelID);
			GameAnalytics.NewDesignEvent ("JumpsUsed:"+sceneName+":Completed", jumpCount);
			GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusComplete, sceneName);
			break;
		case GameState.FailedLevel:
			jumpCount = (float)PlayerData.current.JumpCountInLevel(levelID);
			GameAnalytics.NewDesignEvent ("JumpsUsed:"+sceneName+":Failed", jumpCount);
			causeOfDeath = playerScript.deathState.ToString();
			GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusFail, sceneName, causeOfDeath);
			break;
		case GameState.RestartLevel:
			GameAnalytics.NewDesignEvent ("LevelRestarts:"+sceneName, 1);
			break;
		default:
			break;
		}
	}
}
