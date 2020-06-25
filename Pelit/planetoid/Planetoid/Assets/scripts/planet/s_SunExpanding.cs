using UnityEngine;
using System.Collections;

public class s_SunExpanding : MonoBehaviour
{
	public float startDelay;
	public float scaleRate;
	public float targetScaleMultiplier;

	private Vector3 targetScale;
	private float timer = 0;

	// Use this for initialization
	void Start ()
	{
		targetScale = transform.localScale * targetScaleMultiplier;
		targetScale = new Vector3(targetScale.x, targetScale.y, 3f);
		scaleRate = scaleRate * 0.001f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer = timer + Time.deltaTime;

		if (timer > startDelay)
		{
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, scaleRate * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			col.GetComponent<s_PlayerControls>().KillPlayer(CauseOfDeath.Burn);
		}
	}
}
