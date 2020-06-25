using UnityEngine;
using System.Collections;

public class s_BoundaryReset : MonoBehaviour {

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player"){
			// Call the player script and kill the player
			s_PlayerControls playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerControls>();
			playerScript.KillPlayer(CauseOfDeath.Boundary);
		}
	}
}
