using UnityEngine;
using System.Collections;

public class s_ProjectileController : MonoBehaviour
{
	public float lifeTime;

	private float timer;

	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;

		if (lifeTime < timer)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Ground")
		{
			DestroyProjectile();
		}
		if(col.tag == "Player")
		{
			// Find player and send a kill call
			s_PlayerControls playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerControls>();
			playerScript.KillPlayer(CauseOfDeath.Projectile);
		}
	}

	//Make nice destroy effects for the object start here.
	void DestroyProjectile()
	{
		Destroy(gameObject);
	}
}
