using UnityEngine;
using System.Collections;

public class s_PlayerGhost : MonoBehaviour {

	private s_PlayerFlightPath flightPathScript;


	void Start () {
		flightPathScript = transform.parent.GetComponent<s_PlayerFlightPath>();
	}

	/* Handles the collision on the Ghost gameobject.
	 * Calls for the parent script to handle the gravityarea list if hits a gravityarea.
	 * Calls for the parent script to handle resetting if the ghost hits another gameobject.
	 * */
	void OnTriggerEnter2D (Collider2D col) {
		//Debug.Log("Collided with: " + col.gameObject.name + ", " + col.gameObject.tag);
		if (col.gameObject.tag == "GravityArea") {
			if (!flightPathScript.gravityScriptList.Contains(col.transform.GetComponent<s_PlanetGravity>())) {
				//Debug.Log("<color=#ffa500ff>Adding to list</color>");
				flightPathScript.gravityScriptList.Add(col.transform.GetComponent<s_PlanetGravity>());
			}
		} else if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Meteor") {
			// If the collider hits something other than a gravity area, reset it.
			flightPathScript.ResetPlayerGhost();
			flightPathScript.LaunchGhost(flightPathScript.lastDirection);
		}
	}
}
