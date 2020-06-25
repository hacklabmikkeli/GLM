using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class s_DataSaving : MonoBehaviour
{
	private string path;

	public string saveFileName = "playerSave";
	public PlayerData data;

	public static s_DataSaving dataSavingScript;

	// Use this for initialization
	void Awake () {
		path = Application.persistentDataPath + "/" + saveFileName + ".dat";
		//if (data == null /*s_GameController.gameControllerSingleton == GetComponent<s_GameController>()*/)
		//{
			//Debug.Log("Nymmentii asettellee datoja");
			dataSavingScript = this;
			data = new PlayerData();
			Load();
		//}
	}

	// Saves data to a file
	public void Save() {

		// Create a binary formatter
		BinaryFormatter bf = new BinaryFormatter();

		// Create a new file with the filepath
		FileStream file = File.Create(path);
		//Debug.Log("Opened file for saving: " + path);

		// Put the saved information into the specified file
		bf.Serialize(file, data);

		// Close the file reading
		file.Close();
		//Debug.Log("Finished saving file.");
	}

	// Loads data from a specified file
	public void Load() {

		// TODO: Encrypt the file with a key to make it harder to modify.
		// Check that the file exists already
		if (File.Exists(path)) {
			//Debug.Log("Found existing save file from:" + path);

			// File exists, create binary formatter
			BinaryFormatter bf = new BinaryFormatter();

			// Open the file
			FileStream file = File.Open(path, FileMode.Open);
			//Debug.Log("Opened found save file.");


			// Deserialize the found file object
			// As 'file' is a generic object, we have to cast it back to what we want it to be, here a PlayerData object
			data = (PlayerData) bf.Deserialize(file); 
			//Debug.Log("Deserialized found save file.");


			file.Close();
		}
	}
}

/// <summary>
/// Serializable Player data class for containing the data at runtime.
/// As Monobehaviours should not be used to serialize data (they can cause weird behavior)
/// we need a different non-monobehaviour class for saving.
/// </summary>
/*[Serializable]
class PlayerData {
	public float health;
	public float experience;
}*/
