using UnityEngine;
using System.Collections;

public class s_ConditionTask : MonoBehaviour
{
	public delegate void ConditionTaskEvent(GameObject taskTitle);
	public static event ConditionTaskEvent OnTaskComplete;

	public enum ConditionType{Live, Die, Checkpoint};
	public ConditionType conditionType;


	public enum RequiredCheckpoints{Checkpoint1, Checkpoint2, Both};

	[Header("Checkpoint type related:")]
	public RequiredCheckpoints requiredCheckpoints;
	
	public static bool hasDied = false;
	
	private bool isConditionMet = false;
	private int checkpointsPassed;

	void OnEnable()
	{
		s_GameController.OnStateChange += OnStateChange;
	}
	void OnDisable()
	{
		s_GameController.OnStateChange -= OnStateChange;
	}

	void OnStateChange (GameState newState)
	{
		switch (newState)
		{
		case GameState.PassedLevel:

			HasPlayerDied();

			if (isConditionMet == true)
			{
				OnTaskComplete(gameObject);
			}
			break;
		case GameState.FailedLevel:

			hasDied = true;

			if (conditionType == ConditionType.Die)
			{
				OnTaskComplete(gameObject);
			}
			break;
		}
	}

	void HasPlayerDied()
	{
		if (conditionType == ConditionType.Live && hasDied == false)
		{
			isConditionMet = true;
		}
		else if (conditionType == ConditionType.Die && hasDied == true)
		{
			isConditionMet = true;
		}
	}

	public void CheckpointPassed(string checkpointName)
	{

		checkpointsPassed++;
		if (conditionType == ConditionType.Checkpoint)
		{
			switch (requiredCheckpoints)
			{
			case RequiredCheckpoints.Checkpoint1:
				if (checkpointName == "Checkpoint 1")
				{
					isConditionMet = true;
					Debug.Log("Checkpoint reached");
				}
				break;
			case RequiredCheckpoints.Checkpoint2:
				if (checkpointName == "Checkpoint 2")
				{
					isConditionMet = true;
					Debug.Log("Checkpoint reached");
				}
				break;
			case RequiredCheckpoints.Both:
				if (checkpointsPassed == 2)
				{
					isConditionMet = true;
				}
				break;
			}
		}
	}
}
