using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_FlightVector : MonoBehaviour {

	public GameObject obj;

	public LayerMask gravityLayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	public void UpdatePoints(Vector2 currentPos, Vector2 directionWithPower, float timeStep) {
		/* Take the current position
		 * Calculate direction from current position
		 * For each position:
		 * 	Check the new pos after with added timestep (lastPos + directionWithPower)
		 * 	Check if we're within a gravity area, get gravity area power
		 * 	Add power to new position towards gravity dir	(lastPos + directionWithPower) + gravityDir
		 *  Move the object to the calculated position 
		 * */

/*		List<Vector2> positions = new List<Vector2>();

		Vector2 _lastPos = currentPos;
		int _pointsInList = 3;

		for(int i = 0; i < _pointsInList; i++) {
			Vector2 newPos = _lastPos + directionWithPower;
*/
/*			Collider2D col = Physics2D.OverlapPoint(newPos, gravityLayer) * timeStep;

			if (col != null) {
				// The new point is inside a collider, get and apply gravity
				// Get gravity from planet
				float force = col.GetComponent<s_PlanetGravity>().pullForce;
				Vector2 gDir = col.GetComponent<s_PlanetGravity>().pullDir;

				newPos += gDir * force * timeStep;
			}*/

			/*Instantiate(obj, newPos, Quaternion.identity);*/
//		}
//	}

//	bool TravelTrajectorySegment(Vector2 startPos, Vector2 direction, float speed, float timePerSegmentInSeconds, List<Vector2> positions)
//	{
		/* StartPos = latest position
		 * direction = the direction we get from (currentPos - startPos)
		 * speed = velocity magnitude
		 * timePerSegmentInSeconds = the time we fly between each segment part
		 * + gravity * timeperSegmentInSeconds = the amount of gravity to add to the position for each segment in seconds
		 * */
		//Vector2 newPos = startPos + direction * speed * timePerSegmentInSeconds + Physics2D.gravity * timePerSegmentInSeconds;
//	}
}
