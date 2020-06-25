using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class s_LevelButton : MonoBehaviour {

	public s_MenuManager menuManagerScript;
	public string sceneName;
	public bool levelLocked;
	public Button levelButton;
	public int chapterNumber;
	public int levelNumber;
	
	private s_GameController gameControllerScript;

	void Start ()
	{
		gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();

		if (menuManagerScript == null) {
			menuManagerScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<s_MenuManager>();
		}
		if (levelButton == null) {
			levelButton = transform.GetComponent<Button>();
		}

		//UnlockLevel();
	}

	public void LoadLevel()
	{
/*		if (gameControllerScript.debugMode == true)
		{*/
			//menuManagerScript.LoadLevel (sceneName);
			s_GameController.allowStartZoom = true;
/*		}
		else
		{
			if (PlayerData.current.PlayerEnergyLeft() > 0)
			{
				PlayerData.current.ConsumePlayerEnergy();
				SaveLoadData.Save();
				//menuManagerScript.LoadLevel (sceneName);
				s_GameController.allowStartZoom = true;
			}
		}*/
	}

/*	void UnlockLevel()
	{
		if (gameControllerScript.debugMode == true)
		{
			levelButton.interactable = true;
		}
		else if (chapterNumber < PlayerData.current.passedChapter)
		{
			levelButton.interactable = true;
		}
		else if (levelNumber <= PlayerData.current.passedLevel)
		{
			levelButton.interactable = true;
		}
	}*/
}
