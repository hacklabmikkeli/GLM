using UnityEngine;
using System.Collections;

public class s_ScrapRotate : MonoBehaviour
{
	private float rotateSpeed;

	void Start()
	{
		rotateSpeed = Random.Range(2f, 6f);
	}

	void Update ()
	{
		transform.Rotate(0f, rotateSpeed, 0f);
	}
}
