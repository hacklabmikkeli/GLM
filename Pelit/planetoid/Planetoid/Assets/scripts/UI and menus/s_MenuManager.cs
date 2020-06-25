using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameAnalyticsSDK;

/// <summary>
/// This script is used mainly for loading levels but 
/// it also updates version number text and resets progression.
/// </summary>

public class s_MenuManager : MonoBehaviour
{	
	public GameObject loadingScreen;
	public GameObject startButton;
	public GameObject continueButton;
	public Text versionNumber;

	private s_GameController gameController;
	private s_AnalyticsManager analyticsMngr;

	// Use this for initialization
	void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();

		if (GameObject.FindGameObjectWithTag("AnalyticsManager"))
		{
			this.analyticsMngr = GameObject.FindGameObjectWithTag("AnalyticsManager").GetComponent<s_AnalyticsManager>();
		}

		gameController.ChangeGameState (GameState.Menu);

		if (PlayerData.current.GetNextLevelID() > 1)
		{
			startButton.SetActive(false);
			continueButton.SetActive(true);
		}

		UpdateVersionNumber();
	}

	/// <summary>
	/// Loads the level using the sceneID.
	/// </summary>
	/// <param name="sceneID">SceneID to use for the level load.</param>
	public void LoadLevel(int sceneID)
	{
		/*if (analyticsMngr != null)
		{
			analyticsMngr.LevelStatus(sceneName, "selected");
		}*/

		loadingScreen.SetActive(true);
		gameController.ChangeGameState (GameState.PreparingGame);
		StartCoroutine (LoadAsync(sceneID));
	}

	/// <summary>
	/// Loads the level in background and informs when loading is done.
	/// This is for the loading screen.
	/// </summary>
	/// <returns>async</returns>
	/// <param name="sceneID">SceneID to use for the level load.</param>
	IEnumerator LoadAsync(int sceneID)
	{
		AsyncOperation async = Application.LoadLevelAsync(sceneID);
		yield return async;
		Debug.Log("Loading complete");
	}

	/// <summary>
	/// Should open first level when used first time. After first time
	/// should take the player to the point where they are in game. 
	/// </summary>
	public void Play()
	{
		int nextLevelID = PlayerData.current.GetNextLevelID();

		if (nextLevelID >= 11)
		{
			s_SoundScript.audioSource.PlayLevelMusic(1);
		}
		else
		{
			s_SoundScript.audioSource.PlayLevelMusic(0);
		}

		LoadLevel (nextLevelID);
		s_GameController.allowStartZoom = true;
	}

	/// <summary>
	/// Updates the version number text to current version.
	/// </summary>
	public void UpdateVersionNumber()
	{
		versionNumber.text = "WIP Alpha v" + Application.version;
	}

	/// <summary>
	/// Clears the save data.
	/// </summary>
	public void ClearSaveData()
	{
		startButton.SetActive(true);
		continueButton.SetActive(false);
		PlayerData.current = new PlayerData();
		SaveLoadData.Save();
	}
}
