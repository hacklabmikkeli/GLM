using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_ReadAllScenes : MonoBehaviour
{
	public List<string> sceneList;
	public string nextLevel;
	public string menuLevel;

	private string[] scenes;

	#if UNITY_EDITOR
	private static List<string> ReadNames()
	{
		List<string> temp = new List<string>();
		foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
		{
			if (S.enabled)
			{
				string name = S.path.Substring(S.path.LastIndexOf('/')+1);
				name = name.Substring(0,name.Length-6);
				temp.Add(name);
			}
		}
		return temp;
	}
	[UnityEditor.MenuItem("CONTEXT/ReadSceneNames/Update Scene Names")]
	private static void UpdateNames(UnityEditor.MenuCommand command)
	{
		s_ReadAllScenes context = (s_ReadAllScenes)command.context;
		context.sceneList = ReadNames();
	}	
	
	private void Reset()
	{
		sceneList = ReadNames();
	}
	#endif
	
}

