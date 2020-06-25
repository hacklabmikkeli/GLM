using UnityEngine;
using System.Collections;

public class s_MenuControls : MonoBehaviour
{
	//This could be used to raise and event every time gamestate is changed.
	public delegate void TouchEvent(float deltaPositionY);
	public static event TouchEvent OnTouch;
	public static event TouchEvent OnTouchBegan;
	public static event TouchEvent OnTouchEnded;

	void OnEnable()
	{
		s_MenuCameraController.OnMoveToLevels += AddRecognizers;
		s_MenuCameraController.OnMoveToMainMenu += RemoveRecognizers;
	}

	void OnDisable()
	{
		s_MenuCameraController.OnMoveToLevels -= AddRecognizers;
		s_MenuCameraController.OnMoveToLevels -= RemoveRecognizers;
	}

	void AddRecognizers()
	{
		TouchKit.removeAllGestureRecognizers();





		//-------------ANY TOUCH RECOGNIZER---------------------
		var anyTouchRecognizer = new TKAnyTouchRecognizer(new TKRect(Screen.width, Screen.height, new Vector2(320/2, 180/2)));

		anyTouchRecognizer.onEnteredEvent += ( r ) =>
		{
			if (OnTouchBegan != null)
			{
				OnTouchBegan(0);
			}
		};

		TouchKit.addGestureRecognizer( anyTouchRecognizer );
		//------------------------------------------------------





		//------------------PAN RECOGNIZER----------------------
		var panRecognizer = new TKPanRecognizer(0);

		panRecognizer.gestureRecognizedEvent += ( r ) =>
		{
			if (OnTouch != null)
			{
				OnTouch(panRecognizer.deltaTranslation.y);
			}
		};

		panRecognizer.gestureCompleteEvent += ( r ) =>
		{
			if (OnTouchEnded != null)
			{
				OnTouchEnded(0);
			}
		};

        TouchKit.addGestureRecognizer( panRecognizer );
        // -----------------------------------------------------





	}

	public void RemoveRecognizers()
	{
		TouchKit.removeAllGestureRecognizers();
	}
}
