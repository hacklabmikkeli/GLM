using UnityEngine;
using System.Collections;

public class s_PanCamera : MonoBehaviour
{
	public float sensitivity;
	public float returnPanSpeed = 1;

	private Transform mainCamera;
	private Vector3 lastPosition;
	private s_PlayerControls playerScript;
	private bool isOutsideBoundaries;
	private Vector3 returnPanTarget;

	// Use this for initialization
	void Start ()
	{
		mainCamera = Camera.main.transform;
		playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerControls>();
		returnPanTarget = new Vector3(playerScript.transform.position.x, playerScript.transform.position.y, transform.position.z);
	}

	void Update()
	{
		if (isOutsideBoundaries == true)
		{
			returnPanTarget = new Vector3(playerScript.transform.position.x, playerScript.transform.position.y, transform.position.z);

			transform.position = Vector3.MoveTowards(transform.position, returnPanTarget, returnPanSpeed * Time.deltaTime);
		}
	}

	public void PanCamera(Vector2 deltaTranslation)
	{
		if (playerScript != null && playerScript.CheckPlayerState() == PlayerState.Grounded && isOutsideBoundaries == false)
		{
			//This is the original version
			float deltaTranslationMultipier = Mathf.Abs(mainCamera.position.z/5f) * Time.deltaTime * sensitivity;
            transform.Translate(-deltaTranslation.x * deltaTranslationMultipier, -deltaTranslation.y * deltaTranslationMultipier, 0);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "CameraBoundaries")
		{
			isOutsideBoundaries = true;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "CameraBoundaries")
		{
			isOutsideBoundaries = false;
		}
	}
}
