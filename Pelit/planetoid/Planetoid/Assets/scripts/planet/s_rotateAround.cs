using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class s_rotateAround : MonoBehaviour
{	
	public Animator levelPopupAnimator;
	public Text levelName;
	public float angle;
	public float rotationDampening;
	public float swipeRotationSensitivity;
	public float lockRotationSpeed;

	private Vector3 currentPosition;
	private Vector3 deltaPosition;
	private Vector3 lastPosition;

	public bool rotating;
	private Vector3 target;
	private float angleToLock;
	private bool lockedIn = true;
	private s_LevelPlanetController lockedLevel;


	public float deltaPositionY;

	void OnEnable()
	{
		s_MenuControls.OnTouch += OnTouch;
		s_MenuControls.OnTouchBegan += OnTouchBegan;
		s_MenuControls.OnTouchEnded += OnTouchEnded;
	}

	void OnDisable()
	{
		s_MenuControls.OnTouch -= OnTouch;
		s_MenuControls.OnTouchBegan -= OnTouchBegan;
		s_MenuControls.OnTouchEnded -= OnTouchEnded;
	}

	/// <summary>
	/// Is raised on touch began event.
	/// </summary>
	/// <param name="deltaPositionY">Delta position y.</param>
	void OnTouchBegan(float deltaPositionY)
	{
		angle = 0;
	}

	/// <summary>
	/// Is raised on touch event moved or touching stationary.
	/// </summary>
	/// <param name="deltaPositionY">Delta position y.</param>
	void OnTouch(float deltaPositionY)
	{
		angle -= deltaPositionY * swipeRotationSensitivity * Time.deltaTime;
	}

	/// <summary>
	/// Is raised on touch ended event.
	/// </summary>
	/// <param name="deltaPositionY">Delta position y.</param>
	void OnTouchEnded(float deltaPositionY)
	{
		rotating = true;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		//Used to lock to closest level planet if rotation speed is small enough
		if (rotating && Mathf.Abs(angle) == 0f)
		{
			target = new Vector3(0, 0, angleToLock);
			
			float deltaAngle = Mathf.LerpAngle(transform.eulerAngles.z, target.z, Time.deltaTime * lockRotationSpeed);
			float angleDistance = CalculateAngleDistance(deltaAngle);
			
			if (angleDistance > 1f)
			{
				transform.eulerAngles = new Vector3(0, 0, deltaAngle);
				lockedIn = false;
			}
			else
			{
				transform.eulerAngles = target;
				lockedIn = true;
				rotating = false;
			}
		}

		Dampen();

		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + angle);
	}

	/// <summary>
	/// Calculates the angle distance.
	/// </summary>
	/// <returns>The angle distance.</returns>
	/// <param name="deltaAngle">Delta angle.</param>
	float CalculateAngleDistance(float deltaAngle)
	{
		float angleDistance;

		if (target.z < 0)
		{
			angleDistance = deltaAngle - (360 + target.z);
		}
		else if(target.z > 0)
		{
			angleDistance =  deltaAngle - target.z;
		}
		else
		{
			angleDistance = Mathf.DeltaAngle(deltaAngle, target.z);
		}
		
		angleDistance = Mathf.Abs(angleDistance);

		return angleDistance;
	}

	/// <summary>
	/// Lasts the passed angle.
	/// </summary>
	/// <param name="angle">Angle.</param>
	public void LastPassedAngle(float angle, s_LevelPlanetController level)
	{
		angleToLock = angle;
		lockedLevel = level;
		levelName.text = lockedLevel.levelName;
	}

	/// <summary>
	/// Dampen this instance.
	/// </summary>
	void Dampen()
	{
		if (angle < 0)
		{
			angle += rotationDampening * Time.deltaTime;
			if (angle > 0.001f)
			{
				angle = 0;
			}
		}
		else if(angle > 0)
		{
			angle -= rotationDampening * Time.deltaTime;
			if (angle < 0.001f)
			{
				angle = 0;
			}
		}
	}

	//NÄMÄ KAKS FUNKTIOTA HEVONKUUSEEN TÄSTÄ SCRIPTISTÄ!!!

	public void OpenLevelMenu()
	{
		if (lockedIn == true)
		{
			levelPopupAnimator.SetTrigger("OpenClose");
		}
	}

	public void LoadLevel()
	{
		Debug.Log(lockedLevel.levelNumber);
		Application.LoadLevel(lockedLevel.levelNumber);
	}
}
