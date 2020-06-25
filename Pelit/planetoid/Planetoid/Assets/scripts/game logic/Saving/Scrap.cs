using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Scrap
{
	private int ID;
	private bool isCollected;
	private int value;

	public Scrap()
	{
		ID = 0;
		isCollected = false;
		value = 0;
	}

	public Scrap(int ID, bool isCollected, int value)
	{
		this.ID = ID;
		this.isCollected = isCollected;
		this.value = value;
	}

	public bool IsCollected()
	{
		return isCollected;
	}

	public void SetAsCollected()
	{
		this.isCollected = true;
	}
}