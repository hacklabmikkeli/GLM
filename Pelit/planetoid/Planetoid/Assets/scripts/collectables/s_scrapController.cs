using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controller for the scraps in the game.
/// This script handles the collection of the scraps and
/// deletion of scraps that have been collected prior.
/// </summary>
public class s_scrapController : MonoBehaviour
{
	//This is used to raise an event every time scrap is collected.
	public delegate void ScrapCollection(int scrapAmount);
	public static event ScrapCollection OnScrapCollection;

	public Mesh largeScrapModel;
	public Mesh smallScrapModel;

	public int scrapID;
	public int scrapValue;
	private float scrapResetTime;


	private string meshPath = "Meshes/";
	private string levelName;
	private int levelID;
	private s_PointGiverController pointScript;

	private int smallScrapValue = 10;
	private int mediumScrapValue = 25;
	private int largeScrapValue = 50;


    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
	{	
		// Set the value to minimum of 10
		if (scrapValue < smallScrapValue) scrapValue = smallScrapValue;

		smallScrapModel = (Mesh)Resources.Load(meshPath + "scrap_small", typeof(Mesh));
		largeScrapModel = (Mesh)Resources.Load(meshPath + "scrap_medium", typeof(Mesh));

		if (scrapValue < mediumScrapValue) {
			transform.GetChild(0).GetComponent<MeshFilter>().mesh = smallScrapModel;
		} else {
			transform.GetChild(0).GetComponent<MeshFilter>().mesh = largeScrapModel;
		}

		levelName = Application.loadedLevelName;
		levelID = Application.loadedLevel;
		scrapResetTime = GameObject.FindGameObjectWithTag("SceneInfo").GetComponent<s_SceneController>().scrapResetTime;

		if (GameObject.FindGameObjectWithTag("PointGiver") != null)
		{
			pointScript = GameObject.FindGameObjectWithTag("PointGiver").GetComponent<s_PointGiverController>();
		}

        DeleteCollected();
	}

	/// <summary>
	/// This event is raised when player touches a scrap in the level.
	/// This handles the collection of the scrap.
	/// </summary>
	/// <param name="col">Collider2D that collides with scrap</param>
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{

            //TODO: Add sound when collectable is collected. 

            if (scrapValue < mediumScrapValue) {
				s_SoundScript.audioSource.PlaySoundAtPosition(s_SoundScript.audioSource.scrapSmallPickup, transform.position);
			} else {
				s_SoundScript.audioSource.PlaySoundAtPosition(s_SoundScript.audioSource.scrapLargePickup, transform.position);
			}

			//Increases the total amount of scrap collected
			//PlayerData.current.scrapCollected++;

			if (pointScript != null)
			{
				pointScript.UsePointPopup(scrapValue, transform.position);
			}

			PlayerData.current.AddScrapPoints(scrapValue);
			//PlayerData.current.AddCollectedScrap(levelName, levelID, scrapID);
			PlayerData.current.AddCollectedScrap(levelID, scrapID);
			//This one updates the UI for the collected scrap
			if (OnScrapCollection != null)
			{
				OnScrapCollection(PlayerData.current.GetTotalScrapCollected());
			}

			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Deletes the collected scrap from scene if reset time is up and scraps in level have been collected.
	/// </summary>
	void DeleteCollected()
	{
		if (PlayerData.current.DeleteCollected(levelID, scrapID, scrapResetTime) == true)
		{
			Destroy(gameObject);
		}
	}
}
