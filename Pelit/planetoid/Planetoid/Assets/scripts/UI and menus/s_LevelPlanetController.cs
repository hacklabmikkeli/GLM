using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class s_LevelPlanetController : MonoBehaviour
{
	public delegate void CollisionEvent(float planetAngle, s_LevelPlanetController levelScript);
	public static event CollisionEvent OnPlanetCollision;

	public string levelName;
	public int levelNumber;
	public int scrapAmount;
	public string[] tasks = new string[3];

	private float planetAngle;

	public List<int> levelIDs;

	private MeshRenderer meshRenderer;
	public Material[] materials;
	public TextMesh levelNumberText;

	public bool isUnlocked;

	void Start()
	{
		levelNumberText = transform.GetChild(0).GetComponent<TextMesh>();
		meshRenderer = GetComponent<MeshRenderer>();
		CalculatePlanetAngle();
		SetUpLevelInfo();
	}

	void OnTriggerEnter2D()
	{
		if (OnPlanetCollision != null)
		{
			OnPlanetCollision(planetAngle, GetComponent<s_LevelPlanetController>());
		}
	}

	void CalculatePlanetAngle()
	{
		int multipier = levelNumber - 1;

		planetAngle = -(multipier * 18);
	}

	public void SetUpLevelInfo()
	{
		int lastLevelToUnlock = PlayerData.current.HighestLevelPassed() + 1;

		//Debug.Log(lastLevelToUnlock);

		Level level = PlayerData.current.ReturnLevelByID(levelNumber);

		List<Task> temp = PlayerData.current.GetLevelTasks(levelNumber);

		if (temp != null)
		{
			for (int i = 0; i < temp.Count; i++)
			{
				tasks[i] = temp[i].GetName();
			}
		}
		
		if (level != null)
		{
			levelName = "Level " + level.ReturnLevelID();
			//scrapAmount = level.collectedScraps.Count;
			scrapAmount = PlayerData.current.ScrapAmountCollected(levelNumber);
			meshRenderer.material = materials[0];
			isUnlocked = true;
		}
		else if ((level == null && levelNumber == lastLevelToUnlock) || levelNumber == 1)
		{
			levelName = "Level " + levelNumber;
			scrapAmount = 0;
			meshRenderer.material = materials[0];
			isUnlocked = true;
		}
		else
		{
			meshRenderer.material = materials[1];
			levelNumberText.color = new Color(levelNumberText.color.r, levelNumberText.color.g, levelNumberText.color.b, 0.1f);
			isUnlocked = false;
		}
	}
}
