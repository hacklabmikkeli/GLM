using UnityEngine;
using System.Collections;

public class s_CountingTask : MonoBehaviour
{
	public enum CountType{SameOrLessThan, SameOrMoreThan, ExactlyThat};
	public CountType countType;

	public enum CountedEvent{Jumps, PlanetsVisited, RepelPlanetsTouched, RestartsUsed};
	public CountedEvent countedEvent;

	//This could be used to raise and event every time gamestate is changed.
	public delegate void CountTaskEvent(GameObject taskTitle);
	public static event CountTaskEvent OnTaskComplete;

	public int requiredCount;

	public int taskCounter;

	private static int restartCount = -1;
	private static bool isLevelCleared = false;

	private int taskID;

	void Start()
	{
		taskID = GetComponent<s_TaskGeneral>().taskID;
	}
	
	void OnEnable()
	{
		switch (countedEvent)
		{
		case CountedEvent.Jumps:
			taskCounter = 0;
			s_PlayerControls.OnEventBegan += Count;
			break;
		case CountedEvent.PlanetsVisited:
			//Using -1 the starting planet is not counted as a planet landed.
			taskCounter = -1;
            //s_PlanetCoreCollide.OnEventBegan += CountPlanets;
			break;
		case CountedEvent.RepelPlanetsTouched:
			taskCounter = 0;
			s_PlanetGravity.OnEventBegan += Count;
			break;
		case CountedEvent.RestartsUsed:
			//taskCounter = -1;
			break;
		}

		s_GameController.OnStateChange += OnStateChange;
	}

	void OnDisable()
	{
		switch (countedEvent)
		{
		case CountedEvent.Jumps:
			s_PlayerControls.OnEventBegan -= Count;
			break;
		case CountedEvent.PlanetsVisited:
            //s_PlanetCoreCollide.OnEventBegan -= CountPlanets;
			break;
		case CountedEvent.RepelPlanetsTouched:
			s_PlanetGravity.OnEventBegan -= Count;
			break;
		}

		s_GameController.OnStateChange -= OnStateChange;
	}
	

    void Count()
	{
        taskCounter++;
	}

    void CountPlanets(PlanetType planetType)
    {
        if (planetType == PlanetType.Normal || planetType == PlanetType.Destructive)
        {
            taskCounter++;
        }
    }

	void CountRestarts(GameState newState)
	{
		if (newState == GameState.PlayingLevel && countedEvent == CountedEvent.RestartsUsed && isLevelCleared == false)
		{
			restartCount++;
			taskCounter = restartCount;
		}
	}
	
	void OnStateChange (GameState newState)
	{
		if (newState == GameState.PassedLevel)
		{
			switch (countType)
			{
			case CountType.ExactlyThat:
				if (taskCounter == requiredCount)
				{
					OnTaskComplete(gameObject);
				}
				break;
			case CountType.SameOrMoreThan:
				if (taskCounter >= requiredCount)
				{
					OnTaskComplete(gameObject);
				}
				break;
			case CountType.SameOrLessThan:
				if (taskCounter <= requiredCount)
				{
					OnTaskComplete(gameObject);
				}
				break;
			}

			isLevelCleared = true;
		}

		//Restart counter
		CountRestarts(newState);
	}
}
