using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Task
{
	private int taskID;
	private string taskName;
	private bool isComplete;
	private int taskValue;

	public Task(int taskID, string taskName, int taskValue)
	{
		this.taskID = taskID;
		this.taskName = taskName;
		this.isComplete = false;
		this.taskValue = taskValue;
	}

	public void SetComplete()
	{
		this.isComplete = true;
	}

	public bool IsComplete()
	{
		return this.isComplete;
	}

	public string GetName()
	{
		return this.taskName;
	}

	public int GetID()
	{
		return this.taskID;
	}
}
