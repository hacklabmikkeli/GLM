using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class s_PointGiverController : MonoBehaviour
{
	public List<s_ScrapPopupText> pointPool;

	public List<s_TaskPointController> taskpointPool;
	private int nextPointIndex = 0;
	public ParticleSystem ps;



	void Start()
	{
		taskpointPool = new List<s_TaskPointController>();
	}

	public void UsePointPopup(int scrapAmount, Vector3 position)
	{
		pointPool[nextPointIndex].gameObject.SetActive(true);
		pointPool[nextPointIndex].UsePopupText(scrapAmount, position);

		transform.position = position;
		ps.Emit(scrapAmount / 2);

		nextPointIndex++;

		if (nextPointIndex==pointPool.Count)
		{
			nextPointIndex = 0;
		}
	}

	public void ShowTaskPoints(Transform pointText, int scrapAmount)
	{
		s_TaskPointController taskTextScript = pointText.GetComponent<s_TaskPointController>();
		taskTextScript.SetValue(scrapAmount);

		if (taskpointPool.Count == 0)
		{
			taskpointPool.Add(taskTextScript);
			taskTextScript.UsePopupText();
		}
		else
		{
			taskpointPool.Add(taskTextScript);
		}
	}

	public void NextTaskPointPopup()
	{
		if (taskpointPool.Count > 0)
		{
			taskpointPool.RemoveAt(0);

			if (taskpointPool.Count > 0)
			{
				taskpointPool[0].UsePopupText();
			}
		}
	}
}
