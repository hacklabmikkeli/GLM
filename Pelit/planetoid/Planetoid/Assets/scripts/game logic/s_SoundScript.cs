using UnityEngine;
using System.Collections;

/// <summary>
/// Handles playing sounds on characters and objects.
/// </summary>

public class s_SoundScript : MonoBehaviour
{
	public static s_SoundScript audioSource;

	[Header("Player movement sounds")]
	public AudioClip jumpSound;
	public AudioClip thumpSound;
	
	[Header("Power Up sounds")]
	public AudioClip grapplingHookSound;

	[Header("Planet sounds")]
	public AudioClip enterGravityField;
	public AudioClip burnSound;
	public AudioClip breakingPlanet;

	[Header("Scrap sounds")]
	public AudioClip scrapSmallPickup;
	public AudioClip scrapLargePickup;

	[Header("Background music")]
	public AudioClip menuMusic;
	public AudioClip[] levelMusic;

	private AudioSource sourceSFX;
	private AudioSource sourceMusic;

	// Use this for initialization
	void Awake ()
	{
		// Make this a singleton
		DontDestroyOnLoad(gameObject);
		
		// If we find another gamecontroller object, destroy it.
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		} 
		else 
		{
			// This is the only AudioController. Assign the script.
			audioSource = this;
			sourceSFX = transform.FindChild("SFX").GetComponent<AudioSource>();
			sourceMusic = transform.FindChild("Music").GetComponent<AudioSource>();
		}
		//NextLevelSong();
	}


	void OnEnable()
	{
		s_GameController.OnStateChange += OnStateChange;
	}

	void OnDisable()
	{
		s_GameController.OnStateChange -= OnStateChange;
	}

	/// <summary>
	/// This is raised when game state changes.
	/// </summary>
	void OnStateChange(GameState newState)
	{
		if (newState == GameState.Menu)
		{
			PlayMenuMusic();
		}
	}

	/// <summary>
	/// Plays the sound at the given position.
	/// </summary>
	/// <param name="sound">Sound.</param>
	/// <param name="position">Position.</param>
	public void PlaySoundAtPosition(AudioClip sound, Vector3 position)
	{
		Vector3 pos = new Vector3(position.x, position.y, transform.position.z);
		AudioSource.PlayClipAtPoint(sound, pos);
	}

	/// <summary>
	/// Loads a sound into the AudioSource and plays it.
	/// Pitch is randomized if changePitch is true.
	/// </summary>
	/// <param name="sound">Sound.</param>
	/// <param name="position">Position.</param>
	/// <param name="changePitch">If set to <c>true</c> change pitch.</param>
	public void PlayLoadedSoundAtPosition(AudioClip sound, Vector3 position, bool changePitch)
	{
		// Load the given audioclip
		sourceSFX.clip = sound;
		
		// Change the pitch if wanted
		if (changePitch) sourceSFX.pitch = Random.Range(0.5f, 1.5f);
		else sourceSFX.pitch = 1;
		
		// Change the position of the audio manager
		sourceSFX.transform.position = position;
		
		// Play the sound
		sourceSFX.PlayOneShot(sound);
	}

	public void PlayMenuMusic()
	{
		sourceMusic.clip = menuMusic;
		sourceMusic.Play();
	}

	public void PlayLevelMusic(int levelMusicID)
	{
		sourceMusic.clip = levelMusic[levelMusicID];
		sourceMusic.Play();
	}

	void NextLevelSong()
	{
		//Changes the level music after the first ten levels.
		int i = Application.loadedLevel; //Get the scene id in buildsettings
		int nextLevelID = i + 1;

		if (nextLevelID == 11)
		{
			PlayLevelMusic(1);
		}
	}
}
