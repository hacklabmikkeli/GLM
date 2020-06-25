using UnityEngine;
using System.Collections;

public class s_LookAtGoal : MonoBehaviour
{	
	public float angleOffset;
	private Transform target;
	private Vector3 direction;
	private float angle;

	void Start()
	{
		if (GameObject.FindGameObjectWithTag("Finish") != null)
		{
			target = GameObject.FindGameObjectWithTag("Finish").transform;
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	void Update()
	{
		if (target != null)
		{
			direction = (target.position - transform.position);    
			angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, angle-angleOffset);
		}

	}
}
