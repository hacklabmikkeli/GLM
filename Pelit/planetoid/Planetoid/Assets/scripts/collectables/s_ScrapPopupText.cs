using UnityEngine;
using System.Collections;

public class s_ScrapPopupText : MonoBehaviour
{
	public bool inUse;
	public float speed;
	public float expansionSpeed;
	public float colorFadeSpeed;

	private TextMesh numberText;

	private Vector3 origPos;
	private Vector3 origScale;

	void Start()
	{
		numberText = GetComponent<TextMesh>();
		origPos = transform.position;
		origScale = transform.localScale;
	}

	void Update()
	{
		if (inUse == true)
		{
			transform.Translate(Vector3.up * speed * Time.deltaTime);
			transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;
			numberText.color = new Color(numberText.color.r, numberText.color.g, numberText.color.b, numberText.color.a - colorFadeSpeed * Time.deltaTime);

			if (transform.localScale.magnitude >= (origScale * 2f).magnitude)
			{
				inUse = false;
				Reset();
			}
		}
	}

	public void UsePopupText(int scrapAmount, Vector3 position)
	{
		numberText = GetComponent<TextMesh>();
		numberText.text = "+" + scrapAmount.ToString();
		transform.position = position;
		inUse = true;
	}

	public void Reset()
	{
		transform.position = origPos;
		transform.localScale = origScale;
		numberText.color = new Color(numberText.color.r, numberText.color.g, numberText.color.b, 1f);
		gameObject.SetActive(false);
	}
}
