using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Script that handles saving and loading of the PlayerData.
/// It is static because we want to call it from anywhere in the project.
/// </summary>
public static class SaveLoadData
{
	//This is the data to be saved.
	public static PlayerData data;

	/// <summary>
	/// Call this function if you want to save the game.
	/// </summary>
	public static void Save()
	{
		data = PlayerData.current;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/saveData.dat");
		bf.Serialize(file, data);
		file.Close();
		//Debug.Log("Saving done.");
	}

	/// <summary>
	/// Call this function if you want to load the game. 
	/// It is probably enough to call this once when the application starts.
	/// </summary>
	public static void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/saveData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);
			data = (PlayerData)bf.Deserialize(file);
			file.Close();
			PlayerData.current = data;
			//Debug.Log("Loading done.");
		}
	}
}