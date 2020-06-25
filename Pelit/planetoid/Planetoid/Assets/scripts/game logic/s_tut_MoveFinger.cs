using UnityEngine;
using System.Collections;

public class s_tut_MoveFinger : MonoBehaviour {

	public s_TutorialManager tutMngr;
	public s_GameController gameCtrl;
	public GameObject finger;
	public GameObject player;
	public GameObject startPoint;
	public GameObject endPoint;
	public float speed = 0.05f;
	public bool jumpTutorial;
	public bool panTutorial;
	
	// Use this for initialization
	void Start () {
		if (gameCtrl == null) {
			this.gameCtrl = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();
		}
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		if (tutMngr == null) {
			tutMngr = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<s_TutorialManager>();
		}
		if (jumpTutorial) {
			startPoint = player;
		}
		finger.transform.position = startPoint.transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameCtrl.currentGameState == GameState.PlayingLevel) {
			finger.SetActive(true);
			if (finger.transform.position != endPoint.transform.position) {
				MoveFinger ();
			}
			else {
				finger.transform.position = startPoint.transform.position;
			}
			if (player.GetComponent<s_PlayerControls>().state != PlayerState.Grounded) {
				tutMngr.jumpTutorial = false;
				tutMngr.JumpTutorial();
			}
		}
		
		
	}
	void MoveFinger()
	{
		finger.transform.position = Vector3.MoveTowards(finger.transform.position, endPoint.transform.position, speed);
	}
}
