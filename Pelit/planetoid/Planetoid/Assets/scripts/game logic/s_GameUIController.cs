using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class s_GameUIController : MonoBehaviour
{
	//This could be used to raise and event every time nextlevel button is pressed.
	public delegate void NextLevelPress();
	public static event NextLevelPress OnNextLevelButtonPress;

	public GameObject pauseScreenPanel;
	public Text pauseScreenTitle;

	[Header("Buttons")]
	public Button pauseButton;
	public Button playButton;
	public Button restartButtonUI;
	public Button restartButtonPause;
	public Button menuButton;
	public Button nextLevelButton;

	//Task related objects
	[Header("Tasks")]
	public Text[] taskTitles;

	[Header("Texts")]
	public Text scrapText;
	public Text levelNameText;
	public Text energyText;
	public Text energyTextPauseMenu;
	public Text versionNumber;

	//public Text taskTitle;

	private s_GameController gameControllerScript;
	//private s_DataSaving dataSaving;

	public Dictionary<string, Text> taskToUIConnection = new Dictionary<string, Text>();

	public string levelName;
	public int levelID;

	//private s_PlayerEnergyController energyScript;
	private s_PointGiverController pointGiverScript;
	private s_PlayerControls playerControl;


	public Toggle jetpackToggle;
	public GameObject pointsText;
	public GameObject jetpackObject;

	void Start()
	{
		gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();
		//energyScript = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerEnergyController>();
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerControls>();

		if (GameObject.FindGameObjectWithTag("PointGiver") != null)
		{
			pointGiverScript = GameObject.FindGameObjectWithTag("PointGiver").GetComponent<s_PointGiverController>();
		}

		UpdateScrapCounter(PlayerData.current.GetTotalScrapCollected());

		levelNameText.text = Application.loadedLevelName;
		levelName = Application.loadedLevelName;
		levelID = Application.loadedLevel;

		jetpackToggle.isOn = PlayerData.current.jetpackEquipped;
		jetpackToggle.gameObject.SetActive (PlayerData.current.jetpackUnlocked);

		UpdateLevelText ();
		UpdateVersionNumber ();

		ActivateJetpack();
	}

	void ActivateJetpack()
	{
		int i = Application.loadedLevel; //Get the scene id in buildsettings
		int nextLevelID = i + 1;

		if (nextLevelID == 6)
		{
			PlayerData.current.jetpackUnlocked = true;
			SaveLoadData.Save();
		}
	}

	void OnEnable()
	{
		s_GameController.OnStateChange += OnStateChange;
		s_scrapController.OnScrapCollection += UpdateScrapCounter;
	}

	void OnDisable()
	{
		s_GameController.OnStateChange -= OnStateChange;
		s_scrapController.OnScrapCollection -= UpdateScrapCounter;
	}

	void OnStateChange(GameState newState)
	{
		if (newState == GameState.FailedLevel || newState == GameState.PassedLevel || newState == GameState.PausedLevel)
		{
			SetUpPauseScreen(newState);
			OpenPauseScreen();
		}
	}

	public void TogglePowerUp()
	{
		if (playerControl.JetpackEquipped)
		{
			playerControl.JetpackEquipped = false;
			PlayerData.current.jetpackEquipped = false;
		}
		else
		{
			playerControl.JetpackEquipped = true;
			PlayerData.current.jetpackEquipped = true;
		}

		SaveLoadData.Save();

		if (jetpackObject && jetpackObject.activeInHierarchy)
		{
			jetpackObject.SetActive(false);
		}
	}

	public void PauseButtonPressed()
	{
		SaveLoadData.Save();
		// pause game, no player controls allowed, game state to menu, hide pause/disable pausebutton, show pause menu.
		gameControllerScript.ChangeGameState(GameState.PausedLevel);
		ChangeButtonState();
	}

	public void PlayButtonPressed()
	{
		pauseScreenPanel.SetActive(false);
		gameControllerScript.ChangeGameState(GameState.PlayingLevel);
		ChangeButtonState();
	}

	public void RestartButtonPressed()
	{
		SaveLoadData.Save();
		this.gameControllerScript.ResetLevel();
	}

	public void MenuButtonPressed()
	{
		s_SoundScript.audioSource.PlayMenuMusic();
		this.gameControllerScript.LoadMainMenu();
	}
	
	public void NextLevelButtonPressed()
	{
		if (OnNextLevelButtonPress != null)
		{
			OnNextLevelButtonPress();
		}
		SaveLoadData.Save();
		this.gameControllerScript.LoadNextLevel();
	}

	public void DebugNextLevelButtonPressed()
	{
		if (OnNextLevelButtonPress != null)
		{
			OnNextLevelButtonPress();
		}
		this.gameControllerScript.LoadNextLevel();
	}

	public void DebugPreviousLevelButtonPressed()
	{
		if (OnNextLevelButtonPress != null)
		{
			OnNextLevelButtonPress();
		}
		int i = Application.loadedLevel; //Get the scene id in buildsettings
		Application.LoadLevel(i - 1);
	}

	public void OpenPauseScreen()
	{
		pauseScreenPanel.SetActive(true);
	}

	public void ChangeButtonState()
	{
		if (gameControllerScript.CurrentGameState() == GameState.PausedLevel)
		{
			pauseButton.gameObject.SetActive(false);
			playButton.gameObject.SetActive(true);
			restartButtonUI.gameObject.SetActive(false);
		}
		else if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
		{
			pauseButton.gameObject.SetActive(true);
			playButton.gameObject.SetActive(false);
			restartButtonUI.gameObject.SetActive(true);
		}
		else
		{
			pauseButton.gameObject.SetActive(false);
			playButton.gameObject.SetActive(false);
			restartButtonUI.gameObject.SetActive(false);
		}
	}

	void SetUpPauseScreen(GameState newState)
	{
		switch (newState)
		{
		case GameState.PassedLevel:

			nextLevelButton.gameObject.SetActive(true);
			ChangeButtonState();
			pauseScreenTitle.text = "Level " + levelID + " Cleared!";

			break;
		case GameState.FailedLevel:
			// Find the state of death from the player script
			s_PlayerControls playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerControls>();
			CauseOfDeath currentState = playerScript.deathState;
			
			switch(currentState) {
			case CauseOfDeath.Boundary:
				// Death by map boundaries
				pauseScreenTitle.text = "Lost in space, forever and ever";
				break;
			case CauseOfDeath.Burn:
				// Death by burning planet
				pauseScreenTitle.text = "You're toast!";
				break;
			case CauseOfDeath.Oxygen:
				// Death by lack of oxygen
				pauseScreenTitle.text = "You're out of breath!";
				break;
			case CauseOfDeath.Projectile:
				// Death by a projectile
				pauseScreenTitle.text = "Boom headshot!";
				break;
			default:
				// This shouldn't happen, but if it does, show default death text
				pauseScreenTitle.text = "You're spacejunk!";
				break;
			}

			nextLevelButton.gameObject.SetActive(false);
			ChangeButtonState();
			break;
		case GameState.PausedLevel:
			nextLevelButton.gameObject.SetActive(false);
			pauseScreenTitle.text = "Paused!";
			break;
		}
	}

	public void SetUpTaskTitles(List<GameObject> tasksInScene)
	{
		for (int i = 0; i < tasksInScene.Count; i++)
		{
			int taskID = tasksInScene[i].GetComponent<s_TaskGeneral>().taskID;
			int taskValue = tasksInScene[i].GetComponent<s_TaskGeneral>().scrapFromTask;

			taskToUIConnection.Add(tasksInScene[i].name, taskTitles[i]);
			taskTitles[i].text = tasksInScene[i].name;
			taskTitles[i].transform.parent.gameObject.SetActive(true);

			taskTitles[i].transform.parent.FindChild("PointText").GetComponent<s_TaskPointController>().taskPointText.text = "+" + taskValue.ToString();

			//TODO: Check if task completed previously

			levelID = SceneManager.GetActiveScene().buildIndex;

			if (PlayerData.current.IsTaskCompleted(levelID, taskID) == true)
			{
				SetAsCompleted(tasksInScene[i]);
			}
		}
	}

	public void SetAsCompleted(GameObject task)
	{
		Toggle taskToggle = taskToUIConnection[task.name].transform.parent.GetComponent<Toggle>();
		taskToggle.isOn = true;

		int taskValue = task.GetComponent<s_TaskGeneral>().scrapFromTask;

		Transform pointText = taskToggle.transform.FindChild("PointText");

		if (pointGiverScript != null)
		{
			pointGiverScript.ShowTaskPoints(pointText, taskValue);
		}

	}

	public void UpdateScrapCounter(int scrapAmount)
	{
		scrapText.text = scrapAmount.ToString();
	}

	public void UpdateLevelText()
	{
		levelNameText.text = "Level "+ levelID.ToString ();
	}

	public void UpdateEnergyCounter(int energyAmount)
	{
		energyText.text = energyAmount.ToString() + "/5";
	}

	public void UpdateVersionNumber()
	{
		versionNumber.text = "WIP Alpha v" + Application.version;
	}
}
