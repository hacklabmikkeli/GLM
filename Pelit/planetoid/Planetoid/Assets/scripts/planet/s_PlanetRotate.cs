using UnityEngine;
using System.Collections;

public class s_PlanetRotate : MonoBehaviour {

	// Self rotation
	[Header("Self rotation options")]
	public bool rotateSelf = false;
	public float selfRotateX = 0f;
	public float selfRotateY = 0f;
	public float selfRotateZ = 0f;

	// Self shaking
	[Header("Self shaking options")]
	public bool shakeSelf = false;
	public float postiveShakeValue = 0.01F;
	public float negativeShakeValue = -0.01F;

	// Rotate around some other object
	[Header("Rotating around other object options")]
	public bool rotateAroundOtherObject = false;
	public Transform otherObject;
	[Tooltip("To keep the object on 2D plane, rotate only around Z axis!")]
	public Vector3 axis;
	public float angle;

	// Update is called once per frame
	void Update () {
		if (rotateSelf) {
			Vector3 rotation = new Vector3(selfRotateX, selfRotateY, selfRotateZ);
			RotateSelf(rotation);
		}

		if (rotateAroundOtherObject) {
			RotateAround();
		}
		if (shakeSelf) {
		ShakeSelf();
		}
	}

	void RotateSelf(Vector3 rotation) {
		transform.Rotate(rotation * Time.deltaTime);
	}

	void ShakeSelf() {
		float xAxisShake = Random.Range(postiveShakeValue, negativeShakeValue);
		float yAxisShake = Random.Range(postiveShakeValue, negativeShakeValue);
		transform.position += new Vector3(xAxisShake, yAxisShake);
	}

	void RotateAround () {
		transform.RotateAround(otherObject.position, axis, angle * Time.deltaTime);
	}

}

