using UnityEngine;
using System.Collections;

public class s_TaskCollidersTouched : MonoBehaviour
{
	public bool hasPlayerTouched;
	//public string name;

	private s_ConditionTask taskScript;

	void Start()
	{
		taskScript = transform.parent.GetComponent<s_ConditionTask>();
		name = gameObject.name;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && hasPlayerTouched == false)
		{
			taskScript.CheckpointPassed(name);
			hasPlayerTouched = true;
		}
	}
}
