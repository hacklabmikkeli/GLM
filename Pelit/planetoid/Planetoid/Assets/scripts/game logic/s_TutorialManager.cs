using UnityEngine;
using System.Collections;

public class s_TutorialManager : MonoBehaviour {

	public bool jumpTutorial;
	public bool panTutorial;
	public bool zoomTutorial;

	public s_GameController gameCtrl;
	public GameObject jumpTut;
	public GameObject panTut;
	public GameObject zoomTut;

	// Use this for initialization
	void Start () {
		if (gameCtrl == null) {
			this.gameCtrl = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();
		}
		JumpTutorial();
		PanTutorial();
		ZoomTutorial ();
	}

	public void JumpTutorial()
	{
		if (jumpTutorial) {
			jumpTut.SetActive (true);
		}
		else {
			jumpTut.SetActive (false);
		}
	}

	public void PanTutorial()
	{
		if (panTutorial) {
			panTut.SetActive (true);
		}
		else {
			panTut.SetActive (false);
		}
	}

	public void ZoomTutorial()
	{
		if (zoomTutorial) {
			zoomTut.SetActive (true);
		}
		else {
			zoomTut.SetActive (false);
		}
	}
}
