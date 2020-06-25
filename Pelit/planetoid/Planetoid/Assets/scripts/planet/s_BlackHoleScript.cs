using UnityEngine;
using System.Collections;

public class s_BlackHoleScript : MonoBehaviour {

	private s_GameController gameControllerScript;

	void Start()
	{
		gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" && gameControllerScript.CurrentGameState() == GameState.PlayingLevel) {
			gameControllerScript.ChangeGameState(GameState.PassedLevel);
			//gameControllerScript.ResetLevel();
			print ("You reached the black hole and completed the level!");

		}
	}
}
