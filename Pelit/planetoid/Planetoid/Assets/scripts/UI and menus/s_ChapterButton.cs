using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class s_ChapterButton : MonoBehaviour {

	public s_MenuManager menuManagerScript;
	private s_GameController gameControllerScript;
	public Button chapterButton;
	public GameObject levelSelect;
	public int chapterNumber;

	// Use this for initialization
	void Start ()
	{
		gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();

		if (menuManagerScript == null) {
			menuManagerScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<s_MenuManager>();
		}
		if (chapterButton == null) {
			chapterButton = transform.GetComponent<Button>();
		}

		//UnlockChapterButton();
	}

	/*public void OpenLevelSelect()
	{
		menuManagerScript.OpenLevelSelect (levelSelect);
	}*/

/*	void UnlockChapterButton()
	{
		if (gameControllerScript.debugMode == true)
		{
			chapterButton.interactable = true;
		}
		else if (chapterNumber <= PlayerData.current.passedChapter)
		{
			chapterButton.interactable = true;
		}
	}*/
}