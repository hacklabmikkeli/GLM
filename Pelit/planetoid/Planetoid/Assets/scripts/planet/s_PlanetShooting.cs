using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_PlanetShooting : MonoBehaviour
{
	public GameObject projectilePrefab;
	public float shootingDelayAtStart;
	public float shootingRateInSeconds;
	public float shootingForce;

	private bool isShooting = false;
	private List<GameObject> projectiles = new List<GameObject>();

    void Start()
    {
        isShooting = true;
        StartCoroutine("ShootProjectiles");
    }

	/*public void StopShooting()
	{
		isShooting = false;
		StopCoroutine("ShootProjectiles");
	}*/

	IEnumerator ShootProjectiles()
	{
		yield return new WaitForSeconds(shootingDelayAtStart);

		while (isShooting == true)
		{
			GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;

			float xAxis = Random.Range(-1.0f, 1.0f);
			float yAxis = Random.Range(-1.0f, 1.0f);

			Vector2 direction = new Vector2(xAxis, yAxis);
			direction.Normalize();

			projectile.GetComponent<Rigidbody2D>().AddForce(direction * shootingForce);

			projectiles.Add(projectile);

			yield return new WaitForSeconds(shootingRateInSeconds);
		}
	}
}
