using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class s_LevelInfoController : MonoBehaviour
{
	public Animator levelPopupAnimator;

	public Text levelName;
	public Text scrapAmount;
	public Text fastestTime;

	public Toggle[] tasks;

	private int levelID = 1;
	private s_MenuManager menuManagerScript;

	void Start()
	{
		menuManagerScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<s_MenuManager>();

		SetUpFirstLevelDetails();
	}

	void OnEnable()
	{
		s_RotateOnTouch.OnPlanetLock += OnPlanetlock;
		s_MenuCameraController.OnMoveToLevels += OpenLevelInfo;
	}

	void OnDisable()
	{
		s_RotateOnTouch.OnPlanetLock -= OnPlanetlock;
		s_MenuCameraController.OnMoveToLevels -= OpenLevelInfo;
	}

	void OpenLevelInfo()
	{
		levelPopupAnimator.SetBool("ToggleShow", true);
	}

	public void CloseLevelInfo()
	{
		//Debug.Log("Close");
		levelPopupAnimator.SetBool("ToggleShow", false);
	}

	void OnPlanetlock(s_LevelPlanetController lockedLevelScript)
	{
		if (lockedLevelScript.isUnlocked == true)
		{
			levelID = lockedLevelScript.levelNumber;
			levelName.text = lockedLevelScript.levelName;
			scrapAmount.text = lockedLevelScript.scrapAmount.ToString();
			fastestTime.text = "???";

			List<Task> tempTasks = PlayerData.current.GetLevelTasks(levelID);

			if (tempTasks != null)
			{
				for (int i = 0; i < tasks.Length; i++)
				{
					tasks[i].transform.FindChild("Label").GetComponent<Text>().text = tempTasks[i].GetName();
					tasks[i].isOn = tempTasks[i].IsComplete();
				}
			}
			else
			{
				for (int i = 0; i < tasks.Length; i++)
				{
					tasks[i].transform.FindChild("Label").GetComponent<Text>().text = "???";
					tasks[i].isOn = false;
				}
			}
			


			OpenLevelInfo();
		}

	}

	public void SetUpFirstLevelDetails()
	{
		levelName.text = "Level 1";
		scrapAmount.text = PlayerData.current.ScrapAmountCollected(1).ToString();
		fastestTime.text = "???";

		List<Task> tempTasks = PlayerData.current.GetLevelTasks(1);

		if (tempTasks != null)
		{
			for (int i = 0; i < tasks.Length; i++)
			{
				tasks[i].transform.FindChild("Label").GetComponent<Text>().text = tempTasks[i].GetName();
				tasks[i].isOn = tempTasks[i].IsComplete();
			}
		}
		else
		{
			for (int i = 0; i < tasks.Length; i++)
			{
				tasks[i].transform.FindChild("Label").GetComponent<Text>().text = "???";
				tasks[i].isOn = false;
			}
		}

			



		
		/*if (level != null)
		{
			levelName.text = "Level 1";
			scrapAmount.text = PlayerData.current.ScrapAmountCollected(1);
			fastestTime.text = "???";

			List<Task> tempTasks = PlayerData.current.GetLevelTasks(1);

			for (int i = 0; i < tasks.Length; i++)
			{
				tasks[i].transform.FindChild("Label").GetComponent<Text>().text = tempTasks[i].GetName();
				tasks[i].isOn = tempTasks[i].IsComplete();
			}
		}
		else
		{
			levelName.text = "Level 1";
			scrapAmount.text = "???";
			fastestTime.text = "???";

			for (int i = 0; i < tasks.Length; i++)
			{
				tasks[i].transform.FindChild("Label").GetComponent<Text>().text = "???";
				tasks[i].isOn = false;
			}
		}*/
	}

	public void PlayButtonPressed()
	{
		menuManagerScript.LoadLevel(levelID);
	}
}
