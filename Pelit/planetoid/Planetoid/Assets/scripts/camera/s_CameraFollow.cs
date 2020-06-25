using UnityEngine;
using System.Collections;

public class s_CameraFollow : MonoBehaviour {

	private GameObject target;
	public bool rotateWithTarget = false;
	public bool lookAtIsActive;
	public bool following;
	public float followSpeed;

	private float playerSpeed;
	private Rigidbody2D playerRigidbody;
	private s_PlayerControls playerScript;

	void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player");
		playerScript = target.GetComponent<s_PlayerControls>();
		if (target != null)
		{
			playerRigidbody = target.GetComponent<Rigidbody2D>();
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (target != null && following == true)
		{
			if (Camera.main.orthographic == false && lookAtIsActive == true) 
			{
				transform.LookAt(target.transform.position);
			}
			else
			{
				if (Camera.main.orthographic == true)
				{
					transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, -10);
				}
				else
				{
					playerSpeed = playerRigidbody.velocity.magnitude;
					followSpeed = playerSpeed;
					Vector3 targetPos = new Vector3 (target.transform.position.x, target.transform.position.y, Camera.main.transform.position.z);
					transform.position = Vector3.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);
					//transform.position = targetPos;
				}

				
				if (rotateWithTarget)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, 0.01f);
				}
			}
		}

		//NOT GOOD! QUICK FIX AND SHOULD BE DONE DIFFERENTLY IN FINISHED PRODUCT
		if (playerScript.CheckPlayerState() != PlayerState.Grounded)
		{
			StartFollowing();
		}
	}

	public void StartFollowing()
	{
		following = true;
	}

	public void StopFollowing()
	{
		if (target.GetComponent<s_PlayerControls>().CheckPlayerState() == PlayerState.Grounded)
		{
			following = false;
		}
	}
}
