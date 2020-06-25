using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class s_RotateOnTouch : MonoBehaviour
{
	public delegate void PlanetLocked(s_LevelPlanetController levelScript);
	public static event PlanetLocked OnPlanetLock;

	public Animator levelPopupAnimator;
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

	public s_LevelInfoController levelScript;
	
	public float deltaPositionY;
	
	void OnEnable()
	{
		SubscribeMethodsToEvents();
	}
	
	void OnDisable()
	{
		UnsubscribeMethodsFromEvents();
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
		if (Mathf.Abs(angle) > 0)
		{
			levelPopupAnimator.SetBool("ToggleShow", false);
		}

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
				if (OnPlanetLock != null)
				{
					OnPlanetLock(lockedLevel);
				}
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
	}
	
	/// <summary>
	/// Dampen this instance.
	/// </summary>
	void Dampen()
	{
		if (angle < 0)
		{
			angle += rotationDampening * Time.deltaTime;
			if (angle > 0.0001f)
			{
				angle = 0;
			}
		}
		else if(angle > 0)
		{
			angle -= rotationDampening * Time.deltaTime;
			if (angle < 0.0001f)
			{
				angle = 0;
			}
		}
	}

	void SubscribeMethodsToEvents()
	{
		s_MenuControls.OnTouch += OnTouch;
		s_MenuControls.OnTouchBegan += OnTouchBegan;
		s_MenuControls.OnTouchEnded += OnTouchEnded;
		s_LevelPlanetController.OnPlanetCollision += LastPassedAngle;
	}

	void UnsubscribeMethodsFromEvents()
	{
		s_MenuControls.OnTouch -= OnTouch;
		s_MenuControls.OnTouchBegan -= OnTouchBegan;
		s_MenuControls.OnTouchEnded -= OnTouchEnded;
		s_LevelPlanetController.OnPlanetCollision -= LastPassedAngle;
	}

	public void ResetProgression()
	{
		s_LevelPlanetController[] levelTemp = GetComponentsInChildren<s_LevelPlanetController>();

		for (int i = 0; i < levelTemp.Length; i++)
		{
			levelTemp[i].SetUpLevelInfo();
		}

		transform.eulerAngles = new Vector3(0, 0, 0);
	}
}
