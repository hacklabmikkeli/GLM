using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class s_TaskPointController : MonoBehaviour
{
	public Text taskPointText;
	private bool inUse;
	public float expansionSpeed;
	public float speed;
	public float popupLifeTime;

	private RectTransform rectTransform;
	private Vector3 origPos;
	private Vector3 origScale;


	private float timer;
	private int taskValue = 0;
	private s_PointGiverController pointGiverScript;

	void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		origPos = rectTransform.position;
		origScale = rectTransform.localScale;

		if (GameObject.FindGameObjectWithTag("PointGiver") != null)
		{
			pointGiverScript = GameObject.FindGameObjectWithTag("PointGiver").GetComponent<s_PointGiverController>();
		}
		else
		{
			taskPointText.enabled = false;
		}
	}

	void Update()
	{
		if (inUse == true)
		{
			timer += Time.unscaledDeltaTime;

			rectTransform.Translate(Vector3.up * speed * Time.unscaledDeltaTime);
			rectTransform.localScale += Vector3.one * expansionSpeed * Time.unscaledDeltaTime;

			if (timer >= popupLifeTime)
			{
				timer = 0;
				Reset();
			}
		}
	}

	public void SetValue(int newValue)
	{
		taskValue = newValue;
		taskPointText.text = "+" + taskValue.ToString();
	}

	public void UsePopupText()
	{
		//taskPointText.enabled = true;
		taskPointText.color = new Color(taskPointText.color.r, taskPointText.color.g, taskPointText.color.b, 1f);
		taskPointText.text = "+" + taskValue.ToString();
		inUse = true;
	}

	void Reset()
	{
		inUse = false;
		rectTransform.position = origPos;
		rectTransform.localScale = origScale;
		//taskPointText.enabled = false;
		pointGiverScript.NextTaskPointPopup();
	}
}
